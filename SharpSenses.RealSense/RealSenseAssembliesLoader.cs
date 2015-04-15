using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.RealSense {
    public static class RealSenseAssembliesLoader {

        private static bool _isInitialized = false;

        public static void Load() {
            if (_isInitialized) {
                return;
            }

            string platform = Environment.Is64BitProcess ? "x64" : "x32";
            //string managedRef = String.Format("SharpSenses.RealSense.{0}.libpxcclr.cs.dll", platform);
            string unmanagedRef = String.Format("SharpSenses.RealSense.{0}.libpxccpp2c.dll", platform);

            //AppDomain.CurrentDomain.AssemblyResolve += (s, a) => Assembly.Load(GetAssembly(managedRef));
            File.WriteAllBytes("libpxccpp2c.dll", GetAssembly(unmanagedRef));
        }

        private static byte[] GetAssembly(string path) {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stm = assembly.GetManifestResourceStream(path)) {
                var ba = new byte[(int) stm.Length];
                stm.Read(ba, 0, (int) stm.Length);
                return ba;
            }
        }
    }
}
