using System;
using System.Diagnostics;

namespace SharpSenses.RealSense {
    public class SpeechRecognition : IDisposable {
        private readonly RealSenseCamera _camera;
        private PXCMSpeechRecognition _speechRecognition;
        private PXCMSpeechRecognition.Handler _speechRecognitionHandler;

        public event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;

        public SpeechRecognition(RealSenseCamera camera) {
            _camera = camera;
            _speechRecognitionHandler = new PXCMSpeechRecognition.Handler {
                onRecognition = OnRecognition
            };
        }

        private void SetRecognitionProfile() {
            PXCMSpeechRecognition.ProfileInfo profileInfo;
            _speechRecognition.QueryProfile(1, out profileInfo);
            Debug.WriteLine(profileInfo.language);
            _speechRecognition.SetProfile(profileInfo);
        }

        public void EnableRecognition(SupportedLanguage language) {
            var audioSource = FindAudioSource();
            _camera.Session.CreateImpl(out _speechRecognition);
            SetRecognitionProfile();
            _speechRecognition.SetDictation();
            _speechRecognition.StartRec(audioSource, _speechRecognitionHandler);
        }

        private PXCMAudioSource FindAudioSource() {
            PXCMAudioSource audioSource = _camera.Session.CreateAudioSource();
            audioSource.ScanDevices();
            int devicesCount = audioSource.QueryDeviceNum();
            var deviceIndex = 0;
            PXCMAudioSource.DeviceInfo deviceInfo;
            for (int i = 0; i < devicesCount; i++) {
                audioSource.QueryDeviceInfo(i, out deviceInfo);
                if (deviceInfo.name.Contains("Creative 3D")) {
                    deviceIndex = i;
                    break;
                }
            }
            audioSource.QueryDeviceInfo(deviceIndex, out deviceInfo);
            audioSource.SetDevice(deviceInfo);
            audioSource.SetVolume(0.2f);
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
        }
    }
}