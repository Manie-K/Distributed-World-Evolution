using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information needed to disjoin from a game lobby.
    /// </summary>
    public class DisjoinLobbyMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.DisjoinLobby;
        /// <summary>
        /// ID of the lobby to disjoin from.
        /// </summary>
        public int LobbyID { get; init; }
        /// <summary>
        /// Constructor for DisjoinLobbyMessage.
        /// </summary>
        public DisjoinLobbyMessage(int lobbyID)
        {
            LobbyID = lobbyID;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                LobbyID = this.LobbyID
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
