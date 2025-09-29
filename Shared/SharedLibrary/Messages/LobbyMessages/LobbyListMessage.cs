using System.Text.Json;
using SharedLibrary.DTOs.LobbyDTO;

namespace SharedLibrary.Messages
{
    public class LobbyListMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyList;

        public IEnumerable<LobbyDTO> Lobbies { get; init; }

        public LobbyListMessage(IEnumerable<LobbyDTO> lobbies)
        {
            Lobbies = lobbies;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Lobbies = this.Lobbies
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
