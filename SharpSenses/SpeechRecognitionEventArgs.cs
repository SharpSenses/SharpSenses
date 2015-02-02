using System;

namespace SharpSenses {
    public class SpeechRecognitionEventArgs : EventArgs {
        public string Sentence { get; set; }

        public SpeechRecognitionEventArgs(string sentence) {
            Sentence = sentence;
        }
    }
}