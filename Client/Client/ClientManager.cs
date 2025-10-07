using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public static class ActionStatus
    {
        public const int WAITING = 0;
        public const int FAILED = 1;
        public const int SUCCESS = 2;
    }

    public class ClientManager
    {
        public const double CLIENT_UPDATES_PER_SECOND = 64;
        public TcpClient Client { get; private set; }

        private string serverIp;
        private int port;
        private Thread receiveThread;

        #region Game variables

        private int lobbyID;
        public int LobbyID
        {
            get => Interlocked.CompareExchange(ref lobbyID, 0, 0);
            set => Interlocked.Exchange(ref lobbyID, value);
        }

        private int lobbyCreated;
        public int LobbyCreated
        {
            get => Interlocked.CompareExchange(ref lobbyCreated, 0, 0);
            set => Interlocked.Exchange(ref lobbyCreated, value);
        }

        private int lobbyJoined;
        public int LobbyJoined
        {
            get => Interlocked.CompareExchange(ref lobbyJoined, 0, 0);
            set => Interlocked.Exchange(ref lobbyJoined, value);
        }

        private int lobbyListReady;
        public int LobbyListReady
        {
            get => Interlocked.CompareExchange(ref lobbyListReady, 0, 0);
            set => Interlocked.Exchange(ref lobbyListReady, value);
        }

        private readonly object entitiesLock = new object();
        private Dictionary<Guid, WorldEntityDTO> entities = new Dictionary<Guid, WorldEntityDTO>();
        public IReadOnlyDictionary<Guid, WorldEntityDTO> Entities
        {
            get
            {
                lock (entitiesLock)
                {
                    return entities;
                }
            }
        }

        private readonly object lobbiesLock = new object();
        private List<LobbyDTO> lobbies = new List<LobbyDTO>();
        public IReadOnlyList<LobbyDTO> Lobbies
        {
            get
            {
                lock (lobbiesLock)
                {
                    return lobbies;
                }
            }
        }

        #endregion

        public ClientManager()
        {
            lobbyCreated = ActionStatus.WAITING;
            lobbyJoined = ActionStatus.WAITING;
            lobbyListReady = ActionStatus.WAITING;
            lobbyID = -1;
            serverIp = "127.0.0.1";
            port = 5000;
            StartClient();
        }

        private void StartClient()
        {
            try
            {
                Client = new TcpClient(serverIp, port);
                _ = MessageManager.SendMessageAsync(Client, new RoleMessage(RoleEnum.User));
                receiveThread = new Thread(async () => await ReceiveMessagesAsync())
                {
                    IsBackground = true
                };
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Client] error: {e.Message}");
            }
        }

        public void CloseClient()
        {
            Client?.Close();
            receiveThread?.Join();
        }

        private async Task ReceiveMessagesAsync()
        {
            while (Client.Connected)
            {
                MessageBase message = await MessageManager.ReceiveMessageAsync(Client);

                if (message == null)
                {
                    Console.WriteLine("Received null message");
                }
                else if (message.MessageType == MessageTypeEnum.InfoMessage)
                {
                    InfoMessage infoMessage = (InfoMessage)message;

                    switch (infoMessage.InfoMessageType)
                    {
                        case InfoMessageTypeEnum.LobbyCreated:
                            LobbyCreated = ActionStatus.SUCCESS;
                            break;
                        case InfoMessageTypeEnum.LobbyNotCreated:
                            LobbyCreated = ActionStatus.FAILED;
                            break;
                        case InfoMessageTypeEnum.LobbyJoined:
                            LobbyJoined = ActionStatus.SUCCESS;
                            break;
                        case InfoMessageTypeEnum.LobbyNotJoined:
                            LobbyJoined = ActionStatus.FAILED;
                            break;

                        default:
                            break;
                    }

                    Console.WriteLine($"[Client] Received message: {infoMessage.MessageContent}");
                }
                else if (message.MessageType == MessageTypeEnum.LobbyData)
                {
                    LobbyDataMessage lobbyMessage = (LobbyDataMessage)message;
                    LobbyJoined = ActionStatus.SUCCESS;
                    LobbyID = lobbyMessage.LobbyID;

                    Console.WriteLine("Received lobby data");
                }
                else if (message.MessageType == MessageTypeEnum.LobbyList)
                {
                    LobbyListMessage lobbyListMessage = (LobbyListMessage)message;
                    LobbyListReady = ActionStatus.SUCCESS;
                    lock (lobbiesLock)
                    {
                        lobbies = lobbyListMessage.Lobbies.ToList();
                    }

                    Console.WriteLine("Received lobby list");
                }
                else if (message.MessageType == MessageTypeEnum.WorldState)
                {
                    WorldStateMessage worldStateMessage = (WorldStateMessage)message;
                    lock (entitiesLock)
                    {
                        entities = worldStateMessage.Entities.ToDictionary(e => e.Id);
                    }
                }
                else
                {
                    Console.WriteLine("Received unknown message");
                }
            }
        }
    }
}
