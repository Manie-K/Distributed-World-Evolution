using System.Text.Json.Serialization;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Client role enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleEnum
    {
        User,
        UI
    }

}
