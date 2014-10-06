using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSenses.RealSense.Playground {
    class Program {
        static void Main(string[] args) {
            var cam = new Camera();
            cam.Start();

            Console.ReadLine();
        }
    }
}
