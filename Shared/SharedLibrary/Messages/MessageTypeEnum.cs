using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Message types enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageTypeEnum
    {
        CreateLobby,
        UserInteraction,
        WorldState,
        UserState,
        JoinLobby,
        InfoMessage,
        RoleMessage,
        LogMessage,
        DisjoinLobby,
        ModuleList,
        BehaviourList,
        CreateModule,
        LobbyList,
        GetMessage,
        LobbyData,
    }
}
