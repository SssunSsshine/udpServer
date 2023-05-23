using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            UdpClient server = null;
            try
            {
                byte[] data = new byte[1024];
                string input, stringData; ;

                server = new UdpClient("127.0.0.1", 8080);
                //создаем конечную точку по адресу сокета. Т.е. будем "слушать" порты 
                //и контролировать все сетевые интерфейсы
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 8080);

                //Отправляем серверу сообщение о подключении
                string welcome = "Client connected sucsessfully!";
                Encoding encoding = Encoding.GetEncoding("windows-1251");

                data = encoding.GetBytes(welcome);
                server.Send(data, data.Length);
                data = server.Receive(ref sender);

                Console.Write("Message recieved from {0}:", sender.ToString());
                stringData = encoding.GetString(data, 0, data.Length);
                Console.WriteLine(stringData);
                while (true)
                {
                    data = new byte[1024];

                    Console.WriteLine();
                    input = Console.ReadLine();

                    data = encoding.GetBytes(input);
                    server.Send(data, data.Length);

                    data = server.Receive(ref sender);
                    stringData = encoding.GetString(data, 0, data.Length);
                    Console.Write("<");
                    Console.WriteLine(stringData);
                }
            }
            catch (Exception ex) { }
            finally
            {
                if(server !=null)
                    server.Close();
            }
            
            
        }
    }
}