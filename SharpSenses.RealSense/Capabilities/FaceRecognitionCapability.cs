using System;
using System.Collections.Generic;
using System.IO;

namespace SharpSenses.RealSense.Capabilities {
    public class FaceRecognitionCapability : ICapability {
        private static string StorageFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "SharpSensesDb.bin";
        private const string StorageName = "SharpSensesDb";

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

        

        public void Loop(LoopObjects loopObjects) {}
        public void Dispose() {}
    }
}