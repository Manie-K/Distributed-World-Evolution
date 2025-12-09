namespace SharedLibrary.Messages
{
    /// <summary>
    /// Base class for all messages exchanged between client and server.
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Message type.
        /// </summary>
        public abstract MessageTypeEnum MessageType { get; }

        /// <summary>
        /// Builds the JSON representation of the message.
        /// </summary>
        /// <returns> JSON string of the message. </returns>
        public abstract string BuildJson();
    }
}
