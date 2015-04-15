using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public class GestureStep {
        private object _sync = new object();
        private double _progress;
        private DateTime _startTime;

        public event Action<double> StepProgress;
        public event Action StepCompleted;
        public TimeSpan Window { get; set; }
        public List<Movement> Movements { get; set; }

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
            movement.Progress += d => {
                lock (_sync) {
                    if (DateTime.Now - _startTime > Window) {
                        _progress = 0;
                        foreach (var m in Movements) {
                            m.Restart();
                        }
                        _startTime = DateTime.Now;
                        return;
                    }
                    _progress = d;
                    OnStepProgress(_progress);
                }
            };
            movement.Completed += () => {
                lock (_sync) {
                    if (Movements.All(m => m.Status == MovementStatus.Completed)) {
                        OnStepCompleted();
                        _progress = 0;
                        foreach (var m in Movements) {
                            m.Restart();
                        }
                    }
                }
            };
            movement.AutoRestart = false;
            Movements.Add(movement);
        }

        public void Activate() {
            lock (_sync) {
                _progress = 0;
                _startTime = DateTime.Now;
                foreach (var m in Movements) {
                    m.Activate();
                }
            }
        }

        public void Deactivate() {
            lock (_sync) {
                _progress = 0;
                foreach (var m in Movements) {
                    m.Deactivate();
                }
            }
        }

        protected virtual void OnStepCompleted() {
            Action handler = StepCompleted;
            if (handler != null) handler();
        }

        protected virtual void OnStepProgress(double progress) {
            Action<double> handler = StepProgress;
            if (handler != null) handler(progress);
        }
    }
}
