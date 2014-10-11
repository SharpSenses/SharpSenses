using System;
using System.Collections.Generic;

namespace SharpSenses.Gestures {
    public class GesturePunch : GestureBase {
        public double Distance { get; set; }
        public TimeSpan Window { get; set; }

        public GesturePunch(Item item) : base(item) {
            Distance = 30;
            Window = TimeSpan.FromSeconds(1);
        }

        public GesturePunch(Item item, double distance, TimeSpan window) : base(item) {
            Distance = distance;
            Window = window;
        }
        protected override IEnumerable<Movement> GetGestureSteps() {
            yield return new MovementForward(Distance, Window);
        }
    }
}