using System.Collections.Generic;

namespace SharpSenses.Gestures {
    public class CustomGesture : GestureBase {

        private List<Movement> _movements = new List<Movement>(); 

        public CustomGesture(Item item) : base(item) {}
        protected override IEnumerable<Movement> GetGestureSteps() {
            return _movements;
        }
        public void AddMovement(Movement movement) {
            _movements.Add(movement);
        }
    }
}