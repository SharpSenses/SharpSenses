using NUnit.Framework;

namespace SharpSenses.Tests {
    public class ItemTests {

        [Test]
        public void Should_notify_is_visible_change() {
            var face = new Face(null);
            var prop = "";
            object sender = null;
            face.PropertyChanged += (s, args) => {
                prop = args.PropertyName;
                sender = s;
            };
            face.IsVisible = true;
            Assert.AreEqual("IsVisible", prop);
            Assert.AreSame(face, sender);
        }

        [Test]
        public void DefaultNoiseThreshold_zero() {
            var item = new Item();
            var count = 0;
            item.Moved += (sender, args) => {
                count++;
            };
            item.NoiseThreshold = 0;
            item.Position = new Position();
            item.Position = new Position {
                Image = new Point3D(1)
            };
            Assert.AreEqual(1, count);
        }

        [Test]
        public void DefaultNoiseThreshold_ten() {
            var item = new Item();
            var count = 0;
            item.Moved += (sender, args) => {
                count++;
            };
            item.NoiseThreshold = 10;
            item.Position = new Position();
            item.Position = new Position {
                Image = new Point3D(1)
            };
            Assert.AreEqual(0, count);
        }
    }
}
