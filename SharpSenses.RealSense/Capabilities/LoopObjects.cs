using System.Collections.Generic;

namespace SharpSenses.RealSense.Capabilities {
    public class LoopObjects {
        private Dictionary<string, object> _dic = new Dictionary<string, object>();

        public T Get<T>(string tag = "") where  T : class {
            if (!_dic.ContainsKey(GetKey<T>(tag))) {
                return null;
            }
            return (T) _dic[GetKey<T>(tag)];
        }

        public void Add<T>(T obj, string tag = "") where T : class {
            _dic[GetKey<T>(tag)] = obj;
        }

        private static string GetKey<T>(string tag) where T : class {
            return typeof(T) + tag;
        }
    }
}