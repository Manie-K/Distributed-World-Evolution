using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class UserStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.UserState;

        //TODO: add properties for user state change
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
                MessageType = MessageType,
                UserGUID = UserGUID.ToString(),
                UserName = UserName,
                UserHealth = UserHealth,
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}