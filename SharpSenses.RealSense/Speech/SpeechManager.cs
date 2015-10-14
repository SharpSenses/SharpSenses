using System;

namespace SharpSenses.RealSense.Speech {
    public class SpeechManager : ISpeech, IDisposable {
        
        private SpeechSynthesis _speechSynthesis;
        private SpeechRecognition _speechRecognition;

        public SpeechManager() {
            _speechSynthesis = new SpeechSynthesis();
            _speechRecognition = new SpeechRecognition();
            _speechRecognition.SpeechRecognized += (s, a) => FireSpeechRecognized(a.Sentence);
        }

        public SupportedLanguage CurrentLanguage { get; set; }

        public void Say(string sentence) {
            Say(sentence, CurrentLanguage);
        }

        public void Say(string sentence, SupportedLanguage language) {
            _speechSynthesis.Say(sentence, language);
        }

        public void EnableRecognition() {
            EnableRecognition(CurrentLanguage);
        }

        public void EnableRecognition(SupportedLanguage language) {
            _speechRecognition.EnableRecognition(language);
        }

        public void DisableRecognition() {
            _speechRecognition.DisableRecognition();
        }

        public event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;

        protected virtual void FireSpeechRecognized(string sentence) {
            SpeechRecognized?.Invoke(this, new SpeechRecognitionEventArgs(sentence));
        }

        public void Dispose() {
            _speechSynthesis.SilentlyDispose();
            _speechRecognition.SilentlyDispose();
        }

    }
}