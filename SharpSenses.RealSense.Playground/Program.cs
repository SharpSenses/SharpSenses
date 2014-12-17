using System;
using SharpSenses.Gestures;
using SharpSenses.Perceptual;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    internal class Program {

        private static ICamera _cam;

        private static void Main(string[] args) {
            _cam = Camera.Create();
            _cam.Start();
            _cam.LeftHand.Visible += (s,a) => Console.WriteLine("Hi");

            _cam.LeftHand.FingerOpened += (sender, eventArgs) => {
                var finger = (Finger) sender;
                Console.WriteLine("Finger Open: " + finger.Kind);
            };

            _cam.LeftHand.Index.Moved += (s, a) => {
                Console.Write("\r");
                Console.Write("FingerX: {0} | Mouth : {1}", a.NewPosition.Image.X,  _cam.Face.Mouth.Position.Image.X);
            };

            _cam.Gestures.SlideLeft += (s, a) => Console.WriteLine("Swipe Left");
            _cam.Gestures.SlideRight += (s, a) => Console.WriteLine("Swipe Right");
            _cam.Gestures.SlideUp += (s, a) => Console.WriteLine("Swipe Up");
            _cam.Gestures.SlideDown += (s, a) => Console.WriteLine("Swipe Down");
            _cam.Gestures.MoveForward += (s, a) => Console.WriteLine("Move Forward");

            var pose = PoseBuilder.Create().ShouldTouch(_cam.Face.Mouth, _cam.LeftHand.Index).Build();
            pose.Begin += (s, a) => {
                Console.WriteLine("Super pose!");
            };

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