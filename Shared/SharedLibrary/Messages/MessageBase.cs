namespace SharedLibrary.Messages
{
    /// <summary>
    /// Base class for all messages exchanged between client and server.
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Message type, for example: JoinLobby, InfoMessage, WorldState etc.
        /// </summary>
        public abstract MessageTypeEnum MessageType { get; }

        /// <summary>
        /// Builds the JSON representation of the message.
        /// </summary>
        public abstract string BuildJson();
    }
}
