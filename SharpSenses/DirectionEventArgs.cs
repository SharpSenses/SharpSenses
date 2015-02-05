using System;
using SharpSenses.Gestures;

namespace SharpSenses {
    public class DirectionEventArgs : EventArgs {
        public Direction OldDirection { get; set; }
        public Direction NewDirection { get; set; }

        public DirectionEventArgs(Direction oldDirection, Direction newDirection) {
            OldDirection = oldDirection;
            NewDirection = newDirection;
        }
    }
}