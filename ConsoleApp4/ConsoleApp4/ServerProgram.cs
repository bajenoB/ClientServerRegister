using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClientProject;
namespace ServerProject
{
    class ServerProgram
    {

        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();
            
            try
            {
                Task.Factory.StartNew(() => server.Connects());
               
                while (true)
                {
                    Console.WriteLine("-Start Browser: 1\n-Disconnect from server: 2");
                    server.SendCommand(int.Parse(Console.ReadLine()));
                    Console.Clear();
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}
