using System.Text.Json.Serialization;

namespace SharedLibrary
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageTypeEnum
    {
        CreateLobby,
        EntityState,
        WorldState,
        UserState,
        JoinLobby,
        InfoMessage
    }
}
