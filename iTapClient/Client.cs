using System;
using System.Net.Sockets;
using System.Text;

namespace iTapClient
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                string[] host = args[0].Split(':');
                string address = host[0];
                int port = int.Parse(host.Length > 1 ? host[1] : args[1]);

                TcpClient client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    string word = Console.ReadLine();
                    if (!string.IsNullOrEmpty(word))
                    {
                        byte[] data = Encoding.ASCII.GetBytes(word);
                        stream.Write(data, 0, data.Length);

                        data = new byte[64];
                        StringBuilder stringBuilder = new StringBuilder();
                        do
                        {
                            var bytes = stream.Read(data, 0, data.Length);
                            stringBuilder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                        } while (stream.DataAvailable);
                        word = stringBuilder.ToString();
                    }
                    Console.WriteLine(word);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
