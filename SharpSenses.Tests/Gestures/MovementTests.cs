using NUnit.Framework;
using SharpSenses.Gestures;

namespace SharpSenses.Tests {
    public class MovementTests {
        private FakeCamera _camera;
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
            _camera.MoveLeftHandZ(2);
            _camera.MoveLeftHandZ(1);
            Assert.IsTrue(progress > 0);
        }

        [Test]
        public void Should_Restart_when_wrong_direction() {
            bool restarted = false;
            _movement.Restarted += () => restarted = true;
            _camera.MoveLeftHandZ(2);
            _camera.MoveLeftHandZ(1);
            _camera.MoveLeftHandZ(2);
            Assert.AreEqual(MovementStatus.Idle, _movement.Status);
            Assert.IsTrue(restarted);
        }

        [Test]
        public void Should_complete_movement() {
            var completed = true;
            _movement.Completed += () => completed = true;
            _camera.MoveLeftHandZ(1);
            _camera.MoveLeftHandZ(5);
            Assert.IsTrue(completed);
        }
    }
}