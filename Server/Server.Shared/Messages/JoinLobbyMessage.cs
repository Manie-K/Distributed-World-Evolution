using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class JoinLobbyMessage : IMessage
    {
        public IMessageType MessageType => IMessageType.JoinLobby;

        //TODO: add properties for lobby joining
        public Guid LobbyGUID { get; set; }
        public string LobbyName { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyGUID = LobbyGUID.ToString(),
                LobbyName = LobbyName,
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
