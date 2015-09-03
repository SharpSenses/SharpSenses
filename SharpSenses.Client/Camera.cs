using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.AspNet.SignalR.Client;

namespace SharpSenses.Client {
    public class Camera : BaseCamera {
        private readonly CoreDispatcher _dispatcher;
        public HubConnection HubConnection { get; private set; }
        private Camera(CoreDispatcher dispatcher) {
            _dispatcher = dispatcher;
        }

        public override int ResolutionWidth {
            get {
                return 640;
            }
        }

        public override int ResolutionHeight {
            get {
                return 480;
            }
        }

        public override int FramesPerSecond {
            get { return 60; }
        }

        public override ISpeech Speech {
            get {
                return new SpeechClient();
            }
        }

        public static ICamera Create(CoreDispatcher dispatcher = null) {
            return new Camera(dispatcher);
        }

        public override void Start() {
            Task.Run(async () => {
                while (true) {
                    
                    await Task.Delay(1000);
                    var x = LeftHand.Position.Image.X;
                    var p = new Position();
                    p.Image = new Point3D(++x, 0, 0);
                    LeftHand.Position = p;
                }
            });
        }

        public override void Dispose() {
            
        }

        protected override IFaceRecognizer GetFaceRecognizer() {
            return new FaceRecognizerClient();
        }
    }
}
