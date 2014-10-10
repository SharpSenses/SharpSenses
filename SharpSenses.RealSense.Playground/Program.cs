using System;

namespace SharpSenses.RealSense.Playground {
    class Program {
        static void Main(string[] args) {
            var cam = new Camera();
            cam.Start();

            cam.LeftHand.Moved += d => {
                Console.WriteLine("Left: X: {0} Y: {1} Z: {2}",
                    d.X, d.Y, d.Z);
            };
            cam.RightHand.Moved += d => {
                Console.WriteLine("Right: X: {0} Y: {1} Z: {2}",
                    d.X, d.Y, d.Z);
            };

            cam.Dispose();
            Console.ReadLine();
        }
    }
}
