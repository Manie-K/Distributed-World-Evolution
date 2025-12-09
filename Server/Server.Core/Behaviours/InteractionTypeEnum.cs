using System.Text.Json.Serialization;

namespace Server.Core.Behaviours
{
    /// <summary>
    /// Enumeration representing different types of interactions that entities can perform.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InteractionTypeEnum
    {
        None = 0,
        Move = 1,
        Attack = 2,
        Eat = 3,
        Reproduce = 4
    }

}