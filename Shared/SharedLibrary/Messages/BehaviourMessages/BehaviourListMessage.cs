using System.Text.Json;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages.BehaviourMessages
{
    /// <summary>
    /// Contains a list of behaviours available on the server.
    /// </summary>
    public class BehaviourListMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.BehaviourList;
        /// <summary>
        /// List of behaviours available on the server.
        /// </summary>
        public IEnumerable<BehaviourDTO> Behaviours { get; init; }
        /// <summary>
        /// Constructor for BehaviourListMessage.
        /// </summary>
        public BehaviourListMessage(IEnumerable<BehaviourDTO> behaviours)
        {
            Behaviours = behaviours;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType,
                Behaviours
            };
            return JsonSerializer.Serialize(payload);
        }
    }
}
