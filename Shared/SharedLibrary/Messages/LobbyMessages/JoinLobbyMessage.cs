using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information for a user to join a lobby.
    /// </summary>
    public class JoinLobbyMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.JoinLobby;
        /// <summary>
        /// ID of the lobby to join.
        /// </summary>
        public int LobbyID { get; init; }
        /// <summary>
        /// Username of the player joining the lobby.
        /// </summary>
        public string UserName { get; init; }
        /// <summary>
        /// Constructor for JoinLobbyMessage.
        /// </summary>
        public JoinLobbyMessage(int lobbyID, string userName)
        {
            LobbyID = lobbyID;
            UserName = userName;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                LobbyID = this.LobbyID,
                UserName = this.UserName
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
