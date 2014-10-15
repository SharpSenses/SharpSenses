using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpSenses.Gestures {
    public class Gesture {
        private object _sync = new object();
        public event Action GestureDetected;
        public event Action<int> NextStep;
        public event Action<double> StepProgress;

        public string Name { get; set; }

        protected int CurrentStep;
        protected List<GestureStep> GestureSteps = new List<GestureStep>();

        public Gesture() {}

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
                GestureSteps.ForEach(x => x.Deactivate());
            }
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
                await Task.Delay(800);
                Activate();
            });
        }

        private bool IsOver() {
            return CurrentStep >= GestureSteps.Count;
        }

        protected virtual void OnNextStep(int obj) {
            Action<int> handler = NextStep;
            if (handler != null) handler(obj);
        }


        protected virtual void OnGestureDetected() {
            Action handler = GestureDetected;
            if (handler != null) handler();
        }

        protected virtual void OnStepProgress(double progress) {
            Action<double> handler = StepProgress;
            if (handler != null) handler(progress);
        }
    }
}