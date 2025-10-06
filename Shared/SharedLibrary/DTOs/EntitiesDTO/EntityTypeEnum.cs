namespace Server.Core
{
    [Flags]
    public enum EntityTypeEnum
    {
        Human = 1 << 0,
        Animal = 1 << 1,
        Plant = 1 << 2
    }
}