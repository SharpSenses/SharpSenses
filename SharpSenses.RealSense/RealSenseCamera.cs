using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.RealSense {
    public class RealSenseCamera : Camera, IFaceRecognizer {
        public static pxcmStatus NoError = pxcmStatus.PXCM_STATUS_NO_ERROR;
        private PXCMSenseManager _manager;
        private ISpeech _speech;
        private CancellationTokenSource _cancellationToken;
        private const string StorageName = "SharpSensesDb";
        private const string StorageFileName = "SharpSensesDb.bin";
        private RecognitionState _recognitionState = RecognitionState.Idle;
        public PXCMSession Session { get; private set; }

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

        public override ISpeech Speech {
            get { return _speech; }
        }

        public int CyclePauseInMillis { get; set; }

        public RealSenseCamera() {
            Session = PXCMSession.CreateInstance();
            _manager = Session.CreateSenseManager();
            ConfigurePoses();
            ConfigureGestures();
            _speech = new Speech(this);
            Debug.WriteLine("SDK Version {0}.{1}", Session.QueryVersion().major, Session.QueryVersion().minor);
        }

        private void ConfigureGestures() {
            GestureSlide.Configure(this, _gestures);
        }

        private void ConfigurePoses() {
            PosePeace.Configue(LeftHand, _poses);
            PosePeace.Configue(RightHand, _poses);
        }

        public override void Start() {
            _cancellationToken = new CancellationTokenSource();
            _manager.EnableEmotion();
            _manager.EnableFace();

            using (var faceModule = _manager.QueryFace()) {
                using (var moduleConfiguration = faceModule.CreateActiveConfiguration()) { 
                    moduleConfiguration.detection.maxTrackedFaces = 1;
                    
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
            
            _manager.EnableHand();
            using (var handModule = _manager.QueryHand()) {
                using (var handConfig = handModule.CreateActiveConfiguration()) {
                    //handConfig.EnableAllAlerts();
                    int numGestures = handConfig.QueryGesturesTotalNumber();
                    for (int i = 0; i < numGestures; i++) {
                        string name;
                        handConfig.QueryGestureNameByIndex(i, out name);
                        Debug.WriteLine("Gestures: " + name);
                    }
                    handConfig.EnableAllGestures();
                    handConfig.SubscribeGesture(OnGesture);
                    handConfig.EnableTrackedJoints(true);
                    handConfig.ApplyChanges();
                }
            }
            Debug.WriteLine("Initializing Camera...");

            var status = _manager.Init();
            if (status != NoError) {
                throw new CameraException(status.ToString());
            }
            Task.Factory.StartNew(Loop,
                                  TaskCreationOptions.LongRunning,
                                  _cancellationToken.Token);
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
            var handModule = _manager.QueryHand();
            var handData = handModule.CreateOutput();
            var faceModule = _manager.QueryFace();
            var faceData = faceModule.CreateOutput();

            while (!_cancellationToken.IsCancellationRequested) {
                _manager.AcquireFrame(true);
                handData.Update();
                TrackHandAndFingers(LeftHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_LEFT_HANDS);
                TrackHandAndFingers(RightHand, handData, PXCMHandData.AccessOrderType.ACCESS_ORDER_RIGHT_HANDS);
                faceData.Update();
                TrackFace(faceData);
                TrackEmotions();

                _manager.ReleaseFrame();
                if (CyclePauseInMillis > 0) {
                    Thread.Sleep(CyclePauseInMillis);
                }
            }
            handData.Dispose();
            faceData.Dispose();
            handModule.Dispose();
            faceModule.Dispose();
        }

        private void TrackEmotions() {
            if (!Face.IsVisible) {
                return;
            }
            var emotionInfo = _manager.QueryEmotion();
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

        private void TrackFace(PXCMFaceData faceData) {
            if (faceData.QueryNumberOfDetectedFaces() == 0) {
                Face.IsVisible = false;
                return;
            }
            Face.IsVisible = true;
            var face = faceData.QueryFaces().First();
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
                }
            }
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
            Face.UserId = userId;
            //Debug.WriteLine("UserId: " + userId);
        }

        private void SaveDatabase(PXCMFaceData faceData) {
            var rmd = faceData.QueryRecognitionModule();
            var buffer = new Byte[rmd.QueryDatabaseSize()];
            rmd.QueryDatabaseBuffer(buffer);
            File.WriteAllBytes(StorageFileName, buffer);
        }

        private string _last = "";
    
        private void OnGesture(PXCMHandData.GestureData gesturedata) {
            string g = String.Format("Gesture: {0}-{1}-{2}",
                gesturedata.name,
                gesturedata.handId,
                gesturedata.state);
            if (_last == g) {
                return;
            }
            _last = g;
            //Debug.WriteLine(g);
            switch (gesturedata.name) {
                case "wave":
                    _gestures.OnWave(new GestureEventArgs("wave"));
                    return;
            }
        }

        private void TrackHandAndFingers(Hand hand, PXCMHandData data, PXCMHandData.AccessOrderType label) {
            PXCMHandData.IHand handInfo;
            if (data.QueryHandData(label, 0, out handInfo) != NoError) {
                hand.IsVisible = false;
                return;
            }
            hand.IsVisible = true;
            SetHandOpenness(hand, handInfo);
            SetHandPosition(hand, handInfo);
            TrackIndex(hand.Index, handInfo);
            TrackMiddle(hand.Middle, handInfo);
            TrackRing(hand.Ring, handInfo);
            TrackPinky(hand.Pinky, handInfo);
            TrackThumb(hand.Thumb, handInfo);
        }

        private void SetHandPosition(Hand hand, PXCMHandData.IHand handInfo) {
            var imagePosition = ToPoint3D(handInfo.QueryMassCenterImage());
            var worldPosition = ToPoint3D(handInfo.QueryMassCenterWorld());
            hand.Position = CreatePosition(imagePosition, worldPosition);
        }

        private void SetHandOpenness(Hand hand, PXCMHandData.IHand handInfo) {
            int openness = handInfo.QueryOpenness();
            SetOpenness(hand, openness);
        }

        private void TrackIndex(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_INDEX_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_INDEX, handInfo);
        }

        private void TrackMiddle(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_MIDDLE_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_MIDDLE, handInfo);
        }

        private void TrackRing(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_RING_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_RING, handInfo);
        }

        private void TrackPinky(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_PINKY_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_PINKY, handInfo);
        }

        private void TrackThumb(Finger finger, PXCMHandData.IHand handInfo) {
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_BASE, finger.BaseJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_JT1, finger.FirstJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_JT2, finger.SecondJoint);
            SetJointdata(handInfo, PXCMHandData.JointType.JOINT_THUMB_TIP, finger);
            SetFingerOpenness(finger, PXCMHandData.FingerType.FINGER_THUMB, handInfo);
        }

        private void SetFingerOpenness(Finger finger, PXCMHandData.FingerType fingerType, PXCMHandData.IHand handInfo) {
            PXCMHandData.FingerData fingerData;
            if (handInfo.QueryFingerData(fingerType, out fingerData) != NoError) {
                return;
            }
            SetOpenness(finger, fingerData.foldedness);            
        }

        private void SetJointdata(PXCMHandData.IHand handInfo, PXCMHandData.JointType jointType, Item joint) {
            PXCMHandData.JointData jointData;
            if (handInfo.QueryTrackedJoint(jointType, out jointData) != NoError) {
                joint.IsVisible = false;
                return;
            }
            SetVisibleJointPosition(joint, jointData);
        }

        private void SetOpenness(FlexiblePart part, int scaleZeroToHundred) {
            if (scaleZeroToHundred > 75) {
                part.IsOpen = true;
            }
            else if (scaleZeroToHundred < 35) {
                part.IsOpen = false;
            }
        }

        private void SetVisibleJointPosition(Item item, PXCMHandData.JointData jointData) {
            var imagePosition = ToPoint3D(jointData.positionImage);
            var worldPosition = ToPoint3D(jointData.positionWorld);
            item.IsVisible = true;
            item.Position = CreatePosition(imagePosition, worldPosition);
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
            if (_cancellationToken != null) {
                _cancellationToken.Cancel();                
            }
            _manager.SilentlyDispose();
            Session.SilentlyDispose();
        }
    }
}
