using System;
using System.Collections.Generic;

namespace SharpSenses.Gestures {
    public class Gesture {
        private object _sync = new object();
        public event Action GestureDetected;
        public event Action<int> NextStep;

        protected int CurrentStep;
        protected List<GestureStep> GestureSteps = new List<GestureStep>();

        public void AddStep(int windowInMilli, params Movement[] movements) {
            AddStep(new GestureStep(TimeSpan.FromMilliseconds(windowInMilli), movements));
        }

        public void AddStep(GestureStep step) {
            CurrentStep = 0;
            GestureSteps.Add(step);
            step.StepCompleted += AdvanceStep;
        }

        public void Activate() {
            CurrentStep = 0;
            ChangeStep();
        }

        private void ChangeStep() {
            lock (_sync) {
                GestureSteps.ForEach(x => x.Deactivate());
                GestureSteps[CurrentStep].Activate();                
            }
        }

        private void AdvanceStep() {
            lock (_sync) {
                CurrentStep++;
                if (IsOver()) {
                    OnGestureDetected();
                    StartOver();
                }
                else {
                    OnNextStep(CurrentStep);
                    ChangeStep();
                }
            }
        }

        private bool IsOver() {
            return CurrentStep >= GestureSteps.Count;
        }

        private void StartOver() {
            CurrentStep = 0;
            ChangeStep();
        }

        protected virtual void OnNextStep(int obj) {
            Action<int> handler = NextStep;
            if (handler != null) handler(obj);
        }


        protected virtual void OnGestureDetected() {
            Action handler = GestureDetected;
            if (handler != null) handler();
        }

    }
}