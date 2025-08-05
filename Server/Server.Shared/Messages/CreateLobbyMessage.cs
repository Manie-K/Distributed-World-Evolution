using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class CreateLobbyMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.CreateLobby;

        //TODO: add properties for lobby creation
        public string LobbyName { get; set; }
        public int MaxPlayers { get; set; }

        //TODO: change when modules are implemented
        Dictionary<string, string> Modules { get; set; } = new Dictionary<string, string>();


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyName = LobbyName,
                MaxPlayers = MaxPlayers,
                Modules = Modules
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
