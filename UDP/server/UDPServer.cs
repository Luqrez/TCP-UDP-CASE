using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPListener
{
    private const int listenPort = 11000;

    private static void StartListener()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                string receivedMessage = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                Console.WriteLine($"Received broadcast from {groupEP} : {receivedMessage}");

                // Prepare response
                string responseMessage = "Message received: " + receivedMessage;
                byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);

                // Send response back to the sender
                listener.Send(responseBytes, responseBytes.Length, groupEP);
                Console.WriteLine($"Response sent to {groupEP}");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public static void Main()
    {
        StartListener();
    }
}
