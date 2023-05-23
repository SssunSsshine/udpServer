using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdpEchoServer2;

namespace udpServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            int recv;
            byte[] data = new byte[1024];
            //Сетевая конечная точка в виде адреса и номера порта
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8080);
            //Создание нового сокета (схема адресации, тип сокета, протокол)
            //параметр AddressFamily задает схему адресации. В нашем случае это адреса IPv4
            //параметр SocketType указывает, какой тип сокета применяется. 
            //Datagram - поддерживает двусторонний поток данных. Не гарантируется, что этот поток будет последовательным, 
            //надежным, и что данные не будут дублироваться. Важной характеристикой данного сокета является то,
            //что границы записи данных предопределены.
            //последний параметр, ProtocolType, задает тип протокола
            Socket SrvSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            SrvSock.Bind(ipep);
            Console.WriteLine("Waiting client connection...");
            //создаем конечную точку по адресу сокета. Т.е. будем "слушать" порты 
            //и контролировать все сетевые интерфейсы
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            //Получаем конечную удаленную точку
            EndPoint Remote = (EndPoint)(sender);

            Encoding encoding = Encoding.GetEncoding("windows-1251");

            //Получаем сообщение от клиента ("Client connected sucsessfully!")
            recv = SrvSock.ReceiveFrom(data, ref Remote);

            Console.Write("Message recieved from {0}:", Remote.ToString());
            Console.WriteLine(encoding.GetString(data, 0, recv));

            //Отправляем клиенту сообщение об успешном подключении
            string welcome = "Connection is successfull!";
            data = encoding.GetBytes(welcome);
            SrvSock.SendTo(data, data.Length, SocketFlags.None, Remote);

            while (true)
            {
                data = new byte[1024];
                recv = SrvSock.ReceiveFrom(data, ref Remote);
                string str = encoding.GetString(data, 0, recv);
                
                Console.WriteLine("Recieved data: from {0}:" + str, Remote.ToString());

                SrvSock.SendTo(data, recv, SocketFlags.None, Remote);
            }
        }
    }
}