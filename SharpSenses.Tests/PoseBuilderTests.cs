using NUnit.Framework;
using SharpSenses.Poses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.Tests {
    public class PoseBuilderTests {
        private FakeCamera _cam;

        [SetUp]
        public void SetUp() {
            _cam = new FakeCamera();
        }

        [Test]
        public void Test() {
            var pose = PoseBuilder.Create().ShouldTouch(_cam.Face.Month, _cam.LeftHand.Index).Build();
            bool ok = false;
            pose.Begin += p => {
                ok = true;
            };
            Assert.IsTrue(ok, "Pose triggered");
        }
    }
}
