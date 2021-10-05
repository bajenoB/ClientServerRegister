using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using ClientProject;
using System.IO;
using System.Linq;

namespace ServerProject
{
    class Server
    {
        public int Client_ID;
        private string ipAddr;
        private int port;
        private IPEndPoint ipPoint;
        public Socket socket;
        public Socket socketclient;
        public List<Client> clients;


        public Server()
        {
            this.Client_ID = -1;
            this.ipAddr = "127.0.0.1";
            this.port = 8000;
            this.ipPoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.clients = new List<Client>();

        }
        public void StartServer()
        {
            try
            {

                this.socket.Bind(ipPoint);
                this.socket.Listen(10);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
          

        }
        public void ConnectOne()
        {
            bool check = true;
            while (check)
            {
                this.socketclient = this.socket.Accept();
                clients.Add(new Client(socketclient));

                if (clients.Count > 0)
                {
                    check = false;
                }
            }

        }
        public void Connects()
        {
            while (true)
            {

                this.socketclient = this.socket.Accept();
                clients.Add(new Client(socketclient));
                this.Client_ID++;
                clients[clients.Count - 1].ID = this.Client_ID;

            }

        }
        public StringBuilder GetMsg()
        {

            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[256];
            foreach (var item in clients)
            {
                do
                {

                    bytes = item.socket.Receive(data);

                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (item.socket.Available > 0);
            }

            return builder;
        }
        public void SendMsg(string message)
        {
            byte[] data = new byte[256];
            foreach (var item in clients)
            {
                if (File.Exists(message) && Path.GetFileName(message).Contains(".txt") || Path.GetFileName(message).Contains(".rtf"))
                {
                    item.socket.Send(Encoding.Unicode.GetBytes(File.ReadAllText(message)));
                }
                else
                {
                    item.socket.Send(Encoding.Unicode.GetBytes(message));
                }
            }

        }
        public void SendMsg(string message, int user)
        {
            byte[] data = new byte[256];
            if (File.Exists(message) && Path.GetFileName(message).Contains(".txt") || Path.GetFileName(message).Contains(".rtf"))
            {
                clients[user].socket.Send(Encoding.Unicode.GetBytes(File.ReadAllText(message)));
            }
            else
            {
                clients[user].socket.Send(Encoding.Unicode.GetBytes(message));
            }

        }
        public void SendCommand(int choice)
        {

            int server_choice = 0;
            int user_choice = 0;
            bool check = false;
            Exception exception = new Exception();
            do
            {
                lock (clients)
                {
                    foreach (var item in clients)
                    {
                        Console.WriteLine($"<ID: {item.ID}> " + $"Connected: {item.socket.Connected}");
                    }
                }

                Console.WriteLine("Choice ID");
                try
                {
                    user_choice = int.Parse(Console.ReadLine());
                    check = true;
                }
                catch (Exception)
                {
                    
                    check = false;
                    Console.Clear();
                }
            } while (!check);

            switch (choice)
            {
                case 1:
                    {
                        lock (clients)
                        {

                            for (int i = 0; i < clients.Count; i++)
                            {
                                if (clients[i].ID == user_choice)
                                {
                                    Console.WriteLine("Choice action\nRead register: 1\nCreate key: 2\nChange console size: 3");
                                    try
                                    {
                                        server_choice = int.Parse(Console.ReadLine());
                                        SendBrowser(server_choice, i);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("You can entry only numbs");
                                        i = 0;
                                    }

                                }
                               
                            }
                        }
                        break;
                    }
                case 2:
                    {

                        lock (clients)
                        {
                            for (int i = 0; i < clients.Count; i++)
                            {

                                if (clients[i].ID == user_choice)
                                {
                                    SendMsg("Exit", i);
                                    clients[i].socket.Disconnect(false);

                                    clients.RemoveAt(i);
                                }
                                
                            }

                        }

                        break;
                    }
                default:
                    break;
            }
        }
       
        public void SendBrowser(int choice, int user)
        {
            int x, y;
            x = int.Parse(Console.ReadLine());
            y = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    {
                        SendMsg("read register", user);
                        break;
                    }
                case 2:
                    {
                        SendMsg("create key", user);
                        break;
                    }
                case 3:
                    {
                        SendMsg("change console size", user);

                        break;
                    }
                
            }
        }

    }
}
