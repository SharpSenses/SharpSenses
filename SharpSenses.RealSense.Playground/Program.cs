using System;
using SharpSenses.Gestures;
using SharpSenses.Perceptual;
using SharpSenses.Poses;

namespace SharpSenses.RealSense.Playground {
    internal class Program {

        private static ICamera _cam;

        private static void Main(string[] args) {
            bool realSense = false;

            if (realSense) {
                _cam = new RealSenseCamera();                
            }
            else {
                _cam = new PerceptualCamera();
            }
            _cam.Start();

            _cam.LeftHand.Moved += p => {
                Console.Write("\r");
                Console.Write("X: " + p.Image.X);
            };

            _cam.Gestures.SwipeLeft += sl => { Console.WriteLine("Swipe Left"); };
            _cam.Gestures.SwipeRight += s => { Console.WriteLine("Swipe Right"); };
            _cam.Gestures.SwipeUp += s => { Console.WriteLine("Swipe Up"); };
            _cam.Gestures.SwipeDown += s => { Console.WriteLine("Swipe Down"); };
            _cam.Poses.PeaceBegin += hand => { Console.WriteLine("Peace, bro"); };

            //cam.Face.Visible += () => {
            //    Console.WriteLine("Face visible!");
            //};
            //cam.Face.NotVisible += () => {
            //    Console.WriteLine("Face not visible!");
            //};
            //cam.Face.Moved += p => {
            //    Console.WriteLine("FACE -> x: {0}|{1} y: {2}|{3}", 
            //        p.Image.X, 
            //        cam.LeftHand.Position.Image.X,  
            //        p.Image.Y,  
            //        cam.LeftHand.Position.Image.Y);
            //};

            //var pLeft = new Point3D();
            //var pRight = new Point3D();
            //var action = new Action<string, Point3D, Point3D>((s, p1, p2) => {
            //    var x = MathEx.CalcDistance(p1, p2);
            //    Console.WriteLine(s + "-> x1: {0} x2: {1} Dist-> {2}", p1.X, p2.X, x);
            //    if (x <= 30) {
            //        Console.WriteLine("BOOOOOMMMM!");
            //    }
            //});
            //cam.LeftHand.Index.Moved += p => {
            //    pLeft = p.Image;
            //    action.Invoke("L", pLeft, pRight);
            //};
            //cam.RightHand.Index.Moved += p => {
            //    pRight = p.Image;
            //    action.Invoke("R", pLeft, pRight);
            //};


            //var m = Movement.Right(cam.LeftHand, 10);
            ////m.Check = () => cam.LeftHand.GetAllFingers().Any(f => f.IsOpen);
            //m.Restarted += () => {
            //    Console.WriteLine("Restarted");
            //};
            //m.Progress += d => {
            //    Console.WriteLine("Progress: " + d);
            //};
            //m.Completed += () => {
            //    Console.WriteLine("Gesture---------------------");
            //};
            //m.Activate();

            //var s = new GestureStep(TimeSpan.FromDays(1), m);
            //s.StepProgress += d => {
            //    Console.WriteLine("Progress: " + d);
            //};
            //s.StepCompleted += () => {
            //    Console.WriteLine("Gesture---------------------");
            //};
            //s.Activate();

            //var swipe = new Gesture("hello");
            //swipe.AddStep(800, m);
            //swipe.StepProgress += d => {
            //    Console.WriteLine("Progress: " + d);
            //};
            //swipe.GestureDetected += () => {
            //    Console.WriteLine("Gesture");
            //};
            //swipe.Activate();

            //TrackCustomPoseWithBothHands(cam);
            //TrackMovement(cam);
            //TrackHandMovement(cam);
            //TrackVisibleAndOpen(cam);

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
            _cam.Dispose();
        }

        private static void TrackCustomPoseWithBothHands(RealSenseCamera cam) {
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

        private static void TrackVisibleAndOpen(RealSenseCamera cam) {
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

        private static void TrackHandMovement(RealSenseCamera cam) {
            int i = 0;
            cam.LeftHand.Moved += d => {
                i++;
                if (i%3 == 0) {
                    Console.WriteLine("Left: IX: {0} IY: {1} WX: {2}, WY:{3} WZ: {4} ",
                        d.Image.X, d.Image.Y, d.World.X, d.World.Y, d.World.Z);
                }
            };
            cam.RightHand.Moved +=
                d => Console.WriteLine("Right: X: {0} Y: {1} Z: {2}", d.Image.X, d.Image.Y, d.World.Z);
        }
    }
}