using System.Text.Json;

namespace SharedLibrary.Messages
{
    //TODO: Change after user state is fully implemented
    public class UserStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.UserState;

        public Guid UserGUID { get; init; }
        public string UserName { get; init; }
        public int UserHealth { get; init; }

        public UserStateMessage (Guid userGUID, string userName, int userHealth)
        {
            UserGUID = userGUID;
            UserName = userName;
            UserHealth = userHealth;
        }

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