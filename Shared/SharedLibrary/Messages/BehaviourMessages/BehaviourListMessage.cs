using System.Text.Json;
using SharedLibrary.DTOs.ModuleDTO;

namespace SharedLibrary.Messages.BehaviourMessages
{
    public class BehaviourListMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.BehaviourList;
        public IEnumerable<BehviourDTO> Behaviours { get; init; }

        public BehaviourListMessage(IEnumerable<BehviourDTO> behaviours)
        {
            Behaviours = behaviours;
        }

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
