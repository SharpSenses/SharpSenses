using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpSenses.Storage {
    public class UserIdRepository {
        private const string UserIdStorageName = "SharpSensesUserIds.txt";
        private Dictionary<int, string> _dic = new Dictionary<int, string>();

        private static object _sync = new object();

        public UserIdRepository() {
            EnsureDb();
        }

        public string LoadNameOrEmpty(int id) {
            lock (_sync) {
                return !_dic.ContainsKey(id) ? "" : _dic[id];
            }
        }

        public void Save(int id, string name) {
            lock (_sync) {
                _dic[id] = name;
                Persist();
            }
        }

        private void EnsureDb() {
            lock (_sync) {
                if (!File.Exists(UserIdStorageName)) {
                    File.Create(UserIdStorageName);
                }
                var lines = File.ReadAllLines(UserIdStorageName);
                foreach (var line in lines) {
                    var keyValue = line.Split('=');
                    if (keyValue.Length != 2) continue;
                    _dic[Convert.ToInt32(keyValue[0])] = keyValue[1];
                }
            }
        }

        private void Persist() {
            File.WriteAllLines(UserIdStorageName, _dic.Select(x => x.Key + "=" + x.Value));
        }
    }
}
