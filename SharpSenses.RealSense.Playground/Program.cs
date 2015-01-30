using System;
using SharpSenses.Gestures;
using SharpSenses.Perceptual;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    internal class Program {

        private static ICamera _cam;

        private static void Main(string[] args) {
            _cam = Camera.Create();
            //_cam.Start();

            _cam.Speech.Say("Hello world!");
            _cam.Speech.Say("Hello world!");
            _cam.Speech.Say("Hello world!");

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