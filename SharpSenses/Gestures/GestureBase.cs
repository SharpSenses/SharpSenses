using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public abstract class GestureBase {
        public event Action GestureDetected;

        protected virtual void OnGestureDetected() {
            Action handler = GestureDetected;
            if (handler != null) handler();
        }
        protected int CurrentStep;
        protected List<Movement> GestureSteps;

        protected GestureBase(Item item) {
            item.NotVisible += () => CurrentStep = 0;
            item.Moved += HandMoved;
        }

        private void HandMoved(Point3D position) {
            EnsureSteps();
            var step = GestureSteps[CurrentStep];
            if (!step.ComputePosition(position)) {
                return;
            }
            NextStep();
            if (IsOver()) {
                OnGestureDetected();
                StartOver();
            }
        }

        private void StartOver() {
            CurrentStep = 0;
        }

        private void NextStep() {
            CurrentStep++;
        }

        private bool IsOver() {
            return CurrentStep >= GestureSteps.Count;
        }

        private void EnsureSteps() {
            if (GestureSteps == null) {
                GestureSteps = GetGestureSteps().ToList();
            }
        }

        protected abstract IEnumerable<Movement> GetGestureSteps();
    }
}