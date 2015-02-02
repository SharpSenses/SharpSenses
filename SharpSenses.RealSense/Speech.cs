using System;

namespace SharpSenses.RealSense {
    public class Speech : ISpeech, IDisposable {
        private readonly RealSenseCamera _camera;
        private PXCMSpeechSynthesis _speechSynthesisModule;
        private PXCMSpeechSynthesis.ProfileInfo _pinfo;

        private PXCMSpeechRecognition _speechRecognition;
        private PXCMSpeechRecognition.Handler _speechRecognitionHandler;

        public event EventHandler<SpeechRecognitionEventArgs> SpeechRecognized;

        public Speech(RealSenseCamera camera) {
            _camera = camera;
            _speechRecognitionHandler = new PXCMSpeechRecognition.Handler {
                onRecognition = OnRecognition
            };
        }

        public void Say(string what) {
            EnsureSynthesisModule();
            _speechSynthesisModule.BuildSentence(1, what);

            int nbuffers=_speechSynthesisModule.QueryBufferNum(1);
            for (int i = 0;i < nbuffers; i++) {
               PXCMAudio audio =_speechSynthesisModule.QueryBuffer(1, i);
               PXCMAudio.AudioData audioData;
               audio.AcquireAccess(PXCMAudio.Access.ACCESS_READ, PXCMAudio.AudioFormat.AUDIO_FORMAT_PCM, out audioData);
               RealSenseAudioPlayer.Play(audioData, _pinfo.outputs);
               audio.ReleaseAccess(audioData);
            }
            _speechSynthesisModule.ReleaseSentence(1);
        }

        public void EnableRecognition() {
            var audioSource = FindAudioSource();
            _camera.Session.CreateImpl(out _speechRecognition);
            SetRecognitionProfile();
            _speechRecognition.SetDictation();
            _speechRecognition.StartRec(audioSource, _speechRecognitionHandler);
        }

        public void DisableRecognition() {
            if (_speechRecognition == null) {
                return;
            }
            _speechRecognition.StopRec();
            _speechRecognition.SilentlyDispose();
        }

        private void SetRecognitionProfile() {
            PXCMSpeechRecognition.ProfileInfo profileInfo;
            _speechRecognition.QueryProfile(0, out profileInfo);
            _speechRecognition.SetProfile(profileInfo);
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

        private void EnsureSynthesisModule() {
            if (_speechSynthesisModule != null) {
                return;
            }
            _camera.Session.CreateImpl(out _speechSynthesisModule);
            _speechSynthesisModule.QueryProfile(0, out _pinfo);
            _speechSynthesisModule.SetProfile(_pinfo);
        }

        public void Dispose() {
            _speechSynthesisModule.SilentlyDispose();
        }

        protected virtual void FireSpeechRecognized(string sentence) {
            var handler = SpeechRecognized;
            if (handler != null) handler(this, new SpeechRecognitionEventArgs(sentence));
        }
    }
}