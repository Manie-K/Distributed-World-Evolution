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
                message_type = MessageType,
                user_guid = LobbyGUID.ToString(),
                lobby_name = LobbyName,
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
