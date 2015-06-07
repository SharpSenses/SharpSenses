using NUnit.Framework;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace SharpSenses.Tests {
    public class PoseBuilderTests {
        private FakeCamera _cam;
        private FlexiblePart _part1, _part2;

        [SetUp]
        public void SetUp() {
            _cam = new FakeCamera();
            _part1 = new FlexiblePart();
            _part2 = new FlexiblePart();
        }

        [Test]
        public void Should_create_proximity_pose() {
            var pose = new PoseBuilder().ShouldBeNear(_cam.Face.Mouth, _cam.LeftHand.Index, 20)
                .HoldPoseFor(0)
                .Build();
            bool ok = false;
            pose.Begin += (s, a) => { ok = true; };
            _cam.Face.Mouth.Position = new Position {
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
                .ShouldBeNear(_cam.Face.Mouth, _cam.LeftHand.Index)
                .HoldPoseFor(0)
                .Build();
            bool ok = false;
            pose.Begin += (s, a) => { ok = true; };
            _cam.Face.Mouth.Position = new Position {
                Image = new Point3D(10, 10)
            };
            _cam.LeftHand.Index.Position = new Position {
                Image = new Point3D(10, 10)
            };
            _cam.LeftHand.Index.IsOpen = true;
            Assert.IsTrue(ok, "Pose not triggered");
        }

        [Test]
        public void Should_create_simple_pose() {
            var called = false;
            var b = new PoseBuilder();
            var pose = b.ShouldBe(_part1, State.Opened).HoldPoseFor(0).Build("p");
            pose.Begin += (sender, args) => {
                called = true;
            };
            _part1.IsOpen = true;
            Assert.IsTrue(called);
        }

        [Test]
        public void Should_set_initial_value() {
            _part1.IsOpen = true;
            var called = false;
            var b = new PoseBuilder();
            var pose = b.ShouldBe(_part1, State.Opened).HoldPoseFor(0).Build("p");
            pose.Begin += (sender, args) => {
                called = true;
            };
            Assert.IsTrue(called);
        }
    }
}