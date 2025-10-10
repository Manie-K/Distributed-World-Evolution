using SharedLibrary.DTOs.LobbyDTO;
using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information about a lobby.
    /// </summary>
    public class LobbyDataMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyData;
        /// <summary>
        /// Lobby information.
        /// </summary>
        public LobbyDTO Lobby { get; init; }
        /// <summary>
        /// Lobby ID.
        /// </summary>
        public LobbyDataMessage(LobbyDTO lobby)
        {
            Lobby = lobby;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Lobby = this.Lobby
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
