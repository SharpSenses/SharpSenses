using System;

namespace SharpSenses.RealSense {
    public static class DisposableExtensions {
        public static void SilentlyDispose(this IDisposable disposable) {
            try {
                disposable.Dispose();
            }
            catch { }
        }
    }
}