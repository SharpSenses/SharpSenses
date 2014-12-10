using System;
using System.Collections.Generic;
using SharpSenses.Gestures;
using SharpSenses.Util;

namespace SharpSenses.Poses {

    public class PoseBuilder  {
        private List<PoseStateTrigger> _stateItems = new List<PoseStateTrigger>();
        private List<ItemPositionTrigger> _positionItems = new List<ItemPositionTrigger>();
        private int _poseThreshold = 500;

        public static PoseBuilder Create() {
            return new PoseBuilder();
        }

        public PoseBuilder ShouldTouch(Item itemA, Item itemB) {
            _positionItems.Add(new ItemPositionTrigger(itemA, itemB));
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
            foreach (var itemState in _stateItems) {
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

            foreach(var itemPosition in _positionItems) {
                var itemA = itemPosition.ItemA;
                var itemB = itemPosition.ItemB;
                int id = pose.AddFlag();
                var trigger = itemPosition;
                itemA.Moved += p => pose.Flag(id, IsCloseEnough(trigger));
                itemB.Moved += p => pose.Flag(id, IsCloseEnough(trigger));
            }

            _positionItems.Clear();
            _stateItems.Clear();
            return pose;
        }

        private bool IsCloseEnough(ItemPositionTrigger trigger) {
            double dist = Math.Abs(MathEx.CalcDistance(trigger.ItemA.Position.Image, trigger.ItemB.Position.Image));
            bool itIs = trigger.DistanceInCm >= dist;
            return itIs;
        }

        private class ItemPositionTrigger {
            public Item ItemA { get; private set; }
            public Item ItemB { get; private set; }
            public int DistanceInCm { get; private set; }

            public ItemPositionTrigger(Item itemA, Item itemB, int distanceInCm = 10) {
                ItemA = itemA;
                ItemB = itemB;
                DistanceInCm = distanceInCm;
            }
        }

        private class PoseStateTrigger {
            public FlexiblePart What { get; private set; }
            public State Trigger { get; private set; }

            public PoseStateTrigger(FlexiblePart what, State trigger) {
                What = what;
                Trigger = trigger;
            }
        }
    }
}