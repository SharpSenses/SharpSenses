using System;

namespace SharpSenses {
    public interface ISpeech {
        SupportedLanguage CurrentLanguage { get; set; }
        void Say(string sentence);
        void Say(string sentence, SupportedLanguage language);
        void EnableRecognition();
        void EnableRecognition(SupportedLanguage language);
        void DisableRecognition();
        event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;
    }
}