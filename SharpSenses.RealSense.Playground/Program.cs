using System;
using System.Diagnostics;
using SharpSenses.Gestures;
using SharpSenses.Poses;
using SharpSenses.RealSense.Capabilities;
using static System.Console;

namespace SharpSenses.RealSense.Playground {
    internal class Program {

        private static ICamera _cam;

        private static void Main(string[] args) {
            Item.DefaultNoiseThreshold = 0;
            
            _cam = Camera.Create(Capability.FaceTracking);

            //TestHands();
            //TestHandsRotations();
            //TestHandsMovements();
            //TestFace();
            //TestFaceRecognition();
            //TestEyes();
            //TestSpeech();
            //TestGestures();
            _cam.Start();
            
            //int yawned = 0;
            //_cam.Face.Yawned += (sender, eventArgs) => {
            //    Console.WriteLine("-> YAWNED " + yawned++);
            //};

            

            


            //_cam.Speech.CurrentLanguage = SupportedLanguage.EnUS;
            //_cam.Speech.Say("Hello world!");
            //_cam.Speech.Say("Hello world!");
            //_cam.Speech.Say("Hello world!");

            //_cam.Speech.SpeechRecognized += (sender, eventArgs) => {
            //    Console.WriteLine("-> " + eventArgs.Sentence.ToLower());
            //};

            //_cam.Face.EyesDirectionChanged += (s, a) => {
            //    if (a.NewDirection == Direction.None) return;
            //    Console.WriteLine("-> " + a.NewDirection);
            //};

            //_cam.Face.LeftEye.Opened += (s, a) => {
            //    Console.WriteLine("-> LeftEye opened");
            //};
            //_cam.Face.LeftEye.Closed += (s, a) => {
            //    Console.WriteLine("-> LeftEye Closed");
            //};

            //_cam.Face.LeftEye.Blink += (s, a) => {
            //    Console.WriteLine("-> LeftEye Blink");
            //};

            //_cam.Face.LeftEye.DoubleBlink += (s, a) => {
            //    Console.WriteLine("-> LeftEye DoubleBlink");
            //};

            //_cam.Face.Mouth.Opened += (s, a) => {
            //    Console.WriteLine("-> month opened");
            //};

            //_cam.Face.Mouth.Closed += (s, a) => {
            //    Console.WriteLine("-> month closed");
            //};

            //_cam.Face.Mouth.Smiled += (s, a) => {
            //    Console.WriteLine("-> month smiled");
            //};

            //_cam.Speech.EnableRecognition();
            //Console.WriteLine("Speech");
            //Console.ReadLine();
            //_cam.Start();
            //Console.WriteLine("Cam Started");
            //Console.ReadLine();



            //_cam.LeftHand.Visible += (s,a) => Console.WriteLine("Hi  l");
            //_cam.LeftHand.NotVisible += (s,a) => Console.WriteLine("Bye l");
            //_cam.RightHand.Visible += (s,a) => Console.WriteLine("Hi r");

            //_cam.Gestures.SlideRight += (s, a) => Console.WriteLine(a.GestureName);
            //_cam.Gestures.SlideLeft += (s, a) => Console.WriteLine(a.GestureName);
            //_cam.Gestures.SlideUp += (s, a) => Console.WriteLine(a.GestureName);
            //_cam.Gestures.SlideDown += (s, a) => Console.WriteLine(a.GestureName);

            //int xmoved = 0;
            //_cam.LeftHand.Moved += (s, a) => {
            //    Console.WriteLine(++xmoved);
            //};

            //_cam.RightHand.Moved += (s,a) => Console.WriteLine("-> " + a.NewPosition.Image.X);

            //_cam.Face.RecognizeFace();
            //_cam.Face.FaceRecognized += (s, a) => {
            //    Console.WriteLine("Hello " + a.UserId); 
            //};

            //_cam.LeftHand.FingerOpened += (sender, eventArgs) => {
            //    var finger = (Finger) sender;
            //    Console.WriteLine("Finger Open: " + finger.Kind);
            //};

            //_cam.Face.FacialExpresssionChanged += (s, e) => Console.WriteLine("FacialExpression: " + e.NewFacialExpression);


            //_cam.LeftHand.Index.Moved += (s, a) => moved();
            //_cam.RightHand.Index.Moved += (s, a) => moved();

            

            //var pose = PoseBuilder.Create().ShouldBeNear(_cam.LeftHand, _cam.RightHand,100).Build();
            //pose.Begin += (s, a) => {
            //    Console.WriteLine("Super pose!");
            //};
            

            ReadLine();
            _cam.Dispose();
        }

        private static void TestFaceRecognition() {
            _cam.Face.FaceRecognized += (sender, eventArgs) => {
                WriteLine("User: " + eventArgs.UserId);
            };

            while (true) {
                ReadLine();
                _cam.Face.RecognizeFace();
                WriteLine("Recognize!");
            }
        }

        private static void TestFace() {
            var face = default(Point3D);
            var visible = false;
            var id = 0;
            Action update = () => {
                Clear();
                WriteLine($"-> Face: {face} Id: {id}");
                WriteLine($"-> Visible: {visible}");
            };

            _cam.Face.Visible += (sender, eventArgs) => {
                visible = true;
                id = _cam.Face.UserId;
                update.Invoke();
            };

            _cam.Face.NotVisible += (sender, eventArgs) => {
                visible = false;
                update.Invoke();
            };
            _cam.Face.Moved += (sender, args) => {
                face = args.NewPosition.World;
                update.Invoke();
            };
        }

        private static void TestGestures() {
            _cam.Gestures.SlideLeft += (s, a) => Console.WriteLine("Swipe Left");
            _cam.Gestures.SlideRight += (s, a) => Console.WriteLine("Swipe Right");
            _cam.Gestures.SlideUp += (s, a) => Console.WriteLine("Swipe Up");
            _cam.Gestures.SlideDown += (s, a) => Console.WriteLine("Swipe Down");
            _cam.Gestures.MoveForward += (s, a) => Console.WriteLine("Move Forward");
        }

        private static void TestSpeech() {
            _cam.Speech.SpeechRecognized += (sender, eventArgs) => {
                WriteLine("-> Speech Recognized: " + eventArgs.Sentence.ToLower());
            };
            _cam.Speech.EnableRecognition(SupportedLanguage.PtBR);
        }

        private static void TestEyes() {
            _cam.Face.LeftEye.Blink += (sender, eventArgs) => { WriteLine("Blink"); };
            _cam.Face.LeftEye.DoubleBlink += (sender, eventArgs) => { WriteLine("Double Blink"); };
            _cam.Face.WinkedLeft += (sender, eventArgs) => { WriteLine("WinkedLeft"); };
            _cam.Face.WinkedRight += (sender, eventArgs) => { WriteLine("WinkedRight"); };

            _cam.Face.LeftEye.Closed += (s, e) => {
                WriteLine("-> Olho esquerdo fechado ");
            };
        }

        private static void TestHands() {
            _cam.RightHand.Visible += (s, a) => { WriteLine("-> Right Hand: Visible "); };
            _cam.RightHand.NotVisible += (s, a) => { WriteLine("-> Right Hand: NotVisible "); };
            _cam.RightHand.Opened += (s, a) => { WriteLine("-> Right Hand: Open"); };
            _cam.RightHand.Closed += (s, a) => { WriteLine("-> Right Hand: Closed");};

            _cam.LeftHand.Visible += (s, a) => { WriteLine("-> Left Hand: Visible "); };
            _cam.LeftHand.NotVisible += (s, a) => { WriteLine("-> Left Hand: NotVisible "); };
            _cam.LeftHand.Opened += (s, a) => { WriteLine("-> Left Hand: Open"); };
            _cam.LeftHand.Closed += (s, a) => { WriteLine("-> Left Hand: Closed"); };
        }

        private static void TestHandsMovements() {
            var right = default(Point3D);
            var left = default(Point3D);
            Action update = () => {
                Clear();
                WriteLine($"-> Hands: Right: {right}");
                WriteLine($"-> Hands: Left: {left}");
            };
            _cam.RightHand.Moved += (s, a) => {
                right = a.NewPosition.World;
                update.Invoke();
            };
            _cam.LeftHand.Moved += (s, a) => {
                left = a.NewPosition.World;
                update.Invoke();
            };
        }

        private static void TestHandsRotations() {
            _cam.LeftHand.RotationChanged += (sender, eventArgs) => {
                WriteLine("-> Left Hand: Roll: {0:0} Yaw: {1:0} Pitch {2:0}",
                    _cam.LeftHand.Rotation.Roll,
                    _cam.LeftHand.Rotation.Yaw,
                    _cam.LeftHand.Rotation.Pitch);
                Write((char)13);
            };
        }

        private static void TrackMovement(Movement m) {
            m.Completed += () => WriteLine(m.Name + " -> DONE!!!!");
            m.Restarted += () => WriteLine(m.Name + " -> Restarted");
            m.Progress += d => {
                Write(m.Name + " -> ");
                for (int i = 0; i < d; i++) {
                    Write("-");
                }
                WriteLine(">");
            };
        }


        private static void TrackHandMovement(ICamera cam) {
            int i = 0;
            cam.LeftHand.Moved += (s,a) => {
                var d = a.NewPosition;
                i++;
                if (i%3 == 0) {
                    WriteLine("Left: IX: {0} IY: {1} WX: {2}, WY:{3} WZ: {4} ",
                        d.Image.X, d.Image.Y, d.World.X, d.World.Y, d.World.Z);
                }
            };
            cam.RightHand.Moved += (s, a) => {
                var d = a.NewPosition;
                WriteLine("Right: X: {0} Y: {1} Z: {2}", d.Image.X, d.Image.Y, d.World.Z);
            };
        }
    }
}