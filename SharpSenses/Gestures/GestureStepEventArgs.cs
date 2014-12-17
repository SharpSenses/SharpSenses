namespace SharpSenses.Gestures {
    public class GestureStepEventArgs : GestureEventArgs {
        public int Step { get; set; }
        public double StepProgress { get; set; }

        public GestureStepEventArgs(string gestureName, int step = 0, double stepProgress = 0)
            : base(gestureName) {
            Step = step;
            StepProgress = stepProgress;
        }
    }
}