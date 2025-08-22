using System.Text.Json;

namespace SharedLibrary
{
    public class UpdateUserStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.UpdateUserState;

        //TODO: add properties for user state change
        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public int UserHealth { get; set; }


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