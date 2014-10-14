using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public class GestureStep {
        private object _sync = new object();

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

        public void Activate() {
            lock (_sync) {
                Movements.ForEach(x => x.Activate());                
            }
        }

        public void Deactivate() {
            lock (_sync) {
                Movements.ForEach(x => x.Deactivate());                            
            }
        }

        public void AddMovement(Movement movement) {
            movement.Progress += d => {
                lock (_sync) {
                    if (DateTime.Now - StartTime > Window) {
                        //Movements.ForEach(x => x.Restart());
                    }
                }
            };
            movement.Completed += () => {
                lock (_sync) {
                    if (Movements.All(m => m.Status == MovementStatus.Completed)) {
                        OnStepCompleted();
                        Movements.ForEach(m => m.Restart());
                    }
                }
            };
            movement.AutoRestart = false;
            Movements.Add(movement);
        }

        protected virtual void OnStepCompleted() {
            Action handler = StepCompleted;
            if (handler != null) handler();
        }
    }
}
