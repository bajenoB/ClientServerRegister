using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;
using System.Linq;

namespace ClientProject
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            RegistryKey key = Registry.CurrentUser;
            if (key.GetSubKeyNames().Contains("ClientData"))
            {
                RegistryKey CuurrentKey = key.OpenSubKey("ClientData");
                
               
                CuurrentKey.GetValue("Width");
                CuurrentKey.GetValue("Height");
                Console.SetWindowSize(int.Parse(CuurrentKey.GetValue("Width").ToString()), int.Parse(CuurrentKey.GetValue("Height").ToString()));
            }
            else
            {
                RegistryKey newKey = key.CreateSubKey("ClientData");
                newKey.SetValue("Width", Console.WindowWidth);
                newKey.SetValue("Height", Console.WindowHeight);
                newKey.Close();
                
            }

            if (key.GetSubKeyNames().Contains("IpData"))
            {
                RegistryKey CuurrentKey = key.OpenSubKey("IpData");
             


                client.ipAddr=CuurrentKey.GetValue("Ip").ToString();
               
                
            }
            else
            {
                RegistryKey newKey = key.CreateSubKey("IpData");
                newKey.SetValue("Ip", "127.0.0.1");
                client.ipAddr = key.GetValue("Ip").ToString();
                newKey.Close();
            }


            client.CreateIpEndPoint();
            client.Connect();
            while (true)
            {
        
                client.GetServerCommand(client.GetMsg());
                
                
            }
          
        }
    }
}
