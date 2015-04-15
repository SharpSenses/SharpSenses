using System;

namespace SharpSenses {
    public class FaceRecognizedEventArgs : EventArgs {
        public int UserId { get; set; }

        public FaceRecognizedEventArgs(int userId) {
            UserId = userId;
        }
    }
}