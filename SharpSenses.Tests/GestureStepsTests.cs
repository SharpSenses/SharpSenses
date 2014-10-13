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
        public void Test() {
            var m1 = Movement.Forward(_cam.LeftHand, 10);
            var step = new GestureStep(TimeSpan.FromHours(1), m1);
            var completed = false;
            step.StepCompleted += () => completed = true;
            step.Activate();
            _cam.MoveLeftHandZ(11);
            Assert.IsTrue(completed);
        }
    }
}
