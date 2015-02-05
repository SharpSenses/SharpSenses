using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSenses.RealSense {
    public class SpeechSynthesis : IDisposable {
        private readonly RealSenseCamera _camera;
        private PXCMSpeechSynthesis _synthesisModule;
        private Dictionary<SupportedLanguage, PXCMSpeechSynthesis.ProfileInfo> _synthesisProfiles;

        public SpeechSynthesis(RealSenseCamera camera) {
            _camera = camera;
            _synthesisProfiles = new Dictionary<SupportedLanguage, PXCMSpeechSynthesis.ProfileInfo>();
        }

        public void Say(string sentense, SupportedLanguage language) {
            EnsureSynthesisModule();
            if (language == SupportedLanguage.NotSpecified) {
                language = _synthesisProfiles.Keys.First();
            }
            if (!_synthesisProfiles.ContainsKey(language)) {
                throw new LanguageNotSupportedException(language);
            }
            _synthesisModule.SetProfile(_synthesisProfiles[language]);
            _synthesisModule.BuildSentence(1, sentense);

            int nbuffers = _synthesisModule.QueryBufferNum(1);
            for (int i = 0; i < nbuffers; i++) {
                PXCMAudio audio = _synthesisModule.QueryBuffer(1, i);
                PXCMAudio.AudioData audioData;
                audio.AcquireAccess(PXCMAudio.Access.ACCESS_READ, PXCMAudio.AudioFormat.AUDIO_FORMAT_PCM, out audioData);
                RealSenseAudioPlayer.Play(audioData, _synthesisProfiles[language].outputs);
                audio.ReleaseAccess(audioData);
            }
            _synthesisModule.ReleaseSentence(1);
        }

        private void EnsureSynthesisModule() {
            if (_synthesisModule != null) {
                return;
            }
            _camera.Session.CreateImpl(out _synthesisModule);
            for (int i = 0;; i++) {
                PXCMSpeechSynthesis.ProfileInfo profile;
                if (_synthesisModule.QueryProfile(i, out profile) != RealSenseCamera.NoError) {
                    break;
                }
                var languageLabel = profile.language.ToString();
                SupportedLanguage language = SupportedLanguageMapper.FromString(languageLabel);
                if (language != SupportedLanguage.NotSpecified) {
                    _synthesisProfiles.Add(language, profile);
                }
            }
        }

        public void Dispose() {
            _synthesisModule.SilentlyDispose();
        }
    }
}