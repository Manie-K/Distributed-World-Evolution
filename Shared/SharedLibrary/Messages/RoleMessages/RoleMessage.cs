using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains information about the role of a TCP client in the communication.
    /// </summary>
    public class RoleMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.RoleMessage;
        
        /// <summary>
        /// Role of the TCP client.
        /// </summary>
        public RoleEnum Role { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="role"> Role of the TCP client. </param>
        public RoleMessage(RoleEnum role)
        {
            Role = role;
        }
        
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Role = this.Role      
            };

            return JsonSerializer.Serialize(payload);
        }

    }

}
