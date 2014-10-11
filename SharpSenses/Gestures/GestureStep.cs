using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public class GestureStep {

        public event Action StepCompleted;
        public TimeSpan Window { get; set; }
        public List<Movement> Movements { get; set; }

        protected DateTime StartTime { get; set; }

        public GestureStep(TimeSpan window, params Movement[] movements) {
            Window = window;
            Movements = new List<Movement>();
            if (movements == null) {
                return;
            }
            foreach (var movement in movements) {
                AddMovement(movement);
            }
        }

        public void AddMovement(Movement movement) {
            movement.Update += () => {
                if (DateTime.Now - StartTime > Window) {
                    movement.Reset();
                }
            };
            movement.Completed += () => {
                if (Movements.All(m => m.Status == MovementStatus.Completed)) {
                    OnStepCompleted();
                }
            };
        }

        protected virtual void OnStepCompleted() {
            Action handler = StepCompleted;
            if (handler != null) handler();
        }
    }
}
