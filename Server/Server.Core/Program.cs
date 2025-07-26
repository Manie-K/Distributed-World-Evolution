using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("[DEBUG]: Debug console for Core project, independent from UI project.");
        }
    }

    //@FranciszekGwarek
    class Lobby
    {
        public int LobbyId { get; private set; }
        private List<TcpClient> clients = new List<TcpClient>();
        private object clientLock = new object();
        private bool running = true;

        public Lobby(int id)
        {
            LobbyId = id;
        }

        public void AddClient(TcpClient client)
        {
            lock (clientLock)
            {
                clients.Add(client);
            }

            NetworkStream stream = client.GetStream();
            string msg = $"You have joined lobby {LobbyId}.\n";
            byte[] response = Encoding.UTF8.GetBytes(msg);
            stream.Write(response, 0, response.Length);
        }

        public void Run()
        {
            Console.WriteLine($"Lobby {LobbyId} running.");

            while (running)
            {
                lock (clientLock)
                {
                    foreach (var client in clients.ToArray())
                    {
                        if (client.Available > 0)
                        {
                            //Chat communication between users
                            NetworkStream stream = client.GetStream();
                            byte[] buffer = new byte[1024];
                            int count = stream.Read(buffer, 0, buffer.Length);
                            string msg = Encoding.UTF8.GetString(buffer, 0, count);
                            Console.WriteLine($"[Lobby {LobbyId}] Message: {msg.Trim()}");

                            foreach (var c in clients)
                            {
                                if (c != client)
                                {
                                    NetworkStream s = c.GetStream();
                                    byte[] data = Encoding.UTF8.GetBytes($"User: {msg}");
                                    s.Write(data, 0, data.Length);
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }

            Console.WriteLine($"Lobby {LobbyId} closed.");
        }
    }


    class Main_Server
    {
        static int lobbyCounter = 1;
        static Dictionary<int, Lobby> lobbies = new Dictionary<int, Lobby>();
        static object locker = new object();

        //Why static?
        static void Start(string[] args)
        {
            Console.WriteLine("Main server working...");
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }

        //Why static?
        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount).Trim();

            //Creating new lobby
            if (message.StartsWith("CREATE"))
            {
                int lobbyId;
                Lobby newLobby;

                lock (locker)
                {
                    lobbyId = lobbyCounter++;
                    newLobby = new Lobby(lobbyId);
                    lobbies[lobbyId] = newLobby;
                    new Thread(() => newLobby.Run()).Start();
                }

                newLobby.AddClient(client);
                Console.WriteLine($"Lobby {lobbyId} created and client added.");
            }
            //Joining created lobby
            else if (message.StartsWith("JOIN"))
            {
                string[] parts = message.Split(' ');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int joinId))
                {
                    lock (locker)
                    {
                        if (lobbies.ContainsKey(joinId))
                        {
                            lobbies[joinId].AddClient(client);
                            Console.WriteLine($"Client joined lobby {joinId}.");
                            return;
                        }
                    }

                    byte[] error = Encoding.UTF8.GetBytes("Lobby not found.\n");
                    stream.Write(error, 0, error.Length);
                    client.Close();
                }
            }
            else
            {
                byte[] error = Encoding.UTF8.GetBytes("Invalid command. Use CREATE or JOIN <id>.\n");
                stream.Write(error, 0, error.Length);
                client.Close();
            }
        }
    }
}