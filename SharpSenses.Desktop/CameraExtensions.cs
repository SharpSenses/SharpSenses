using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses {
    public static class CameraExtensions {

        public static ICamera Create(CameraKind cameraKind) {
            return TryAssembly(cameraKind);
        }
    }
}
