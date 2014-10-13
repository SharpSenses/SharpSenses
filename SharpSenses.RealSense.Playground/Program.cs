using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    class Program {
        static void Main(string[] args) {
            var cam = new Camera();
            cam.Start();

            //TrackHandMovement(cam);
            //TrackVisibleAndOpen(cam);
            var step = new GestureStep(TimeSpan.FromMilliseconds(500), Movement.Forward(cam.LeftHand, 10));
            
            var gesture = new Gesture();
            gesture.AddStep(step);
            gesture.GestureDetected += () => {
                Console.WriteLine("Punch!");
            };
            //Movement m = new MovementBackward(10);
            //gesture.AddMovement();

            //cam.PoseSensor.PeaceBegin += n => Console.WriteLine("Peace Begin: " + n.Side);
            //cam.PoseSensor.PeaceEnd += n => Console.WriteLine("Peace End" + n.Side);

            Console.ReadLine();
            cam.Dispose();
        }

        private static void TrackVisibleAndOpen(Camera cam) {
            cam.LeftHand.Opened += () => Console.WriteLine("Left Open");
            cam.LeftHand.Closed += () => Console.WriteLine("Left Closed");
            cam.LeftHand.Visible += () => Console.WriteLine("Left Visible");
            cam.LeftHand.NotVisible += () => Console.WriteLine("Left Not Visible");
            cam.LeftHand.Index.Opened += () => Console.WriteLine("Left Index Open");
            cam.LeftHand.Index.Closed += () => Console.WriteLine("Left Index Closed");

            cam.RightHand.Opened += () => Console.WriteLine("Right Open");
            cam.RightHand.Closed += () => Console.WriteLine("Right Closed");
            cam.RightHand.Visible += () => Console.WriteLine("Right Visible");
            cam.RightHand.NotVisible += () => Console.WriteLine("Right Not Visible");
        }

        private static void TrackHandMovement(Camera cam) {
            cam.LeftHand.Moved += d => Console.WriteLine("Left: X: {0} Y: {1} Z: {2}", d.X, d.Y, d.Z);
            cam.RightHand.Moved += d => Console.WriteLine("Right: X: {0} Y: {1} Z: {2}", d.X, d.Y, d.Z);
        }
    }
}
