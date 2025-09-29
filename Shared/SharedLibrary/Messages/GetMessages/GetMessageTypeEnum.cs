using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GetMessageTypeEnum
    {
        GetAllLobbies,
        GetAllModules,
        GetWorldState,
        GetAllBehaviors,
    }
}
