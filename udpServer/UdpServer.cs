using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpEchoServer2
{
    public class UdpServer
    {
        UdpClient client;

        public UdpServer(UdpClient client)
        {
            this.client = client;
        }

        public async Task ConversationAsync()
        {
            try
            {

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8080);


                UdpReceiveResult udpReceiveResult = await client.ReceiveAsync();

                string incoming = Encoding.UTF8.GetString(udpReceiveResult.Buffer);

                while (incoming != ".")
                {
                    Console.WriteLine("Message recieved: " + incoming);

                    client.Send(Encoding.UTF8.GetBytes(incoming), incoming.Length);
                    Console.WriteLine("Message Sent back: " + incoming);

                    udpReceiveResult = await client.ReceiveAsync();
                    incoming = Encoding.UTF8.GetString(udpReceiveResult.Buffer);
                }

                Console.WriteLine("Client sent '.': closing connection.");
                client.Close();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e + " " + e.StackTrace);
            }
            finally
            {
                if (client != null) client.Close();
            }
        }
    }
}
