using System.Net.Sockets;
using SharedLibrary.Messages;
using SharedLibrary;
using SharedLibrary.Messages.BehaviourMessages;

class Client
{
    static void Main()
    {
        string serverIp = "127.0.0.1";
        int port = 8080;

        try
        {
            TcpClient client = new TcpClient(serverIp, port);
            Thread.Sleep(3000);
            _ = MessageManager.SendMessageAsync(client, new RoleMessage(RoleEnum.User));

            //Thread receiving messages
            Thread receiveThread = new Thread(async () =>
            {
                MessageBase message = await MessageManager.ReceiveMessageAsync(client);
                InfoMessage stringMessage = (InfoMessage)message;
                Console.WriteLine($"[Client] Received message: {stringMessage.MessageContent}");
                
                _ = MessageManager.SendMessageAsync(client, new GetMessage(GetMessageTypeEnum.BehaviourList));
                message = await MessageManager.ReceiveMessageAsync(client);
                BehaviourListMessage moduleListMessage = (BehaviourListMessage)message;
            });
            receiveThread.Start();

            Thread.Sleep(5000);

            //Sending messages
            while (true)
            {
                //Console.Write("Write message or type 'exit' to end: ");
                //string message = Console.ReadLine();
                //if (message == "exit") break;

                //MessageManager.SendMessageAsync(client, new GetMessage(GetMessageTypeEnum.ModuleList));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Client] error: {e.Message}");
        }
    }
}
