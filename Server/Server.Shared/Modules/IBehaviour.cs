namespace Server.Shared.Modules
{
    public interface IBehaviour
    {
        // IMPORTANT! If this ID is changed or duplicated it will break the module system.
        // If the property name is changed, in memory database will collapse....
        // We also can't change the singnature to a method or make it non-readonly.
        // Don't touch it.....................:D
        public int DatabaseID
        {
            get;
        }
    }
}