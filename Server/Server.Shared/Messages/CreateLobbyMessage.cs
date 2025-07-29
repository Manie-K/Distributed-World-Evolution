using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class CreateLobbyMessage : IMessage
    {
        public IMessageType MessageType => IMessageType.CreateLobby;

        //TODO: add properties for lobby creation
        //Question: How to add modules?
        public Guid LobbyGUID { get; set; }
        public string LobbyName { get; set; }
        public int MaxPlayers { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                message_type = MessageType,
                lobby_guid = LobbyGUID.ToString(),
                lobby_name = LobbyName,
                max_players = MaxPlayers
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
