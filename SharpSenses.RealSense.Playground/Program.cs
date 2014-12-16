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

            _cam.LeftHand.Moved += (s, a) => {
                Console.Write("\r");
                Console.Write("X: " + a.NewPosition.Image.X);
            };

            _cam.Gestures.SwipeLeft += s => Console.WriteLine("Swipe Left");
            _cam.Gestures.SwipeRight += s => Console.WriteLine("Swipe Right");
            _cam.Gestures.SwipeUp += s => Console.WriteLine("Swipe Up");
            _cam.Gestures.SwipeDown += s => Console.WriteLine("Swipe Down");

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