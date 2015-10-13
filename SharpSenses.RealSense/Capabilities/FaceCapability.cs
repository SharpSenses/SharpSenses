using System.Collections.Generic;
using System.Linq;
using SharpSenses.RealSense.Util;
using static SharpSenses.PositionHelper;

namespace SharpSenses.RealSense.Capabilities {
    public class FaceCapability : ICapability {
        private RealSenseCamera _camera;
        private PXCMFaceModule _faceModule;
        private PXCMFaceData _faceData;
        public IEnumerable<Capability> Dependencies => new List<Capability>();

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableFace();
            _faceModule = camera.Manager.QueryFace();
            _faceData = _faceModule.CreateOutput();
        }
        public void Loop() {
            _faceData.Update();
            var face = FindFace(_faceData);
            if (face != null) {
                TrackFace(face);
            }
        }

        private PXCMFaceData.Face FindFace(PXCMFaceData faceData) {
            if (faceData.QueryNumberOfDetectedFaces() == 0) {
                _camera.Face.IsVisible = false;
                return null;
            }
            _camera.Face.IsVisible = true;
            return faceData.QueryFaces().First();
        }

        private void TrackFace(PXCMFaceData.Face face) {
            //PXCMFaceData.HeadPosition position;
            //face.QueryPose().QueryHeadPosition(out position);
            //PXCMRectI32 rect;
            //face.QueryDetection().QueryBoundingRect(out rect);
            //var point = new Point3D(rect.x + rect.w / 2, rect.y + rect.h / 2);
            //_camera.Face.Position = CreatePosition(point, new Point3D());
            //_camera.Face.Position = CreatePosition(position.headCenter.ToPoint3D(), );

            PXCMFaceData.LandmarksData landmarksData = face.QueryLandmarks();
            if (landmarksData == null) {
                return;
            }
            PXCMFaceData.LandmarkPoint[] facePoints;
            landmarksData.QueryPoints(out facePoints);
            if (facePoints == null) {
                return;
            }
            foreach (var item in facePoints) {
                switch (item.source.alias) {
                    case PXCMFaceData.LandmarkType.LANDMARK_UPPER_LIP_CENTER:
                        _camera.Face.Mouth.Position = CreatePosition(item.image.ToPoint3D(), item.world.ToPoint3D());
                        break;
                    case PXCMFaceData.LandmarkType.LANDMARK_EYE_LEFT_CENTER:
                        _camera.Face.LeftEye.Position = CreatePosition(item.image.ToPoint3D(), item.world.ToPoint3D());
                        break;
                    case PXCMFaceData.LandmarkType.LANDMARK_EYE_RIGHT_CENTER:
                        _camera.Face.RightEye.Position = CreatePosition(item.image.ToPoint3D(), item.world.ToPoint3D());
                        break;
                    case PXCMFaceData.LandmarkType.LANDMARK_NOSE_TOP:
                        _camera.Face.Position = CreatePosition(item.image.ToPoint3D(), item.world.ToPoint3D());
                        break;
                }
            }
        }

        public void Dispose() {
            _faceModule.SilentlyDispose();
            _faceData.SilentlyDispose();
        }
    }
}