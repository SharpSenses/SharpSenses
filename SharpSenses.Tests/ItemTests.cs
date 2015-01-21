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
    }

    
}
