using System;

namespace SharpSenses {
    public class FacialExpressionEventArgs : EventArgs {
        public Emotion OldEmotion { get; set; }
        public Emotion NewEmotion { get; set; }

        public FacialExpressionEventArgs(Emotion oldEmotion, Emotion newEmotion) {
            OldEmotion = oldEmotion;
            NewEmotion = newEmotion;
        }
    }
}