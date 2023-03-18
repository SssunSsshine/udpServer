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
            byte[] data = new byte[1024];
            string input, stringData; ;

            UdpClient server = new UdpClient("127.0.0.1", 8080);
            //создаем конечную точку по адресу сокета. Т.е. будем "слушать" порты 
            //и контролировать все сетевые интерфейсы
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 8080);

            //Отправляем серверу сообщение о подключении
            string welcome = "Client connected sucsessfully!";
            Encoding encoding = Encoding.GetEncoding("windows-1251");

            data = encoding.GetBytes(welcome);
            server.Send(data, data.Length);
            data = server.Receive(ref sender);

            //Обеспечиваем их отображение на экране
            Console.Write("Message recieved from {0}:", sender.ToString());
            stringData = encoding.GetString(data, 0, data.Length);
            Console.WriteLine(stringData);

            //Бесконечный цикл.
            while (true)
            {
                //Объявляем новый массив под пришедшие данные.
                data = new byte[1024];
                //Ожидаем ввода с клавиатуры (строки) и заносим в переменную input
                Console.Write("\r\n>");
                input = Console.ReadLine();

                //Перевод отсылаемой строки в байты
                data = encoding.GetBytes(input);
                //Отсылаем серверу строку (переведенную в байты)
                server.Send(data, data.Length);

                //Если пришла команла exit - выходим из цикла (далее - закрываем сокет и т.д.)
                if (input == "exit") break;

                //Получение данных...
                data = server.Receive(ref sender);
                //Перевод принятых байтов в строку
                stringData = encoding.GetString(data, 0, data.Length);
                Console.Write("<");
                //Отображение на экране принятой строки (размер файла)
                Console.WriteLine(stringData);
            }
            
            server.Close();
        }
    }
}