using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace iTapServer
{
    class ConnectClient
    {
        public TcpClient Client;
        public ConnectClient(TcpClient tcpClient)
        {
            Client = tcpClient;
        }

        private string Search(string message)
        {
            Trie trie = Server.Trie;
            var words = trie.FindValues(message);
            message = words != null ? words.Aggregate((x, word) => x + (word + "\n")) : "\n";
            return message;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = Client.GetStream();
                byte[] data = new byte[64];
                while (true)
                {

                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    string request = builder.ToString();

                    int get = request.IndexOf("get ", StringComparison.Ordinal);
                    string response = " ";
                    if (get >= 0)
                        response = Search(request.Substring(4));
                    data = Encoding.ASCII.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                stream?.Close();
                Client?.Close();
            }
        }

    }
}
