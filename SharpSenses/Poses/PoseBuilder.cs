using System;
using System.Collections.Generic;
using SharpSenses.Gestures;
using SharpSenses.Util;

namespace SharpSenses.Poses {

    public class PoseBuilder {

        public static int DefaultPoseThresholdInMillis = 0;

        private List<PoseStateTrigger> _stateItems = new List<PoseStateTrigger>();
        private List<ItemPositionTrigger> _positionItems = new List<ItemPositionTrigger>();
        private int _poseThreshold = DefaultPoseThresholdInMillis;

        public static PoseBuilder Create() {
            return new PoseBuilder();
        }

        public PoseBuilder ShouldBeNear(Item itemA, Item itemB, int distanceInPx = 30) {
            _positionItems.Add(new ItemPositionTrigger(itemA, itemB, distanceInPx));
            return this;
        }

        public PoseBuilder ShouldBe(FlexiblePart what, State trigger) {
            _stateItems.Add(new PoseStateTrigger(what, trigger));
            return this;
        }

        public PoseBuilder HoldPoseFor(int milliseconds) {
            _poseThreshold = milliseconds;
            return this;
        }

        public Pose Build(string name = "custompose") {
            var pose = new Pose(name, _poseThreshold);
            BindItemStateEvents(pose);
            BindItemPositionEvents(pose);
            _stateItems.Clear();
            _positionItems.Clear();
            return pose;
        }

        private void BindItemStateEvents(Pose pose) {
            foreach (var itemState in _stateItems) {
                var item = itemState.What;
                var state = itemState.Trigger;
                int id = pose.AddFlag();
                switch (state) {
                    case State.Opened:
                        item.Opened += (s, a) => pose.Flag(id, true);
                        item.Closed += (s, a) => pose.Flag(id, false);
                        break;
                    case State.Closed:
                        item.Closed += (s, a) => pose.Flag(id, true);
                        item.Opened += (s, a) => pose.Flag(id, false);
                        break;
                    case State.Visible:
                        item.Visible += (s, a) => pose.Flag(id, true);
                        item.NotVisible += (s, a) => pose.Flag(id, false);
                        break;
                    case State.NotVisible:
                        item.NotVisible += (s, a) => pose.Flag(id, true);
                        item.Visible += (s, a) => pose.Flag(id, false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void BindItemPositionEvents(Pose pose) {
            foreach (var itemPosition in _positionItems) {
                var itemA = itemPosition.ItemA;
                var itemB = itemPosition.ItemB;
                int id = pose.AddFlag();
                var trigger = itemPosition;
                itemA.Moved += (s, a) => pose.Flag(id, IsCloseEnough(trigger));
                itemB.Moved += (s, a) => pose.Flag(id, IsCloseEnough(trigger));
            }
        }

        private bool IsCloseEnough(ItemPositionTrigger trigger) {
            double dist = Math.Abs(MathEx.CalcDistance(trigger.ItemA.Position.Image, trigger.ItemB.Position.Image));
            //Debug.WriteLine("Dist: -> " + dist);
            bool itIs = trigger.DistanceInPx >= dist;
            return itIs;
        }

        private class PoseStateTrigger {
            public FlexiblePart What { get; private set; }
            public State Trigger { get; private set; }

            public PoseStateTrigger(FlexiblePart what, State trigger) {
                What = what;
                Trigger = trigger;
            }
        }

        private class ItemPositionTrigger {
            public Item ItemA { get; private set; }
            public Item ItemB { get; private set; }
            public int DistanceInPx { get; private set; }

            public ItemPositionTrigger(Item itemA, Item itemB, int distanceInPx = 10) {
                ItemA = itemA;
                ItemB = itemB;
                DistanceInPx = distanceInPx;
            }
        }
    }
}