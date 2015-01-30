using System;

namespace SharpSenses.RealSense {
    public class Speech : ISpeech, IDisposable {
        private readonly RealSenseCamera _camera;
        private PXCMSpeechSynthesis _speechSynthesisModule;
        private PXCMSpeechSynthesis.ProfileInfo _pinfo;
        public Speech(RealSenseCamera camera) {
            _camera = camera;
        }

        public void Say(string what) {
            EnsureModule();
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

        private void EnsureModule() {
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
    }
}