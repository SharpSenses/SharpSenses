using System;
using System.Diagnostics;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    internal class Program {

        private static ICamera _cam;

        private static void Main(string[] args) {
            Item.DefaultNoiseThreshold = 0;
            
            _cam = Camera.Create();
            _cam.LeftHand.RotationChanged += (sender, eventArgs) => {
                Console.Write("Roll: {0:0} Yaw: {1:0} Pitch {2:0}                  ", 
                    _cam.LeftHand.Rotation.Roll,
                    _cam.LeftHand.Rotation.Yaw,
                    _cam.LeftHand.Rotation.Pitch);
                Console.Write('\r');
            };

            _cam.Face.LeftEye.Blink += (sender, eventArgs) => {
                Console.WriteLine("Blink");
            };
            _cam.Face.LeftEye.DoubleBlink += (sender, eventArgs) => {
                Console.WriteLine("Double Blink");
            };
            _cam.Face.WinkedLeft += (sender, eventArgs) => {
                Console.WriteLine("WinkedLeft");
            };
            _cam.Face.WinkedRight += (sender, eventArgs) => {
                Console.WriteLine("WinkedRight");
            };

            //_cam.Speech.SpeechRecognized += (sender, eventArgs) => {
            //    Console.WriteLine("-> " + eventArgs.Sentence.ToLower());
            //};
            //_cam.Speech.EnableRecognition(SupportedLanguage.PtBR);

            _cam.Face.LeftEye.Closed += (s, e) => {
                Console.WriteLine("-> Olho esquerdo fechado ");
            };

            _cam.RightHand.Visible += (sender, eventArgs) => {
                Console.WriteLine("-> Visible ");
            };
            //_cam.Start();


            //_cam.RightHand.Visible += (sender, eventArgs) => {
            //    Console.WriteLine("-> Visible ");
            //};

            //_cam.RightHand.NotVisible += (sender, eventArgs) => {
            //    Console.WriteLine("-> NotVisible ");
            //};

            _cam.RightHand.Opened += (sender, eventArgs) => {
                Console.WriteLine("-> Open");
            };

            _cam.RightHand.Closed += (sender, eventArgs) => {
                Console.WriteLine("-> Closed");
            };

            //_cam.RightHand.Moved += (sender, eventArgs) => {
            //    Console.Write((char)13);
            //    Console.Write("-> P: " + eventArgs.NewPosition.Image);
            //};

            //_cam.Gestures.SlideLeft += (sender, eventArgs) => {
            //    Console.WriteLine("");
            //    Console.WriteLine("<--------------------");
            //};

            //_cam.Gestures.SlideRight += (sender, eventArgs) => {
            //    Console.WriteLine("");
            //    Console.WriteLine("-------------------->");
            //};

            //Process.Start("WINWORD.EXE");
            //Process.Start("EXCEL.EXE");
            //Process.Start("POWERPNT.EXE");
            //Process.Start("POWERPNT.EXE");
            //Process.Start("http://google.com/search?q=" + t);

            //int yawned = 0;
            //_cam.Face.Yawned += (sender, eventArgs) => {
            //    Console.WriteLine("-> YAWNED " + yawned++);
            //};

            //_cam.Face.Visible += (sender, eventArgs) => {
            //    Console.WriteLine("-> Face visible " + _cam.Face.UserId);
            //};

            //_cam.Face.NotVisible += (sender, eventArgs) => {
            //    Console.WriteLine("-> Face not visible " + _cam.Face.UserId);
            //};

            //_cam.Face.FaceRecognized += (sender, eventArgs) => {
            //    Console.WriteLine("User: " + eventArgs.UserId);
            //};

            //while (true) {
            //    Console.ReadLine();
            //    _cam.Face.RecognizeFace();
            //    Console.WriteLine("Recognize!");
            //}


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


            Action moved = () => {
                Console.Write("\r");
                Console.Write("LeftXY: {0}|{1} | Right XY: {2}|{3}",
                    _cam.LeftHand.Index.Position.Image.X,
                    _cam.LeftHand.Index.Position.Image.Y,
                    _cam.RightHand.Index.Position.Image.X,
                    _cam.RightHand.Index.Position.Image.Y
                    );
            };

            //_cam.LeftHand.Index.Moved += (s, a) => moved();
            //_cam.RightHand.Index.Moved += (s, a) => moved();

            //_cam.Gestures.SlideLeft += (s, a) => Console.WriteLine("Swipe Left");
            //_cam.Gestures.SlideRight += (s, a) => Console.WriteLine("Swipe Right");
            //_cam.Gestures.SlideUp += (s, a) => Console.WriteLine("Swipe Up");
            //_cam.Gestures.SlideDown += (s, a) => Console.WriteLine("Swipe Down");
            //_cam.Gestures.MoveForward += (s, a) => Console.WriteLine("Move Forward");

            //var pose = PoseBuilder.Create().ShouldBeNear(_cam.LeftHand, _cam.RightHand,100).Build();
            //pose.Begin += (s, a) => {
            //    Console.WriteLine("Super pose!");
            //};
            _cam.Start();

            Console.ReadLine();
            _cam.Dispose();
        }

        private static void TrackMovement(Movement m) {
            m.Completed += () => Console.WriteLine(m.Name + " -> DONE!!!!");
            m.Restarted += () => Console.WriteLine(m.Name + " -> Restarted");
            m.Progress += d => {
                Console.Write(m.Name + " -> ");
                for (int i = 0; i < d; i++) {
                    Console.Write("-");
                }
                Console.WriteLine(">");
            };
        }


        private static void TrackHandMovement(ICamera cam) {
            int i = 0;
            cam.LeftHand.Moved += (s,a) => {
                var d = a.NewPosition;
                i++;
                if (i%3 == 0) {
                    Console.WriteLine("Left: IX: {0} IY: {1} WX: {2}, WY:{3} WZ: {4} ",
                        d.Image.X, d.Image.Y, d.World.X, d.World.Y, d.World.Z);
                }
            };
            cam.RightHand.Moved += (s, a) => {
                var d = a.NewPosition;
                Console.WriteLine("Right: X: {0} Y: {1} Z: {2}", d.Image.X, d.Image.Y, d.World.Z);
            };
        }
    }
}