namespace Server.Core.Modules
{
    // VERY IMPORTANT TODO: Currently, each entity will require a seperate behaviour instance.
    // This is not memory efficient. We will need to unload it to client somehow.
    // Another way to solve this is to make behaviours static and stateless.
    // WE NEED TO COME BACK TO THIS LATER.
    // For now, I replaced it so each entity contains only the ID of the module it is based on.
    // This way, we only create instances for modules loaded in lobby, not separate for each entity.
    // This is a very important design decision.
    // To accomodate it, we need to keep all implementations state/fieldless for now.
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

        public EntityTypeEnum Type
        {
            get;
        }
    }
}