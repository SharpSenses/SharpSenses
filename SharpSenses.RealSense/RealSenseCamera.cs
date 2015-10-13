using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpSenses.Gestures;
using SharpSenses.Poses;
using SharpSenses.RealSense.Capabilities;
using static SharpSenses.PositionHelper;
using static SharpSenses.RealSense.Errors;
using FaceExpression = PXCMFaceData.ExpressionsData.FaceExpression;

namespace SharpSenses.RealSense {
    public class RealSenseCamera : Camera, IFaceRecognizer {

        public static int ExpressionThreshold = 30;
        public static int SmileThreshold = 40;
        public static int MonthOpenThreshold = 15;
        public static int EyesClosedThreshold = 15;

        private static Dictionary<Capability, ICapability> _availableCapabilities = new Dictionary<Capability, ICapability> {
            [Capability.HandTracking] = new HandTrackingCapability(),
            [Capability.GestureTracking] = new GesturesCapability(),
            [Capability.FaceTracking] = new FaceCapability()
        };

        private List<Capability> _enabledCapabilities = new List<Capability>(); 

        private ISpeech _speech;
        private CancellationTokenSource _cancellationToken;
        private RecognitionState _recognitionState = RecognitionState.Idle;
        public PXCMSenseManager Manager { get; }
        public PXCMSession Session { get; }

        private Dictionary<Direction, double> _eyesThresholds;
        private enum RecognitionState {
            Idle,
            Requested,
            Working,
            Done
        }

        public override int ResolutionWidth {
            get { return 640; }
        }

        public override int ResolutionHeight {
            get { return 480; }
        }

        public override int FramesPerSecond {
            get { return 30; }
        }

        public override ISpeech Speech {
            get { return _speech; }
        }

        public int CyclePauseInMillis { get; set; }

        public RealSenseCamera() {
            Session = PXCMSession.CreateInstance();
            Manager = Session.CreateSenseManager();
            ConfigurePoses();
            _speech = new Speech();
            _eyesThresholds = new Dictionary<Direction, double> {
                { Direction.Up, 10},
                { Direction.Down, 10},
                { Direction.Left, 10},
                { Direction.Right, 10}
            };
            Debug.WriteLine("SDK Version {0}.{1}", Session.QueryVersion().major, Session.QueryVersion().minor);
        }

        public void AddCapability(Capability capability) {
            if (_enabledCapabilities.Contains(capability)) {
                return;
            }
            var capImpl = _availableCapabilities[capability];
            foreach (var dependency in capImpl.Dependencies) {
                AddCapability(dependency);
            }
            _enabledCapabilities.Add(capability);
        }

        private void ConfigurePoses() {
            PosePeace.Configue(LeftHand, _poses);
            PosePeace.Configue(RightHand, _poses);
        }

        public override void Start() {
            _cancellationToken = new CancellationTokenSource();
            foreach (var capability in _enabledCapabilities) {
                _availableCapabilities[capability].Configure(this);
            }

            //_manager.EnableEmotion();
            //_manager.EnableFace();

            //using (var faceModule = _manager.QueryFace()) {
            //    using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) {
            //        moduleConfiguration.detection.maxTrackedFaces = 1;
            //        var expressionCofig = moduleConfiguration.QueryExpressions();
            //        expressionCofig.Enable();
            //        expressionCofig.EnableAllExpressions();

            //        var desc = new PXCMFaceConfiguration.RecognitionConfiguration.RecognitionStorageDesc();
            //        desc.maxUsers = 10;
            //        desc.isPersistent = true;
            //        var recognitionConfiguration = moduleConfiguration.QueryRecognition();
            //        recognitionConfiguration.CreateStorage(StorageName, out desc);
            //        recognitionConfiguration.UseStorage(StorageName);
            //        recognitionConfiguration.SetRegistrationMode(PXCMFaceConfiguration.RecognitionConfiguration.RecognitionRegistrationMode.REGISTRATION_MODE_CONTINUOUS);

            //        if (File.Exists(StorageFileName)) {
            //            var bytes = File.ReadAllBytes(StorageFileName);
            //            recognitionConfiguration.SetDatabaseBuffer(bytes);
            //        }
            //        recognitionConfiguration.Enable();
            //        moduleConfiguration.ApplyChanges();
            //    }
            //}
            //_manager.EnableHand();
            //using (var handModule = _manager.QueryHand()) {
            //    using (var handConfig = handModule.CreateActiveConfiguration()) {
            //        //handConfig.EnableAllAlerts();
            //        int numGestures = handConfig.QueryGesturesTotalNumber();
            //        for (int i = 0; i < numGestures; i++) {
            //            string name;
            //            handConfig.QueryGestureNameByIndex(i, out name);
            //            Debug.WriteLine("Gestures: " + name);
            //        }
            //        handConfig.EnableAllGestures();
            //        handConfig.SubscribeGesture(OnGesture);
            //        handConfig.EnableTrackedJoints(true);
            //        handConfig.ApplyChanges();
            //    }
            //}
            //EnableStreams();
            Debug.WriteLine("Initializing Camera...");

            var status = Manager.Init();
            if (status != NoError) { 
                throw new CameraException(status.ToString());
            }
            Task.Factory.StartNew(Loop,
                                  TaskCreationOptions.LongRunning,
                                  _cancellationToken.Token);
        }

        private void EnableStreams() {
            //var streamProfile = PXCMCapture.StreamTypeToIndex(PXCMCapture.StreamType.STREAM_TYPE_COLOR);
            //var info = new
            //    PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2
            //var desc = new PXCMSession.ImplDesc();
            //desc.group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR;
            //desc.subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE;

            //for (int i = 0; ; i++) {
            //    PXCMSession.ImplDesc implDesc;
            //    if (Session.QueryImpl(desc, i, out implDesc) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            //        break;
            //    PXCMCapture capture;
            //    if (session.CreateImpl<PXCMCapture>(implDesc, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            //        continue;
            //    for (int j = 0; ; j++) {
            //        PXCMCapture.DeviceInfo dinfo;
            //        if (capture.QueryDeviceInfo(j, out dinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR)
            //            break;

            //        //ToolStripMenuItem sm1 = new ToolStripMenuItem(dinfo.name, null, new EventHandler(Device_Item_Click));
            //        //devices[sm1] = dinfo;
            //        //devices_iuid[sm1] = implDesc.iuid;
            //        //DeviceMenu.DropDownItems.Add(sm1);
            //    }
            //    capture.Dispose();
            //}

            //_manager.captureManager.FilterByDeviceInfo(dinfo2);
            //_manager.captureManager.FilterByStreamProfiles(profiles);
            //_manager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR,
            //    ResolutionWidth, ResolutionHeight, FramesPerSecond);
        }

        private void Loop(object notUsed) {
            try {
                TryLoop();                
            }
            catch (Exception ex) {
                Debug.WriteLine("Loop error: " + ex);
                throw;
            }
        }

        private void TryLoop() {
            Debug.WriteLine("Loop started");

            while (!_cancellationToken.IsCancellationRequested) {
                Manager.AcquireFrame(false);
                foreach (var capability in _enabledCapabilities) {
                    _availableCapabilities[capability].Loop();
                }
                Manager.ReleaseFrame();
            }
            
            //var handModule = Manager.QueryHand();
            //var handData = handModule.CreateOutput();
            //var faceModule = Manager.QueryFace();
            //var faceData = faceModule.CreateOutput();

            //while (!_cancellationToken.IsCancellationRequested) {
            //    Manager.AcquireFrame(true);
            //    handData.Update();
            //    TrackHandAndFingers(LeftHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
            //    TrackHandAndFingers(RightHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
            //    faceData.Update();
            //    var face = FindFace(faceData);
            //    if (face != null) {
            //        TrackFace(face);
            //        RecognizeFace(faceData, face);
            //        TrackExpressions(face);
            //    }
            //    TrackEmotions();
            //    TrackImageFrame();

            //    Manager.ReleaseFrame();
            //    if (CyclePauseInMillis > 0) {
            //        Thread.Sleep(CyclePauseInMillis);
            //    }
            //}
            //handData.Dispose();
            //faceData.Dispose();
            //handModule.Dispose();
            //faceModule.Dispose();
        }

        private void TrackImageFrame() {
            var sample = Manager.QuerySample();
            if (sample == null) {
                return;
            }
            PXCMImage image = sample.color;
            PXCMImage.ImageData imageData;
            image.AcquireAccess(PXCMImage.Access.ACCESS_READ, 
                                PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, 
                                out imageData);
            Bitmap bitmap = imageData.ToBitmap(0, image.info.width, image.info.height);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ImageStream.CurrentBitmapImage = ms.ToArray();
            image.ReleaseAccess(imageData);
        }

        private PXCMFaceData.Face FindFace(PXCMFaceData faceData) {
            if (faceData.QueryNumberOfDetectedFaces() == 0) {
                Face.IsVisible = false;
                return null;
            }
            Face.IsVisible = true;
            return faceData.QueryFaces().First();
        }

        private void TrackEmotions() {
            if (!Face.IsVisible) {
                return;
            }
            var emotionInfo = Manager.QueryEmotion();
            if (emotionInfo == null) {
                return;
            }
            PXCMEmotion.EmotionData[] allEmotions;
            emotionInfo.QueryAllEmotionData(0, out allEmotions);
            emotionInfo.Dispose();
            if (allEmotions == null) {
                return;
            }
            var emotions =
                allEmotions.Where(e => (int)e.eid > 0 && (int)e.eid <= 64 && e.intensity > 0.4).ToList();
            if (emotions.Any()) {
                var emotion = emotions.OrderByDescending(e => e.evidence).First();
                Face.FacialExpression = (FacialExpression)emotion.eid;
            }
            else {
                Face.FacialExpression = FacialExpression.None;
            }
            emotionInfo.Dispose();
        }

        private void TrackExpressions(PXCMFaceData.Face face) {
            PXCMFaceData.ExpressionsData data = face.QueryExpressions();
            if (data == null) {
                return;
            }
            Face.Mouth.IsSmiling = CheckFaceExpression(data, FaceExpression.EXPRESSION_SMILE, SmileThreshold);
            Face.Mouth.IsOpen = CheckFaceExpression(data, FaceExpression.EXPRESSION_MOUTH_OPEN, MonthOpenThreshold);

            Face.LeftEye.IsOpen = !CheckFaceExpression(data, FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, EyesClosedThreshold);
            Face.RightEye.IsOpen = !CheckFaceExpression(data, FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, EyesClosedThreshold);
            
            Face.EyesDirection = GetEyesDirection(data);
        }

        private DateTime _lastEyesDirectionDetection;

        private Direction GetEyesDirection(PXCMFaceData.ExpressionsData data) {
            if ((DateTime.Now - _lastEyesDirectionDetection).TotalMilliseconds < 500) {
                return Face.EyesDirection;
            }
            _lastEyesDirectionDetection = DateTime.Now;
            var up = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_UP);
            var down = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_DOWN);
            var left = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_TURN_LEFT);
            var right = GetFaceExpressionIntensity(data, FaceExpression.EXPRESSION_EYES_TURN_RIGHT);

            //Debug.WriteLine("U:{0}/{4} D:{1}/{5} L:{2}/{6} R:{3}/{7}", up, down, left, right,
            //    _eyesThresholds[Direction.Up],
            //    _eyesThresholds[Direction.Down],
            //    _eyesThresholds[Direction.Left],
            //    _eyesThresholds[Direction.Right]
            //    );

            if (up > _eyesThresholds[Direction.Up]) {
                _eyesThresholds[Direction.Up] = up*0.7;
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
            //if (score.intensity > 0) Debug.WriteLine(faceExpression + ":" +score.intensity);
            return score.intensity;
        }


        private void TrackFace(PXCMFaceData.Face face) {
            PXCMRectI32 rect;
            face.QueryDetection().QueryBoundingRect(out rect);
            var point = new Point3D(rect.x + rect.w /2, rect.y + rect.h /2);
            Face.Position = CreatePosition(point, new Point3D());

            PXCMFaceData.LandmarksData landmarksData = face.QueryLandmarks();
            if (landmarksData == null) {
                return;
            }
            PXCMFaceData.LandmarkPoint[] facePoints;
            landmarksData.QueryPoints(out facePoints);
            if(facePoints == null) {
                return;
            }
            foreach (var item in facePoints) {
                switch(item.source.alias) {
                    case PXCMFaceData.LandmarkType.LANDMARK_UPPER_LIP_CENTER:
                        Face.Mouth.Position = CreatePosition(ToPoint3D(item.image), ToPoint3D(item.world));
                        break;
                    case PXCMFaceData.LandmarkType.LANDMARK_EYE_LEFT_CENTER:
                        Face.LeftEye.Position = CreatePosition(ToPoint3D(item.image), ToPoint3D(item.world));
                        break;
                    case PXCMFaceData.LandmarkType.LANDMARK_EYE_RIGHT_CENTER:
                        Face.RightEye.Position = CreatePosition(ToPoint3D(item.image), ToPoint3D(item.world));
                        break;
                }
            }
        }

        private Point3D ToPoint3D(PXCMPointF32 p) {
            return new Point3D(p.x, p.y);            
        }

        private Point3D ToPoint3D(PXCMPoint3DF32 p) {
            return new Point3D(p.x, p.y, p.z);
        }

        protected override IFaceRecognizer GetFaceRecognizer() {
            return this;
        }


        public void RecognizeFace() {
            _recognitionState = RecognitionState.Requested;
        }

        public override void Dispose() {
            _cancellationToken?.Cancel();
            Manager.SilentlyDispose();
            Session.SilentlyDispose();
            foreach (var capability in _enabledCapabilities) {
                _availableCapabilities[capability].SilentlyDispose();
            }
        }
    }
}
