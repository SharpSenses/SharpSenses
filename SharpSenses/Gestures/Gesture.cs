using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.Gestures {
    public class Gesture {
        private object _sync = new object();
        public event Action GestureDetected;
        protected int CurrentStep;
        protected List<GestureStep> GestureSteps = new List<GestureStep>();

        public void AddStep(GestureStep step) {
            CurrentStep = 0;
            GestureSteps.Add(step);
            step.StepCompleted += NextStep;
        }

        public void StartListening() {
            CurrentStep = 0;
            ChangeStep();
        }
        
        private void StartOver() {
            CurrentStep = 0;
            ChangeStep();
        }

        private void ChangeStep() {
            lock (_sync) {
                GestureSteps.ForEach(x => x.Deactivate());
                GestureSteps[CurrentStep].Activate();                
            }
        }

        private void NextStep() {
            lock (_sync) {
                CurrentStep++;
                if (IsOver()) {
                    OnGestureDetected();
                    StartOver();
                }
                else {
                    ChangeStep();
                }
            }
        }

        private bool IsOver() {
            return CurrentStep >= GestureSteps.Count;
        }

        protected virtual void OnGestureDetected() {
            Action handler = GestureDetected;
            if (handler != null) handler();
        }

    }
}