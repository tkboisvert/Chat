using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Task listeningTask = new Task(Listen);

            listeningTask.Start();

            IPAddress[] IPs = Dns.GetHostAddresses("vivian");

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to vivain. . . ");
            s.Connect(IPs[0], 5432);
            Console.WriteLine("Connection established");
            
            Console.WriteLine("Enter your message to be sent");
            while (true)
            {
                string messageToBeSent = Console.ReadLine();
                s.Send(Encoding.ASCII.GetBytes(messageToBeSent + "\n"));
                if (messageToBeSent == "exit")
                {
                    break;
                }
            }


        }
        private static void Listen()
        {
            Console.WriteLine("Waiting for a connection. . .");

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress hostIP = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hostIP, 12345);
            listenSocket.Bind(ep);
            listenSocket.Listen(100);
            Socket handler = listenSocket.Accept();
            string data = "";

            while(true)
            {
                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf('\n') > -1)
                {
                    Console.Write("Text recieved : {0}", data);
                    bytes = new byte[1024];
                    data = string.Empty;
                }
            }
        }
    }
}
