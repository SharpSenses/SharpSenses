using System;
using System.IO;
using System.Reflection;

namespace SharpSenses.RealSense {
    public static class RealSenseAssembliesLoader {
        private static bool _isInitialized = false;

        public static void Load() {
            if (_isInitialized) {
                return;
            }
            var platform = Environment.Is64BitProcess ? "x64" : "x32";
            var unmanagedRef = $"SharpSenses.RealSense.{platform}.libpxccpp2c.dll";
            File.WriteAllBytes("libpxccpp2c.dll", GetAssembly(unmanagedRef));
        }

        private static byte[] GetAssembly(string path) {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stm = assembly.GetManifestResourceStream(path)) {
                var ba = new byte[(int) stm.Length];
                stm.Read(ba, 0, (int) stm.Length);
                return ba;
            }
        }
    }
}