using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSenses.Poses {
    public class Pose {
        private volatile bool _active;
        private Dictionary<int, bool> _flags = new Dictionary<int, bool>();

        public event EventHandler<PoseEventArgs> Begin;
        public event EventHandler<PoseEventArgs> End;

        public int PoseThresholdInMilli { get; set; }

        public string Name { get; set; }

        public Pose(string name = "pose", int poseThresholdInMilli = 500) {
            Name = name;
            PoseThresholdInMilli = poseThresholdInMilli;
        }
        protected virtual void OnBegin() {
            var handler = Begin;
            if (handler != null) handler(this, new PoseEventArgs(Name));
        }

        protected virtual void OnEnd() {
            var handler = End;
            if (handler != null) handler(this, new PoseEventArgs(Name));
        }

        internal int AddFlag() {
            int id = _flags.Count;
            _flags.Add(id, false);
            return id;
        }

        internal void Flag(int id, bool state) {
            _flags[id] = state;
            Evaluate();
        }

        private void Evaluate() {
            Active = _flags.Values.All(x => x);
        }

        private CancellationTokenSource _token;
        private volatile bool _waiting;

        public bool Active {
            get { return _active; }
            private set {
                if (value == _active) {
                    return;
                }
                _active = value;
                if (_active) {
                    if (PoseThresholdInMilli == 0) {
                        OnBegin();
                        return;
                    }
                    _waiting = true;
                    _token = new CancellationTokenSource();
                    Task.Run(async () => {
                        try {
                            await Task.Delay(PoseThresholdInMilli, _token.Token);
                        }
                        catch {}
                        if (_active) {
                            //Debug.WriteLine("Custom pose begin: " + Name);
                            OnBegin();                            
                        }
                        _waiting = false;
                    });
                }
                else if (_waiting) {
                    if (_token != null) {
                        _token.Cancel();
                        //Debug.WriteLine("Pose cancelled");
                    }
                }
                else {
                    //Debug.WriteLine("Custom pose end: " + Name);
                    OnEnd();
                }
            }
        }

        public class PoseEventArgs : EventArgs {
            public string PoseName { get; set; }
            public PoseEventArgs(string poseName) {
                PoseName = poseName;
            }
        }
    }
}