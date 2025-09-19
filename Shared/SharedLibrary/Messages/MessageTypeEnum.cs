using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageTypeEnum
    {
        CreateLobby,
        EntityState,
        WorldState,
        UserState,
        JoinLobby,
        InfoMessage,
        RoleMessage,
        LogMessage,
        DisjoinLobby
    }
}
