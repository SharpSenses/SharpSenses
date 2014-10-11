using System.Runtime.CompilerServices;
using SharpSenses.Gestures;

namespace SharpSenses.Poses {
    public static class PosePeace {
        public static void Configue(Hand hand, PoseSensor poseSensor) {
            var pose = Build(hand);
            pose.Begin += s => poseSensor.OnPosePeaceBegin(hand);
            pose.End += s => poseSensor.OnPosePeaceEnd(hand);
        }
        private static Pose Build(Hand hand) {
            return PoseBuilder.Combine(hand.Index, State.Opened)
                .With(hand.Middle, State.Opened)
                .With(hand.Pinky, State.Closed)
                .With(hand.Ring, State.Closed)
                .With(hand.Thumb, State.Closed)
                .Build("Peace");
        }
    }
}