using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;
using System.Linq;

namespace ClientProject
{
    public class Client
    {
        public int ID;
        public string ipAddr { get; set; }
        public int port;
        public IPEndPoint iPEndPoint;
        public Socket socket;
       

        public Client()
        {
            this.ID++;
            this.ipAddr = "";
            this.port = 8000;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            

        }

        public Client(Socket socket)
        {
          
            this.socket = socket;
            
        }

        public void CreateIpEndPoint()
        {
            this.iPEndPoint = new IPEndPoint(IPAddress.Parse(this.ipAddr), port);

        }

        public void Connect()
        {
            socket.Connect(iPEndPoint);
        }
        public void SendMsg(string sms)
        {
            byte[] data = new byte[256];
            data = Encoding.Unicode.GetBytes(sms);
            socket.Send(data);
        }
        public StringBuilder GetMsg()
        {
            int bytes = 0;
            byte[] data = new byte[256];
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                bytes = socket.Receive(data);
                stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (socket.Available > 0);
            if(stringBuilder.ToString().ToLower() == "exit")
            {
                Environment.Exit(0);
            }
            return stringBuilder;
        }
        public void GetServerCommand(StringBuilder command)
        {
            RegistryKey key = Registry.CurrentUser;
            int x, y;
            Console.WriteLine("1");

                if (command.ToString().ToLower().Contains("read register"))
                {
                
                    foreach (var item in key.GetSubKeyNames())
                    {
                        Console.WriteLine(item);
                        SendMsg(item);
                    }
                    key.Close();
                }
                if (command.ToString().ToLower().Contains("create key"))
                {
                Console.WriteLine("1");
                RegistryKey newKey = key.CreateSubKey("ZohaKEY");
                    newKey.SetValue("age", "19");
                    newKey.Close();

                }
                if(command.ToString().ToLower().Contains("change console"))
                {
                Console.WriteLine("1");
                RegistryKey keyconsole = key.OpenSubKey("ClientData");
                    keyconsole.GetValue("Width");
                    keyconsole.GetValue("Height");
                    x = int.Parse(Console.ReadLine());
                    y = int.Parse(Console.ReadLine());
                    keyconsole.SetValue("Width", x);
                    keyconsole.SetValue("Height", y);
                    Console.SetWindowSize(int.Parse(keyconsole.GetValue("Width").ToString()), int.Parse(keyconsole.GetValue("Height").ToString()));

            }

                
            
        }

    }
}
