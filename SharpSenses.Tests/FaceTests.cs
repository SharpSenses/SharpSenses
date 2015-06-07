using NUnit.Framework;
using SharpSenses.Poses;

namespace SharpSenses.Tests {
    public class FaceTests {

        [SetUp]
        public void SetUp() {
            PoseBuilder.DefaultPoseThresholdInMillis = 0;
        }

        [Test]
        public void Should_left_wink() {
            bool fired = false;
            var face = new Face(null);
            face.WinkedLeft += (s, a) => {
                fired = true;
            };
            face.LeftEye.IsOpen = false;
            face.RightEye.IsOpen = true;
            
            Assert.IsTrue(fired);
        }

        [Test]
        public void Should_right_wink() {
            bool fired = false;
            var face = new Face(null);
            face.WinkedRight += (s, a) => {
                fired = true;
            };
            face.LeftEye.IsOpen = false;
            face.RightEye.IsOpen = true;

            Assert.IsTrue(fired);
        }
    }
}
