using System;
using System.Collections.Generic;
using System.IO;

namespace SharpSenses.RealSense.Capabilities {
    public class FaceRecognitionCapability : ICapability, IFaceRecognizer {
        private static string StorageFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "SharpSensesDb.bin";
        private const string StorageName = "SharpSensesDb";

        private RealSenseCamera _camera;
        private RecognitionState _recognitionState = RecognitionState.Idle;
        public IEnumerable<Capability> Dependencies => new List<Capability> { Capability.FaceTracking };

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            using (var faceModule = _camera.Manager.QueryFace()) {
                using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                    var desc = new PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc();
                    desc.maxUsers = Int32.MaxValue;
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

        public void Loop(LoopObjects loopObjects) {
            PXCMFaceData.Face face = loopObjects.Get<PXCMFaceData.Face>();
            PXCMFaceData faceData = loopObjects.Get<PXCMFaceData>();

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

        public void RecognizeFace() {
            _recognitionState = RecognitionState.Requested;
        }

        private void SaveDatabase(PXCMFaceData faceData) {
            var rmd = faceData.QueryRecognitionModule();
            var buffer = new Byte[rmd.QueryDatabaseSize()];
            rmd.QueryDatabaseBuffer(buffer);
            File.WriteAllBytes(StorageFileName, buffer);
        }
        public void Dispose() {}
    }
}