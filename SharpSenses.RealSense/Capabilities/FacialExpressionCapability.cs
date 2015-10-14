using System;
using System.Collections.Generic;
using SharpSenses.Gestures;
using FaceExpression = PXCMFaceData.ExpressionsData.FaceExpression;

namespace SharpSenses.RealSense.Capabilities {
    public class FacialExpressionCapability : ICapability {

        public static int ExpressionThreshold = 30;
        public static int SmileThreshold = 40;
        public static int MonthOpenThreshold = 15;
        public static int EyesClosedThreshold = 15;

        private DateTime _lastEyesDirectionDetection;
        private Dictionary<Direction, double> _eyesThresholds = new Dictionary<Direction, double> {
            [Direction.Up] =  10,
            [Direction.Down] = 10,
            [Direction.Left] = 10,
            [Direction.Right] = 10
        };
        private RealSenseCamera _camera;

        public IEnumerable<Capability> Dependencies => new[] {Capability.FaceTracking};

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            using (var faceModule = _camera.Manager.QueryFace()) {
                using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
                    moduleConfiguration.detection.maxTrackedFaces = 1;
                    var expressionCofig = moduleConfiguration.QueryExpressions();
                    expressionCofig.Enable();
                    expressionCofig.EnableAllExpressions();
                    moduleConfiguration.ApplyChanges();
                }
            }
        }

        public void Loop(LoopObjects loopObjects) {
            PXCMFaceData.Face face = loopObjects.Get<PXCMFaceData.Face>();
            PXCMFaceData.ExpressionsData data = face?.QueryExpressions();
            if (data == null) {
                return;
            }
            _camera.Face.Mouth.IsSmiling = CheckFaceExpression(data, FaceExpression.EXPRESSION_SMILE, SmileThreshold);
            _camera.Face.Mouth.IsOpen = CheckFaceExpression(data, FaceExpression.EXPRESSION_MOUTH_OPEN, MonthOpenThreshold);
            _camera.Face.LeftEye.IsOpen = !CheckFaceExpression(data, FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, EyesClosedThreshold);
            _camera.Face.RightEye.IsOpen = !CheckFaceExpression(data, FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, EyesClosedThreshold);
            _camera.Face.EyesDirection = GetEyesDirection(data);
        }

        private Direction GetEyesDirection(PXCMFaceData.ExpressionsData data) {
            if ((DateTime.Now - _lastEyesDirectionDetection).TotalMilliseconds < 500) {
                return _camera.Face.EyesDirection;
            }
            _lastEyesDirectionDetection = DateTime.Now;
            var up = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_UP);
            var down = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_DOWN);
            var left = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_TURN_LEFT);
            var right = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_TURN_RIGHT);

            if (up > _eyesThresholds[Direction.Up]) {
                _eyesThresholds[Direction.Up] = up * 0.7;
                return Direction.Up;
            }
            if (down > _eyesThresholds[Direction.Down]) {
                _eyesThresholds[Direction.Down] = down * 0.7;
                return Direction.Down;
            }
            if (left > _eyesThresholds[Direction.Left]) {
                _eyesThresholds[Direction.Left] = left * 0.7;
                return Direction.Left;
            }
            if (right > _eyesThresholds[Direction.Right]) {
                _eyesThresholds[Direction.Right] = right * 0.7;
                return Direction.Right;
            }
            return Direction.None;
        }

        private bool CheckFaceExpression(PXCMFaceData.ExpressionsData data, FaceExpression faceExpression, int threshold) {
            return GetFaceExpressionIntensity(data, faceExpression) > threshold;
        }

        private int GetFaceExpressionIntensity(PXCMFaceData.ExpressionsData data, FaceExpression faceExpression) {
            PXCMFaceData.ExpressionsData.FaceExpressionResult score;
            data.QueryExpression(faceExpression, out score);
            return score.intensity;
        }

        public void Dispose() { }
    }
}