using System;
using System.ComponentModel;
using System.Diagnostics;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    class Program {
        static void Main(string[] args) {
            var cam = new Camera();
            cam.Start();

            cam.Gestures.SwipeLeft += () => {
                Console.WriteLine("Swipe Left");
            };
            cam.Gestures.SwipeRight += () => {
                Console.WriteLine("Swipe Right");
            };
            cam.Gestures.SwipeUp += () => {
                Console.WriteLine("Swipe Up");
            };
            cam.Gestures.SwipeDown += () => {
                Console.WriteLine("Swipe Down");
            };
            cam.Poses.PeaceBegin += hand => {
                Console.WriteLine("Peace, bro");
            };
            //TrackCustomPoseWithBothHands(cam);
            //TrackMovement(cam);
            //TrackHandMovement(cam);
            TrackVisibleAndOpen(cam);

            //var s1Left = Movement.Forward(cam.LeftHand, 15);
            //var s1Right = Movement.Forward(cam.RightHand, 15);
            //var s2Left = Movement.Left(cam.LeftHand, 15);
            //var s2Right = Movement.Right(cam.RightHand, 15);

            //var g = new Gesture();
            //g.AddStep(800, s1Left, s1Right);
            //g.AddStep(800, s2Left, s2Right);
            //g.NextStep += i => {
            //    Console.WriteLine("STEP -> " + i);
            //};
            //g.GestureDetected += () => {
            //    Console.WriteLine("WOWWWWWWWWWWWWWWWWWWWWWWWWW!!!");
            //};
            //g.Activate();

            //TrackMovement(lh);
            //TrackMovement(rh);

            //lh.Activate();
            //var step = new GestureStep(TimeSpan.FromHours(800), lh, rh);
            //step.StepCompleted += () => Console.WriteLine("HADOKEN HADOKEN HADOKEN HADOKEN ");
            //step.Activate();
            
            //var gesture = new Gesture();
            //gesture.AddStep(step);
            //gesture.GestureDetected += () => {
            //    Console.WriteLine("Punch!");
            //};
            //Movement m = new MovementBackward(10);
            //gesture.AddMovement();

            //cam.Poses.PeaceBegin += n => Console.WriteLine("Peace Begin: " + n.Side);
            //cam.Poses.PeaceEnd += n => Console.WriteLine("Peace End" + n.Side);

            Console.ReadLine();
            cam.Dispose();
        }

        private static void TrackCustomPoseWithBothHands(Camera cam) {
            var bothHandsClosed = PoseBuilder.Combine(cam.LeftHand, State.Closed)
                .With(cam.RightHand, State.Closed)
                .With(cam.LeftHand, State.Visible)
                .With(cam.RightHand, State.Visible)
                .Build("bothhandsclosed");
            bothHandsClosed.Begin += s => Console.WriteLine("BOTH Begin");
            bothHandsClosed.End += s => Console.WriteLine("BOTH End");
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

        private static void TrackVisibleAndOpen(Camera cam) {
            cam.LeftHand.Opened += () => Console.WriteLine("Left Open");
            cam.LeftHand.Closed += () => Console.WriteLine("Left Closed");
            cam.LeftHand.Visible += () => Console.WriteLine("Left Visible");
            cam.LeftHand.NotVisible += () => Console.WriteLine("Left Not Visible");
            //cam.LeftHand.Index.Opened += () => Console.WriteLine("Left Index Open");
            //cam.LeftHand.Index.Closed += () => Console.WriteLine("Left Index Closed");

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
