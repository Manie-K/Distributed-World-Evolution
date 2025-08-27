using System.Net.Sockets;
using SharedLibrary.Messages;
using SharedLibrary;

class Client
{
    static void Main()
    {
        string serverIp = "127.0.0.1";
        int port = 5000;

        try
        {
            TcpClient client = new TcpClient(serverIp, port);
            Thread.Sleep(3000);
            _ = MessageManager.SendMessageAsync(client, new RoleMessage(RoleEnum.User));

            //Thread receiving messages
            Thread receiveThread = new Thread(() =>
            {
                MessageBase message = MessageManager.ReceiveMessage(client);
                InfoMessage stringMessage = (InfoMessage)message;
                Console.WriteLine($"[Client] Received message: {stringMessage.MessageContent}");
            });
            receiveThread.Start();

            Thread.Sleep(5000);

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
