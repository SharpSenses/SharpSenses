using NUnit.Framework;

namespace SharpSenses.Tests {
    public class FlexiblePartTests {
        private Finger _finger;

        public FlexiblePartTests() {
            _finger = new Finger(FingerKind.Index);
        }

        [Test]
        public void Should_notify_position_change() {
            var prop = "";
            object sender = null;
            _finger.PropertyChanged += (s, args) => {
                prop = args.PropertyName;
                sender = s;
            };
            _finger.Position = new Position {Image = new Point3D(1, 1, 1)};
            Assert.AreEqual("Position", prop);
            Assert.AreSame(_finger, sender);
        }

        [Test]
        public void Should_notify_IsOpen_change() {
            var prop = "";
            object sender = null;
            _finger.PropertyChanged += (s, args) => {
                prop = args.PropertyName;
                sender = s;
            };
            _finger.IsOpen = true;
            Assert.AreEqual("IsOpen", prop);
            Assert.AreSame(_finger, sender);
        }
    }
}