using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace iTapServer
{
    class Server
    {
        static TcpListener _listener;
        public static Trie Trie;

        static void Main(string[] args)
        {
            try
            {

                Trie = new Trie();
                TextReader reader = new StreamReader(File.OpenRead(args[0]));
                int lines = int.Parse(reader.ReadLine());
                for (int i = 0; i < lines; i++)
                {
                    Trie.AddValue(reader.ReadLine());
                }

                int port = int.Parse(args[1]);
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();

                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    ConnectClient connectClient = new ConnectClient(client);

                    Thread clientThread = new Thread(new ThreadStart(connectClient.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
