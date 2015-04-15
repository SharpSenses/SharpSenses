using System;

namespace SharpSenses.Gestures {
    public class GestureEventArgs : EventArgs {
        public string GestureName { get; set; }

        public GestureEventArgs(string name) {
            GestureName = name;
        }
    }
}