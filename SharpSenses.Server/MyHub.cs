using Microsoft.AspNet.SignalR;

namespace SharpSenses.Server {
    public class MyHub : Hub {
        public void Send(string name, string message) {
            Clients.All.addMessage(name, message);
        }
    }
}