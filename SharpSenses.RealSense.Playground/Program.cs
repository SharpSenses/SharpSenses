using System;

namespace SharpSenses.RealSense.Playground {
    class Program {
        static void Main(string[] args) {
            var cam = new Camera();
            cam.Start();

            //TrackHandMovement(cam);

            cam.LeftHand.Opened += () => Console.WriteLine("Left Open");
            cam.LeftHand.Closed += () => Console.WriteLine("Left Closed");
            cam.LeftHand.Visible += () => Console.WriteLine("Left Visible");
            cam.LeftHand.NotVisible += () => Console.WriteLine("Left Not Visible");

            cam.RightHand.Opened += () => Console.WriteLine("Right Open");
            cam.RightHand.Closed += () => Console.WriteLine("Right Closed");
            cam.RightHand.Visible += () => Console.WriteLine("Right Visible");
            cam.RightHand.NotVisible += () => Console.WriteLine("Right Not Visible");

            Console.ReadLine();
            cam.Dispose();
        }

        private static void TrackHandMovement(Camera cam) {
            cam.LeftHand.Moved += d => {
                Console.WriteLine("Left: X: {0} Y: {1} Z: {2}",
                    d.X, d.Y, d.Z);
            };
            cam.RightHand.Moved += d => {
                Console.WriteLine("Right: X: {0} Y: {1} Z: {2}",
                    d.X, d.Y, d.Z);
            };
        }
    }
}
