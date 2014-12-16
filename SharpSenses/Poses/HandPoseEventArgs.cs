using System;

namespace SharpSenses.Poses {
    public class HandPoseEventArgs : EventArgs {
        public Hand Hand { get; set; }
        public HandPoseEventArgs(Hand hand) {
            Hand = hand;
        }
    }
}