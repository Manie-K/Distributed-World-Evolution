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
        public int LobbyID { get; init; }
        /// <summary>
        /// Constructor for LobbyDataMessage.
        /// </summary>
        public LobbyDataMessage(LobbyDTO lobby, int lobbyID)
        {
            Lobby = lobby;
            LobbyID = lobbyID;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Lobby = this.Lobby,
                LobbyID = this.LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
