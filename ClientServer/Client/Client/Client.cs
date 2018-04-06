using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private string Host;
        private int Port;

        private TcpClient client;
        private int MAX_CHAR = 1000;

        private Stream stream;

        public Client(string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
        }

        public void ConnectToServer()
        {
            client = new TcpClient();
            client.Connect(Host, Port);
        }

        public void SendRequest()
        {
            Console.Write("Enter a valid path: ");
            string Message = Console.ReadLine();

            stream = client.GetStream();

            ASCIIEncoding RequestEncoding = new ASCIIEncoding();
            byte[] BytesRequest = RequestEncoding.GetBytes(Message);

            stream.Write(BytesRequest, 0, BytesRequest.Length);

            }

        public void ReadResponse()
        {
            byte[] BytesResponse = new byte[MAX_CHAR];
            int n = stream.Read(BytesResponse, 0, MAX_CHAR);

            string serverResponse = "";
            for (int i = 0; i < n; i++)
                serverResponse += Convert.ToChar(BytesResponse[i]);

            Console.WriteLine("Response: " + serverResponse);

        }

        public void CloseConnection()
        {
            client.Close();
        }

        static void Main(string[] args)
        {
            while(true)
            {
                Client client = new Client("127.0.0.1", 8001);
                client.ConnectToServer();
                client.SendRequest();
                client.ReadResponse();
                client.CloseConnection();
            }
        }
    }
}
