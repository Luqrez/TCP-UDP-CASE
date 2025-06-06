using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        UdpClient client = new UdpClient();
        client.EnableBroadcast = true;

        IPAddress broadcast = IPAddress.Parse("127.0.0.1");
        IPEndPoint ep = new IPEndPoint(broadcast, 11000);


        string message;
        if (args.Length == 0)
        {
            Console.Write("Enter a message to send: ");
            message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("No message entered. Exiting.");
                return;
            }
        }
        else
        {
            message = args[0];
        }

        byte[] sendbuf = Encoding.ASCII.GetBytes(message);
        client.Send(sendbuf, sendbuf.Length, ep);

        Console.WriteLine("Message sent to the broadcast address");

        // Wait for response
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = client.Receive(ref remoteEP);
        Console.WriteLine($"Received response from server: {Encoding.ASCII.GetString(response)}");

        client.Close();
    }
}
