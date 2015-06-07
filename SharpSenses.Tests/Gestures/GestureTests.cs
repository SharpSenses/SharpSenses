using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpSenses.Gestures;

namespace SharpSenses.Tests {
    public class GestureTests {
        private FakeCamera _cam;

        [SetUp]
        public void SetUp() {
            _cam = new FakeCamera();
        }

        [Test]
        public void Should_go_to_next_step() {
            var step = 0;
            var b = new Gesture();
            b.AddStep(50000, Movement.Forward(_cam.LeftHand, 10), Movement.Forward(_cam.RightHand, 10));
            b.AddStep(50000, Movement.Left(_cam.LeftHand, 10), Movement.Right(_cam.RightHand, 10));
            b.NextStep += (s,a) => {
                step = a.Step;
            };
            b.Activate();

            _cam.MoveLeftHandZ(30);
            _cam.MoveLeftHandZ(20);
            _cam.MoveRightHandZ(30);
            _cam.MoveRightHandZ(20);

            Assert.AreEqual(1, step);
        }

        [Test]
        public void Should_notify_gesture() {
            var detected = true;
            var b = new Gesture();
            b.AddStep(50000, Movement.Forward(_cam.LeftHand, 10), Movement.Forward(_cam.RightHand, 10));
            b.AddStep(50000, Movement.Left(_cam.LeftHand, 10), Movement.Right(_cam.RightHand, 10));
            b.GestureDetected += (s, a) => {
                detected = true;
            };
            b.Activate();

            _cam.MoveLeftHandZ(30);
            _cam.MoveLeftHandZ(20);
            _cam.MoveRightHandZ(30);
            _cam.MoveRightHandZ(20);

            _cam.MoveLeftHandX(30);
            _cam.MoveLeftHandX(20);
            _cam.MoveRightHandX(20);
            _cam.MoveRightHandX(30);

            Assert.IsTrue(detected);
        }

        [Test]
        public void Simple_swipe_left() {
            var detected = true;
            var b = new Gesture();
            b.AddStep(50000, Movement.Left(_cam.LeftHand, 10));
            b.GestureDetected += (s, a) => {
                detected = true;
            };
            b.Activate();
            Assert.IsTrue(detected);
        }
    }
}
