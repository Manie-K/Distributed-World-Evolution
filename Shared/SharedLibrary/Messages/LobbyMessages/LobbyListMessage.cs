using System.Text.Json;
using SharedLibrary.DTOs.LobbyDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains a list of available lobbies.
    /// </summary>
    public class LobbyListMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.LobbyList;
        /// <summary>
        /// List of available lobbies.
        /// </summary>
        public IEnumerable<LobbyDTO> Lobbies { get; init; }
        /// <summary>
        /// Constructor for LobbyListMessage.
        /// </summary>
        public LobbyListMessage(IEnumerable<LobbyDTO> lobbies)
        {
            Lobbies = lobbies;
        }
        /// <inheritdoc/>
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
