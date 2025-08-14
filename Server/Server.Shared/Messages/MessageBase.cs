namespace Server.Shared.Messages
{
    public abstract class MessageBase
    {
        public abstract MessageTypeEnum MessageType { get; }
        public abstract string BuildJson();
    }
}
