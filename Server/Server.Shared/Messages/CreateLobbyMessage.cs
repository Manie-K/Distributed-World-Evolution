using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class CreateLobbyMessage : IMessageBuilder
    {
        public string MessageType => "CreateLobby";

        //TODO: add properties for lobby creation
        //Question: How to add modules?
        public Guid GUID { get; set; }
        public string LobbyName { get; set; }
        public int MaxPlayers { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                type = MessageType,
                lobbyName = LobbyName,
                maxPlayers = MaxPlayers
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
