using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

class Tcpchatclient{

  private static string clientName;

  static void Main(String[] args)
  {
    Console.WriteLine("Enter your name:");
    clientName = Console.ReadLine() ?? "test, Server!";
    Console.WriteLine($"Welcome to the TCP Chat Client! {clientName}");

    while (true)
        {
            Console.Write($"[{clientName}] Enter message (or 'exit' to quit): ");
            string userInput = Console.ReadLine() ?? "";
            
            if (userInput.ToLower() == "exit")
                break;

            var fullMessage = $"[{clientName}]: {userInput}";
            Connect("127.0.0.1", fullMessage);
        }
  }


static void Connect(String server, String message)
{
  try
  {
    // Create a TcpClient.
    // Note, for this client to work you need to have a TcpServer
    // connected to the same address as specified by the server, port
    // combination.
    Int32 port = 13000;

    // Prefer a using declaration to ensure the instance is Disposed later.
    using TcpClient client = new TcpClient(server, port);

    // Translate the passed message into ASCII and store it as a Byte array.
    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

    // Get a client stream for reading and writing.
    NetworkStream stream = client.GetStream();

    // Send the message to the connected TcpServer.
    stream.Write(data, 0, data.Length);

    Console.WriteLine("Sent: {0}", message);

    // Receive the server response.

    // Buffer to store the response bytes.
    data = new Byte[256];

    // String to store the response ASCII representation.
    String responseData = String.Empty;

    // Read the first batch of the TcpServer response bytes.
    Int32 bytes = stream.Read(data, 0, data.Length);
    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
    Console.WriteLine("Received: {0}", responseData);

    // Explicit close is not necessary since TcpClient.Dispose() will be
    // called automatically.
    // stream.Close();
    // client.Close();
  }
  catch (ArgumentNullException e)
  {
    Console.WriteLine("ArgumentNullException: {0}", e);
  }
  catch (SocketException e)
  {
    Console.WriteLine("SocketException: {0}", e);
  }

 
}
}