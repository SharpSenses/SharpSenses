using System;
using System.Collections.Generic;
using SharpSenses.Gestures;

namespace SharpSenses.Poses {

    public class PoseBuilder  {
        private List<PoseTrigger> _items = new List<PoseTrigger>();

        private PoseBuilder() { }

        public static PoseBuilder Combine(FlexiblePart what, State trigger) {
            var builder = new PoseBuilder();
            return builder.With(what, trigger);
        }

        public PoseBuilder With(FlexiblePart what, State trigger) {
            _items.Add(new PoseTrigger(what, trigger));
            return this;
        }

        public Pose Build(string name = "custompose") {
            var pose = new Pose(name);
            foreach (var itemState in _items) {
                var item = itemState.What;
                var state = itemState.Trigger;
                int id = pose.AddFlag();
                switch (state) {
                    case State.Opened:
                        item.Opened += () => pose.Flag(id, true);
                        item.Closed += () => pose.Flag(id, false);
                        break;
                    case State.Closed:
                        item.Closed += () => pose.Flag(id, true);
                        item.Opened += () => pose.Flag(id, false);
                        break;
                    case State.Visible:
                        item.Visible += () => pose.Flag(id, true);
                        item.NotVisible += () => pose.Flag(id, false);
                        break;
                    case State.NotVisible:
                        item.NotVisible += () => pose.Flag(id, true);
                        item.Visible += () => pose.Flag(id, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _items.Clear();
            return pose;
        }

        private class PoseTrigger {
            public FlexiblePart What { get; private set; }
            public State Trigger { get; private set; }

            public PoseTrigger(FlexiblePart what, State trigger) {
                What = what;
                Trigger = trigger;
            }
        }
    }
}