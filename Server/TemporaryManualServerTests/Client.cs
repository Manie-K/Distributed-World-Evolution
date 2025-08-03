using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Server.Shared.Logging;
using Server.Shared.Messages;

class Client
{
    static void Main()
    {
        string serverIp = "127.0.0.1";
        int port = 5000;

        try
        {
            //Thread.Sleep(6000);
            TcpClient client = new TcpClient(serverIp, port);

            //Thread receiving messages
            Thread receiveThread = new Thread(() =>
            {
                IMessage message = MessageManager.ReceiveMessage(client);
                DefaultMessage stringMessage = (DefaultMessage)message;
                Console.WriteLine($"[Client] Received message: {stringMessage.MessageContent}");
            });
            receiveThread.Start();

            //Sending messages
            while (true)
            {
                //Console.Write("Write message or type 'exit' to end: ");
                //string message = Console.ReadLine();
                //if (message == "exit") break;

                //MessageManager.SendMessage(client, new StringMessage(message));
            }

            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Client] error: {e.Message}");
        }
    }
}
