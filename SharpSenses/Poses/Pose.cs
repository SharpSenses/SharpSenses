using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSenses.Poses {
    public class Pose {
        private volatile bool _active;
        private Dictionary<int, bool> _flags = new Dictionary<int, bool>();

        public event Action<string> Begin;
        public event Action<string> End;

        public string Name { get; set; }

        public Pose(string name = "pose") {
            Name = name;
        }

        protected virtual void OnEnd() {
            Action<string> handler = End;
            if (handler != null) handler(Name);
        }

        protected virtual void OnBegin() {
            Action<string> handler = Begin;
            if (handler != null) handler(Name);
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
                    _waiting = true;
                    _token = new CancellationTokenSource();
                    Task.Run(async () => {
                        try {
                            await Task.Delay(300, _token.Token);
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
    }
}