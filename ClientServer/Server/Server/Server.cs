using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Server
    {
        private string Host;
        private int Port;

        private Socket socket;
        private TcpListener listener;
        private int MAX_CHAR = 1000;

        private string clientRequest;

        public Server(string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
            clientRequest = "";
        }

        public void RunListener()
        {
            IPAddress IP = IPAddress.Parse(Host);
            listener = new TcpListener(IP, Port);

            listener.Start();

            socket = listener.AcceptSocket();
        }

        public void ReadRequest()
        {
            byte[] RequestStream = new byte[MAX_CHAR];
            int n = socket.Receive(RequestStream);

            for (int i = 0; i < n; i++)
            {
                clientRequest += Convert.ToChar(RequestStream[i]);
            }
            Console.WriteLine("Path to be processed: " + clientRequest);
        }

        public void SendResponse()
        {
            string serverResponse = SolveRequest(clientRequest);
            socket.Send(Encoding.ASCII.GetBytes(serverResponse));

            Console.WriteLine("Done!");
        }

        public void CloseConnection()
        {
            socket.Close();
            listener.Stop();
        }

        private string SolveRequest(string path)
        {
            string response = "";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string readText = File.ReadAllText(path);
                    response += readText;
                }
            }
            else if (Directory.Exists(path))
            {
                string[] fileEntries = Directory.GetFiles(path);
                foreach (string fileName in fileEntries)
                    response += '\n' + fileName;

                string[] dirEntries = Directory.GetDirectories(path);
                foreach (string dirName in dirEntries)
                    response += '\n' + dirName;
            }
            else
            {
                response = path + " is not a valid file or directory.";
            }
            return response;
        }

        static void Main(string[] args)
        {
            while(true)
            {
                Server server = new Server("127.0.0.1", 8001);
                server.RunListener();
                server.ReadRequest();
                server.SendResponse();
                server.CloseConnection();
            }
            
        }

    }
}
