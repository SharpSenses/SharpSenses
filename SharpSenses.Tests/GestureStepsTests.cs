using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpSenses.Gestures;

namespace SharpSenses.Tests {
    public class GestureStepsTests {
        private FakeCamera _cam;

        [SetUp]
        public void SetUp() {
            _cam = new FakeCamera();
        }

        [Test]
        public void Should_activate_simple_step() {
            var m1 = Movement.Forward(_cam.LeftHand, 10);
            var step = new GestureStep(TimeSpan.FromHours(1), m1);
            var completed = false;
            step.StepCompleted += () => completed = true;
            step.Activate();
            _cam.MoveLeftHandZ(30);
            _cam.MoveLeftHandZ(20);
            Assert.IsTrue(completed);
        }

        [Test]
        public void Should_activate_double_step() {
            var m1 = Movement.Forward(_cam.LeftHand, 10);
            var m2 = Movement.Forward(_cam.RightHand, 10);
            var step = new GestureStep(TimeSpan.FromHours(1), m1, m2);
            var completed = false;
            step.StepCompleted += () => completed = true;
            step.Activate();
            _cam.MoveLeftHandZ(30);
            _cam.MoveLeftHandZ(20);
            _cam.MoveRightHandZ(30);
            _cam.MoveRightHandZ(20);
            Assert.IsTrue(completed);
        }
    }
}
