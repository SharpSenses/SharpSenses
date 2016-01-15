using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SharpSenses.Poses;
using SharpSenses.RealSense.Capabilities;
using SharpSenses.RealSense.Speech;
using static SharpSenses.RealSense.Errors;

namespace SharpSenses.RealSense {
    public class RealSenseCamera : Camera {
        private static Dictionary<Capability, ICapability> _availableCapabilities =
            new Dictionary<Capability, ICapability> {
                [Capability.HandTracking] = new HandTrackingCapability(),
                [Capability.FingersTracking] = new FingerTrackingCapability(),
                [Capability.GestureTracking] = new GesturesCapability(),
                [Capability.FaceTracking] = new FaceCapability(),
                [Capability.EmotionTracking] = new EmotionCapability(),
                [Capability.FacialExpressionTracking] = new FacialExpressionCapability(),
                [Capability.FaceRecognition] = new FaceRecognitionCapability(),
                [Capability.ImageStreamTracking] = new ImageStreamCapability()
            };

        private CancellationTokenSource _cancellationToken;

        private List<Capability> _enabledCapabilities = new List<Capability>();

        public RealSenseCamera() {
            Session = PXCMSession.CreateInstance();
            Manager = Session.CreateSenseManager();
            ConfigurePoses();
            Speech = new SpeechManager();
            Debug.WriteLine("SDK Version {0}.{1}", Session.QueryVersion().major, Session.QueryVersion().minor);
        }

        public PXCMSenseManager Manager { get; }
        public PXCMSession Session { get; }

        public override int ResolutionWidth => 640;
        public override int ResolutionHeight => 480;
        public override int FramesPerSecond => 30;
        public override ISpeech Speech { get; }
        public int CyclePauseInMillis { get; set; }

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
            Debug.WriteLine("Initializing Camera...");

            var status = Manager.Init();
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
            catch (Exception ex) when (LogException(ex)) {
                throw;
            }
        }

        private bool LogException(Exception ex) {
            Debug.WriteLine("Loop error: " + ex);
            return true;
        }

        private void TryLoop() {
            Debug.WriteLine("Loop started");

            var loopObjects = new LoopObjects();

            while (!_cancellationToken.IsCancellationRequested) {
                Manager.AcquireFrame(true);
                foreach (var capability in _enabledCapabilities) {
                    _availableCapabilities[capability].Loop(loopObjects);
                }
                Manager.ReleaseFrame();
                if (CyclePauseInMillis > 0) {
                    Thread.Sleep(CyclePauseInMillis);
                }
            }
        }

        protected override IFaceRecognizer GetFaceRecognizer() {
            return (IFaceRecognizer)_availableCapabilities[Capability.FaceRecognition];
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

    public class CapabilityException : Exception {
        public CapabilityException(string message) : base(message) {
        }
    }
}