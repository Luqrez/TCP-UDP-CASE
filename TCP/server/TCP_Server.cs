using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic; // Add this at the top

class TcpChatServer
{
    static List<TcpClient> clients = new List<TcpClient>(); // Shared list of clients
    static object clientsLock = new object(); // For thread safety

    static void Main()
    {
        TcpListener server = null;
        try
        {
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddr, port);
            server.Start();

            Console.WriteLine("Serveren er klar på 127.0.0.1:{0}", port);

            while (true)
            {
                Console.WriteLine("Venter på en forbindelse...");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Forbundet!");

                lock (clientsLock)
                {
                    clients.Add(client); // Add new client to the list
                }

                Thread t = new Thread(HandleClient);
                t.Start(client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server.Stop();
        }

        Console.WriteLine("\nTryk Enter for at afslutte...");
        Console.Read();
    }

    static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        byte[] bytes = new byte[256];
        int i;

        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Modtaget: {0}", data);

                // Broadcast to all clients except the sender
                byte[] msg = Encoding.ASCII.GetBytes(data);
                lock (clientsLock)
                {
                    foreach (var c in clients)
                    {
                        if (c != client && c.Connected)
                        {
                            try
                            {
                                NetworkStream s = c.GetStream();
                                s.Write(msg, 0, msg.Length);
                            }
                            catch { /* Ignore errors for disconnected clients */ }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Fejl i klienttråd: " + e.Message);
        }
        finally
        {
            lock (clientsLock)
            {
                clients.Remove(client); // Remove client on disconnect
            }
            client.Close();
        }
    }
}
