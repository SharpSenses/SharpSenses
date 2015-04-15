using System.Collections.Generic;

namespace SharpSenses {
    public class Finger : FlexiblePart {

        public FingerKind Kind { get; private set; }
        public Item BaseJoint { get; set; }
        public Item FirstJoint { get; set; }
        public Item SecondJoint { get; set; }

        public Finger(FingerKind kind) {
            Kind = kind;
            BaseJoint = new Item();
            FirstJoint = new Item();
            SecondJoint = new Item();
        }

        public List<Item> GetAllJoints() {
            return new List<Item> {
                BaseJoint,
                FirstJoint,
                SecondJoint
            };
        }
    }
}