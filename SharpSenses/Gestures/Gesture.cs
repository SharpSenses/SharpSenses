using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpSenses.Gestures {
    public class Gesture {

        public static int DetayBetweenDetectionInMilli = 500;

        private object _sync = new object();
        public event EventHandler<GestureEventArgs> GestureDetected;
        public event EventHandler<GestureStepEventArgs> NextStep;
        public event EventHandler<GestureStepEventArgs> StepProgress;

        public string Name { get; set; }

        protected int CurrentStep;
        protected List<GestureStep> GestureSteps = new List<GestureStep>();

        public Gesture() { }

        public Gesture(string name) {
            Name = name;
        }

        public void AddStep(int windowInMilli, params Movement[] movements) {
            AddStep(new GestureStep(TimeSpan.FromMilliseconds(windowInMilli), movements));
        }

        public void AddStep(GestureStep step) {
            CurrentStep = 0;
            GestureSteps.Add(step);
            step.StepProgress += OnStepProgress;
            step.StepCompleted += AdvanceStep;
        }

        public void Activate() {
            CurrentStep = 0;
            ChangeStep();
        }

        public void Deactivate() {
            CurrentStep = 0;
            lock (_sync) {
                foreach (var gestureStep in GestureSteps) {
                    gestureStep.Deactivate();
                }
            }
        }

        private void ChangeStep() {
            lock (_sync) {
                foreach (var gestureStep in GestureSteps) {
                    gestureStep.Deactivate();
                }
                GestureSteps[CurrentStep].Activate();
            }
        }

        private void AdvanceStep() {
            lock (_sync) {
                CurrentStep++;
                if (IsOver()) {
                    OnGestureDetected();
                    PauseAndRestart();
                }
                else {
                    OnNextStep(CurrentStep);
                    ChangeStep();
                }
            }
        }

        private void PauseAndRestart() {
            Deactivate();
            Task.Run(async () => {
                await Task.Delay(DetayBetweenDetectionInMilli);
                Activate();
            });
        }

        private bool IsOver() {
            return CurrentStep >= GestureSteps.Count;
        }

        protected virtual void OnNextStep(int step) {
            var handler = NextStep;
            if (handler != null) handler(this, new GestureStepEventArgs(Name, step));
        }


        protected virtual void OnGestureDetected() {
            var handler = GestureDetected;
            if (handler != null) handler(this, new GestureEventArgs(Name));
        }

        protected virtual void OnStepProgress(double progress) {
            var handler = StepProgress;
            if (handler != null) handler(this, new GestureStepEventArgs(Name, CurrentStep, progress));
        }
    }
}