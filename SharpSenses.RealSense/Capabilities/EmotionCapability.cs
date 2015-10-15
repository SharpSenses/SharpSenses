using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SharpSenses.RealSense.Capabilities {
    public class EmotionCapability : ICapability {
        private RealSenseCamera _camera;
        public IEnumerable<Capability> Dependencies => new[] {Capability.FaceTracking};

        public void Configure(RealSenseCamera camera) {
            _camera = camera;
            _camera.Manager.EnableEmotion();
            Debug.WriteLine("EmotionCapability enabled");
        }

        public void Loop(LoopObjects loopObjects) {
            if (!_camera.Face.IsVisible) {
                return;
            }
            var emotionInfo = _camera.Manager.QueryEmotion();
            if (emotionInfo == null) {
                return;
            }
            PXCMEmotion.EmotionData[] allEmotions;
            emotionInfo.QueryAllEmotionData(0, out allEmotions);
            emotionInfo.Dispose();
            if (allEmotions == null) {
                return;
            }
            var emotions =
                allEmotions.Where(e => e.eid > 0 && (int)e.eid <= 64 && e.intensity > 0.4).ToList();
            if (emotions.Any()) {
                var emotion = emotions.OrderByDescending(e => e.evidence).First();
                _camera.Face.Emotion = (Emotion)emotion.eid;
            }
            else {
                _camera.Face.Emotion = Emotion.None;
            }
        }

        public void Dispose() {}
    }
}