using System;
using System.Collections.Generic;
using System.IO;

namespace SharpSenses.RealSense.Capabilities {
    public class FaceRecognitionCapability : ICapability {
        private static string StorageFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "SharpSensesDb.bin";
        private const string StorageName = "SharpSensesDb";
        private RecognitionState _recognitionState = RecognitionState.Idle;
        private enum RecognitionState {
            Idle,
            Requested,
            Working,
            Done
        }

        private RealSenseCamera _camera;
        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            using (var faceModule = _camera.Manager.QueryFace()) {
                using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                    moduleConfiguration.detection.maxTrackedFaces = 1;
                    var expressionCofig = moduleConfiguration.QueryExpressions();
                    expressionCofig.Enable();
                    expressionCofig.EnableAllExpressions();

                    var desc = new PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc();
                    desc.maxUsers = 10;
                    desc.isPersistent = true;
                    var recognitionConfiguration = moduleConfiguration.QueryRecognition();
                    recognitionConfiguration.CreateStorage(StorageName, out desc);
                    recognitionConfiguration.UseStorage(StorageName);
                    recognitionConfiguration.SetRegistrationMode(PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode.REGISTRATION_MODE_CONTINUOUS);

                    if (File.Exists(StorageFileName)) {
                        var bytes = File.ReadAllBytes(StorageFileName);
                        recognitionConfiguration.SetDatabaseBuffer(bytes);
                    }
                    recognitionConfiguration.Enable();
                    moduleConfiguration.ApplyChanges();
                }
            }
        }

        private void RecognizeFace(PXCMFaceData faceData, PXCMFaceData.Face face) {
            var rdata = face.QueryRecognition();
            var userId = rdata.QueryUserID();

            switch (_recognitionState) {
                case RecognitionState.Idle:
                    break;
                case RecognitionState.Requested:
                    rdata.RegisterUser();
                    _recognitionState = RecognitionState.Working;
                    break;
                case RecognitionState.Working:
                    if (userId > 0) {
                        _recognitionState = RecognitionState.Done;
                    }
                    break;
                case RecognitionState.Done:
                    SaveDatabase(faceData);
                    _recognitionState = RecognitionState.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _camera.Face.UserId = userId;
        }

        private void SaveDatabase(PXCMFaceData faceData) {
            var rmd = faceData.QueryRecognitionModule();
            var buffer = new Byte[rmd.QueryDatabaseSize()];
            rmd.QueryDatabaseBuffer(buffer);
            File.WriteAllBytes(StorageFileName, buffer);
        }

        public void Loop() {}
        public void Dispose() {}
    }
}