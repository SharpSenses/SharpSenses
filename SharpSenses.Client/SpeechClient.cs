using System;

namespace SharpSenses.Client {
    public class SpeechClient : ISpeech {
        public SupportedLanguage CurrentLanguage { get; set; }
        public void Say(string sentence) {
            throw new NotImplementedException();
        }

        public void Say(string sentence, SupportedLanguage language) {
            throw new NotImplementedException();
        }

        public void EnableRecognition() {
            throw new NotImplementedException();
        }

        public void EnableRecognition(SupportedLanguage language) {
            throw new NotImplementedException();
        }

        public void DisableRecognition() {
            throw new NotImplementedException();
        }

        public event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;
    }
}