using System.Text.Json.Serialization;

namespace Server.Core
{
    /// <summary>
    /// Enumeration representing different types of entities.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum EntityTypeEnum
    {
        Human = 1 << 0,
        Animal = 1 << 1,
        Plant = 1 << 2
    }

}