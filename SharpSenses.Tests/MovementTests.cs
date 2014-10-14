using NUnit.Framework;
using SharpSenses.Gestures;

namespace SharpSenses.Tests {
    public class MovementTests {
        private ICamera _camera;
        private Movement _movement;


        [SetUp]
        public void SetUp() {
            _camera = new FakeCamera();
            _movement = new MovementForward(_camera.LeftHand, 5);
            _movement.Activate();
        }

        [Test]
        public void Should_Report_Progress() {
            double progress = 0;
            _movement.Progress += d => progress = d;
            SetHandZ(1);
            SetHandZ(2);
            Assert.IsTrue(progress > 0);
        }

        [Test]
        public void Should_Restart_when_wrong_direction() {
            bool restarted = false;
            _movement.Restarted += () => restarted = true;
            SetHandZ(1);
            SetHandZ(2);
            SetHandZ(1);
            Assert.AreEqual(MovementStatus.Idle, _movement.Status);
            Assert.IsTrue(restarted);
        }

        [Test]
        public void Should_complete_movement() {
            var completed = true;
            _movement.Completed += () => completed = true;
            SetHandZ(1);
            SetHandZ(5);
            Assert.IsTrue(completed);
        }

        private void SetHandZ(double location) {
            _camera.LeftHand.Position = new Point3D(0,0,location);
        }
    }
}