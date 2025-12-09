using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs.ModuleDTO
{
    /// <summary>
    /// Behaviour interaction type enumeration.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Serializable]
    public enum BehaviourInteractionTypeEnum
    {
        None, // For currently unavailable behaviours
        Move,
        Attack,
        Eat,
        Reproduce
    }

}