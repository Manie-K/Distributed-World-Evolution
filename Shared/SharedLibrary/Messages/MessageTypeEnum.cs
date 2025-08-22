using System.Text.Json.Serialization;

namespace SharedLibrary
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageTypeEnum
    {
        CreateLobby,
        UpdateWorldEntityState,
        RefreshWorldEntities,
        UpdateUserState,
        JoinLobby,
        DefaultMessage
    }
}
