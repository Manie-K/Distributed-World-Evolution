using System.Text.Json.Serialization;

namespace Server.Shared.Messages
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
