using System;

namespace SharpSenses {
    public interface ISpeech {
        void Say(string what);
        void EnableRecognition();
        void DisableRecognition();
        event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;
    }
}