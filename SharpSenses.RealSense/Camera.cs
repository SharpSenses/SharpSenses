namespace SharpSenses.RealSense {
    public class Camera : ICamera {
        private PXCMSession _session;
        private PXCMSenseManager _manager;

        public Camera() {
            _manager = _session.CreateSenseManager();            
        }

        public void Start() {
            
        }
    }
}
