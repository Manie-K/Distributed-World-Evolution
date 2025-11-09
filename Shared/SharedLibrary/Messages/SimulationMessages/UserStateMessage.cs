using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains the state of a user.
    /// </summary>
    public class UserStateMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.UserState;
        /// <summary>
        /// User's unique identifier.
        /// </summary>
        public Guid UserGUID { get; init; }
        /// <summary>
        /// User's name.
        /// </summary>
        public string UserName { get; init; }
        /// <summary>
        /// User's health.
        /// </summary>
        public int UserHealth { get; init; }
        /// <summary>
        /// Constructor for UserStateMessage.
        /// </summary>
        public UserStateMessage (Guid userGUID, string userName, int userHealth)
        {
            UserGUID = userGUID;
            UserName = userName;
            UserHealth = userHealth;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                UserGUID = this.UserGUID.ToString(),
                UserName = this.UserName,
                UserHealth = this.UserHealth,
            };

            return JsonSerializer.Serialize(payload);
        }

    }

}