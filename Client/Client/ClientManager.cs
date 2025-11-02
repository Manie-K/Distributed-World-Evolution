using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Messages;
using SharedLibrary.Messages.BehaviourMessages;
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
        public const int IDLE = 0;
        public const int PENDING = 1;
        public const int FAILED = 2;
        public const int SUCCESS = 3;
    }

    public class ClientManager
    {
        public const double CLIENT_UPDATES_PER_SECOND = 64;
        public TcpClient Client { get; private set; }

        private string serverIp;
        private int port;
        private Thread receiveThread;

        public static event Action OnErrorMessageReceived;

        #region Game variables

        private int lobbyCreated;
        public int LobbyCreated
        {
            get => Interlocked.CompareExchange(ref lobbyCreated, 0, 0);
            set => Interlocked.Exchange(ref lobbyCreated, value);
        }

        private int moduleCreated;
        public int ModuleCreated
        {
            get => Interlocked.CompareExchange(ref moduleCreated, 0, 0);
            set => Interlocked.Exchange(ref moduleCreated, value);
        }

        private int lobbyJoined;
        public int LobbyJoined
        {
            get => Interlocked.CompareExchange(ref lobbyJoined, 0, 0);
            set => Interlocked.Exchange(ref lobbyJoined, value);
        }
        public void SetPendingLobbyJoined()
        {
            Interlocked.CompareExchange(ref lobbyJoined, ActionStatus.PENDING, ActionStatus.IDLE);
        }

        private int lobbyListReady;
        public int LobbyListReady
        {
            get => Interlocked.CompareExchange(ref lobbyListReady, 0, 0);
            set => Interlocked.Exchange(ref lobbyListReady, value);
        }

        private int moduleListReady;
        public int ModuleListReady
        {
            get => Interlocked.CompareExchange(ref moduleListReady, 0, 0);
            set => Interlocked.Exchange(ref moduleListReady, value);
        }

        private int behaviourListReady;
        public int BehaviourListReady
        {
            get => Interlocked.CompareExchange(ref behaviourListReady, 0, 0);
            set => Interlocked.Exchange(ref behaviourListReady, value);
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

        private readonly object modulesLock = new object();
        private List<ModuleDTO> modules = new List<ModuleDTO>();
        public IReadOnlyList<ModuleDTO> Modules
        {
            get
            {
                lock (modulesLock)
                {
                    return modules;
                }
            }
        }

        private readonly object behavioursLock = new object();
        private List<BehaviourDTO> behaviours = new List<BehaviourDTO>();
        public IReadOnlyList<BehaviourDTO> Behaviours
        {
            get
            {
                lock (behavioursLock)
                {
                    return behaviours;
                }
            }
        }

        private readonly object lobbyDataLock = new object();
        private LobbyDTO lobbyData = new LobbyDTO(-1, "", 0, 0, 0, []);
        public LobbyDTO LobbyData
        {
            get
            {
                lock (lobbyDataLock)
                {
                    return lobbyData;
                }
            }
        }

        private readonly object playerGuidLock = new object();
        private Guid playerGuid = Guid.Empty;
        public Guid PlayerGuid
        {
            get
            {
                lock (playerGuidLock)
                {
                    return playerGuid;
                }
            }
        }

        #endregion

        public ClientManager()
        {
            lobbyCreated = ActionStatus.IDLE;
            moduleCreated = ActionStatus.IDLE;
            lobbyJoined = ActionStatus.IDLE;
            lobbyListReady = ActionStatus.IDLE;
            moduleListReady = ActionStatus.IDLE;
            behaviourListReady = ActionStatus.IDLE;
            serverIp = "127.0.0.1";
            port = 8080; // Docker port
            //port = 5000; // Local port
        }

        public void StartClient()
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
                OnErrorMessageReceived?.Invoke();
            }
        }

        public void CloseClient()
        {
            Client?.Close();
            receiveThread?.Join();
        }

        private async Task ReceiveMessagesAsync()
        {
            MessageBase message = null;

            while (Client.Connected)
            {
                try
                {
                    message = await MessageManager.ReceiveMessageAsync(Client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    continue;
                }

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
                        case InfoMessageTypeEnum.ModuleCreated:
                            ModuleCreated = ActionStatus.SUCCESS;
                            break;
                        case InfoMessageTypeEnum.ModuleNotCreated:
                            ModuleCreated = ActionStatus.FAILED;
                            break;
                        case InfoMessageTypeEnum.LobbyJoined:
                            LobbyJoined = ActionStatus.SUCCESS;
                            break;
                        case InfoMessageTypeEnum.LobbyNotJoined:
                            LobbyJoined = ActionStatus.FAILED;
                            break;
                        case InfoMessageTypeEnum.Error:
                            if (LobbyListReady == ActionStatus.PENDING)
                            { 
                                LobbyListReady = ActionStatus.FAILED;
                            }
                            if (ModuleListReady == ActionStatus.PENDING)
                            {
                                ModuleListReady = ActionStatus.FAILED;
                            }
                            if (BehaviourListReady == ActionStatus.PENDING)
                            {
                                BehaviourListReady = ActionStatus.FAILED;
                            }
                            break;

                        default:
                            break;
                    }

                    Console.WriteLine($"[Client] Received message: {infoMessage.MessageContent}");
                }
                else if (message.MessageType == MessageTypeEnum.LobbyData)
                {
                    LobbyDataMessage lobbyMessage = (LobbyDataMessage)message;
                    lock (playerGuidLock)
                    {
                        playerGuid = lobbyMessage.UserEntityID;
                    }
                    lock (lobbyDataLock)
                    {
                        lobbyData = lobbyMessage.Lobby;
                    }

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
                else if (message.MessageType == MessageTypeEnum.ModuleList)
                {
                    ModuleListMessage moduleListMessage = (ModuleListMessage)message;
                    ModuleListReady = ActionStatus.SUCCESS;
                    lock (modulesLock)
                    {
                        modules = moduleListMessage.Modules.ToList();
                    }

                    Console.WriteLine("Received module list");
                }
                else if (message.MessageType == MessageTypeEnum.BehaviourList)
                {
                    BehaviourListMessage behaviourListMessage = (BehaviourListMessage)message;
                    BehaviourListReady = ActionStatus.SUCCESS;
                    lock (behavioursLock)
                    {
                        behaviours = behaviourListMessage.Behaviours.ToList();
                    }

                    Console.WriteLine("Received behaviours list");
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
