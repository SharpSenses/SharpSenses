using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.RealSense.FaceRecognition {
    public class FaceRecognizer : IFaceRecognizer {
        private RecognitionState _recognitionState = RecognitionState.Idle;

        public void RecognizeFace() {
            _recognitionState = RecognitionState.Requested;
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
            //_camera.Face.UserId = userId;
        }

        private void SaveDatabase(PXCMFaceData faceData) {
            var rmd = faceData.QueryRecognitionModule();
            var buffer = new Byte[rmd.QueryDatabaseSize()];
            rmd.QueryDatabaseBuffer(buffer);
            //File.WriteAllBytes(StorageFileName, buffer);
        }
    }
}
