using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SharpSenses.RealSense {
    public class SpeechRecognition : IDisposable {
        private PXCMSession _session;
        private PXCMSpeechRecognition _speechRecognition;
        private PXCMSpeechRecognition.Handler _speechRecognitionHandler;
        private Dictionary<SupportedLanguage, PXCMSpeechRecognition.ProfileInfo> _recognitionProfiles;
        private SupportedLanguage _language;

        public event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;

        public SpeechRecognition() {
            _recognitionProfiles = new Dictionary<SupportedLanguage, PXCMSpeechRecognition.ProfileInfo>();
            _speechRecognitionHandler = new PXCMSpeechRecognition.Handler {
                onRecognition = OnRecognition,
                onAlert = OnAlert
            };
        }

        private void OnAlert(PXCMSpeechRecognition.AlertData data) {
            Debug.WriteLine("SpeechRecognition alert: " + data.label);
        }

        public void EnableRecognition(SupportedLanguage language) {
            _language = language;
            _session = PXCMSession.CreateInstance();
            var audioSource = FindAudioSource();
            _session.CreateImpl(out _speechRecognition);
            for (int i = 0; ; i++) {
                PXCMSpeechRecognition.ProfileInfo profile;
                if (_speechRecognition.QueryProfile(i, out profile) != RealSenseCamera.NoError) {
                    break;
                }
                var languageLabel = profile.language.ToString();
                SupportedLanguage sdkLanguage = SupportedLanguageMapper.FromString(languageLabel);
                if (sdkLanguage != SupportedLanguage.NotSpecified) {
                    _recognitionProfiles.Add(sdkLanguage, profile);
                }
            }
            if (language == SupportedLanguage.NotSpecified) {
                language = _recognitionProfiles.Keys.First();
            }
            if (!_recognitionProfiles.ContainsKey(language)) {
                throw new LanguageNotSupportedException(language);
            }
            _speechRecognition.SetProfile(_recognitionProfiles[language]);
            _speechRecognition.SetDictation();
            _speechRecognition.StartRec(audioSource, _speechRecognitionHandler);
        }

        private PXCMAudioSource FindAudioSource() {
            PXCMAudioSource audioSource = _session.CreateAudioSource();
            audioSource.ScanDevices();
            int devicesCount = audioSource.QueryDeviceNum();
            var deviceIndex = 0;
            PXCMAudioSource.DeviceInfo deviceInfo;
            for (int i = 0; i < devicesCount; i++) {
                audioSource.QueryDeviceInfo(i, out deviceInfo);
                if (deviceInfo.name.Contains("Array")) {
                    deviceIndex = i;
                    break;
                }
            }
            audioSource.QueryDeviceInfo(deviceIndex, out deviceInfo);
            audioSource.SetDevice(deviceInfo);
            audioSource.SetVolume(0.8f);
            return audioSource;
        }

        private void OnRecognition(PXCMSpeechRecognition.RecognitionData data) {
            FireSpeechRecognized(data.scores[0].sentence);
        }
        protected virtual void FireSpeechRecognized(string sentence) {
            var handler = SpeechRecognized;
            if (handler != null) handler(this, new SpeechRecognitionEventArgs(sentence));
        }

        public void DisableRecognition() {
            if (_speechRecognition == null) {
                return;
            }
            _speechRecognition.StopRec();
            _speechRecognition.SilentlyDispose();
        }

        public void Dispose() {
            _speechRecognition.SilentlyDispose();
            _session.SilentlyDispose();
        }
    }
}