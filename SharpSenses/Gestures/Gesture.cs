using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public class Gesture {
        public event Action GestureDetected;
        protected int CurrentStep;
        protected List<GestureStep> GestureSteps = new List<GestureStep>();

        public void AddStep(GestureStep step) {
            CurrentStep = 0;
            GestureSteps.Add(step);
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

        //protected abstract IEnumerable<Movement> GetGestureSteps();

        protected virtual void OnGestureDetected() {
            Action handler = GestureDetected;
            if (handler != null) handler();
        }

    }
}