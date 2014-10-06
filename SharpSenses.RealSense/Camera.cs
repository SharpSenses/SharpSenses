using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSenses.RealSense {
    public class Camera : ICamera {
        private PXCMSession _session;
        private PXCMSenseManager _manager;
        private CancellationTokenSource _cancellationToken;

        public Camera() {
            _manager = PXCMSenseManager.CreateInstance();            
        }

        public void Start() {
            _cancellationToken = new CancellationTokenSource();
            Task.Factory.StartNew(Loop, 
                                  TaskCreationOptions.LongRunning,
                                  _cancellationToken.Token);
        }

        private void Loop(object notUsed) {
            _manager.Init();
            _manager.EnableHand();
            while (!_cancellationToken.IsCancellationRequested) {
                _manager.AcquireFrame(true);
                var handAnalysis = _manager.QueryHand();
                //var handConfig = handAnalysis.CreateActiveConfiguration();
                //handConfig.EnableAllAlerts();
                //handConfig.ApplyChanges();
                //handConfig.Dispose();

                var handData = handAnalysis.CreateOutput();
                var numberOfHands = handData.QueryNumberOfHands();
                Debug.WriteLine("Hands: " +numberOfHands);

                _manager.ReleaseFrame();
            }
        }

        public void Dispose() {
            _manager.Dispose();
        }
    }
}
