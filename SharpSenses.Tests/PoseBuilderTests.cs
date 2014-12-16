using NUnit.Framework;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.Tests {
    public class PoseBuilderTests {
        private FakeCamera _cam;

        [SetUp]
        public void SetUp() {
            _cam = new FakeCamera();
        }

        [Test]
        public void Should_create_proximity_pose() {
            var pose = new PoseBuilder().ShouldTouch(_cam.Face.Month, _cam.LeftHand.Index)
                .HoldPoseFor(0)
                .Build();
            bool ok = false;
            pose.Begin += (s, a) => { ok = true; };
            _cam.Face.Month.Position = new Position {
                Image = new Point3D(10, 10)
            };
            _cam.LeftHand.Index.Position = new Position {
                Image = new Point3D(10, 10)
            };
            Assert.IsTrue(ok, "Pose triggered");
        }

        [Test]
        public void Should_create_combined_poses() {
            var pose = new PoseBuilder().ShouldBe(_cam.LeftHand.Index, State.Opened)
                .ShouldTouch(_cam.Face.Month, _cam.LeftHand.Index)
                .HoldPoseFor(0)
                .Build();
            bool ok = false;
            pose.Begin += (s, a) => { ok = true; };
            _cam.Face.Month.Position = new Position {
                Image = new Point3D(10, 10)
            };
            _cam.LeftHand.Index.Position = new Position {
                Image = new Point3D(10, 10)
            };
            _cam.LeftHand.Index.IsOpen = true;
            Assert.IsTrue(ok, "Pose not triggered");
        }
    }
}