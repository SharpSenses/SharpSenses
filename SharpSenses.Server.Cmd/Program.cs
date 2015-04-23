using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace SharpSenses.Server.Cmd {
    class Program {
        static void Main(string[] args) {
            string url = "http://*:8012";
            using (WebApp.Start<Startup>(url)) {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}
