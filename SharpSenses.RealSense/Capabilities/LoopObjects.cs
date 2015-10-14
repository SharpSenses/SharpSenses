using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpSenses.RealSense.Capabilities {
    public class LoopObjects {
        private Dictionary<Type, object> _dic = new Dictionary<Type, object>();

        public T Get<T>() where  T : class {
            if (!_dic.ContainsKey(typeof (T))) {
                return null;
            }
            return (T) _dic[typeof (T)];
        }

        public void Add<T>(T obj) where T : class {
            _dic[typeof (T)] = obj;
        }
    }
}