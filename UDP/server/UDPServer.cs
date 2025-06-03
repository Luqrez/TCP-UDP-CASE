// Server-Side Implementation Of UDP:
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;

class StudentUDPServer
{
    public static void Main()
    {
        UdpClient udpc = new UdpClient(7878);
        Console.WriteLine("Server Started, servicing on port no. 7878");
        IPEndPoint ep = null;
        while (true){
          
          // Store received data from client
            byte[] receivedData = udpc.Receive(ref ep);
          
            string studentName = Encoding.ASCII.GetString(receivedData);
          
            string msg = ConfigurationSettings.AppSettings[studentName];
            if (msg == null) msg = "No such Student available for conversation";
            byte[] sdata = Encoding.ASCII.GetBytes(msg);
            udpc.Send(sdata, sdata.Length, ep);
        }
    }
}