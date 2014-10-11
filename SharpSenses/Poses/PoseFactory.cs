using System;
using System.Collections.Generic;
using SharpSenses.Gestures;

namespace SharpSenses.Poses {
    public class PoseFactory {
        private List<PoseTrigger> _items = new List<PoseTrigger>(); 

        public PoseFactory Combine(FlexiblePart what, State trigger) {
            _items.Add(new PoseTrigger(what, trigger));
            return this;
        }

        public CustomPose Build(string name = "custompose") {
            var gesture = new CustomPose(name);
            foreach (var itemState in _items) {
                var item = itemState.What;
                var state = itemState.Trigger;
                int id = gesture.AddFlag();
                switch (state) {
                    case State.Opened:
                        item.Opened += () => gesture.Flag(id, true);
                        item.Closed += () => gesture.Flag(id, false);
                        break;
                    case State.Closed:
                        item.Closed += () => gesture.Flag(id, true);
                        item.Opened += () => gesture.Flag(id, false);
                        break;
                    case State.Visible:
                        item.Visible += () => gesture.Flag(id, true);
                        item.NotVisible += () => gesture.Flag(id, false);
                        break;
                    case State.NotVisible:
                        item.NotVisible += () => gesture.Flag(id, true);
                        item.Visible += () => gesture.Flag(id, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _items.Clear();
            return gesture;
        }

        private class PoseTrigger {
            public FlexiblePart What { get; set; }
            public State Trigger { get; set; }

            public PoseTrigger(FlexiblePart what, State trigger) {
                What = what;
                Trigger = trigger;
            }
        }
    }
}