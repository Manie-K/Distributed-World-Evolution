namespace Server.Core.Helpers
{
    /// <summary>
    /// Parameters used in behaviours Execute method.
    /// </summary>
    public static class CustomBehaviourParams
    {
        /// <summary>
        /// Map walkable tiles parameter key.
        /// </summary>
        public const string MAP_WALKABLE_PARAM = "MapWalkable";

        /// <summary>
        /// Map fertile tiles parameter key.
        /// </summary>
        public const string MAP_FERTILE_PARAM = "MapFertile";

        /// <summary>
        /// New position parameter key.
        /// </summary>
        public const string NEW_POS_PARAM = "NewPosition";

        /// <summary>
        /// Lobby parameter key.
        /// </summary>
        public const string LOBBY_PARAM = "Lobby";

        /// <summary>
        /// Entities map parameter key.
        /// </summary>
        public const string ENTITIES_MAP_PARAM = "EntitiesMap";

        /// <summary>
        /// Enentities map lock parameter key.
        /// </summary>
        public const string ENTITIES_MAP_LOCK_PARAM = "EntitiesMapLock";

    }

}