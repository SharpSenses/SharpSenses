using SharpSenses.Gestures;

namespace SharpSenses.Poses {
    public static class PosePeace {
        public static void Configue(Hand hand, PoseSensor poseSensor) {
            var pose = Build(hand);
            pose.Begin += (s,a) => poseSensor.OnPosePeaceBegin(hand);
            pose.End += (s, a) => poseSensor.OnPosePeaceEnd(hand);
        }
        private static Pose Build(Hand hand) {
            return new PoseBuilder()
                .ShouldBe(hand.Index, State.Opened)
                .ShouldBe(hand.Middle, State.Opened)
                .ShouldBe(hand.Pinky, State.Closed)
                .ShouldBe(hand.Ring, State.Closed)
                .ShouldBe(hand.Thumb, State.Closed)
                .HoldPoseFor(500)
                .Build("Peace");
        }
    }
}