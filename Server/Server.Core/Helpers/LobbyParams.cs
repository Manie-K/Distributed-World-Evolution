using Server.Core.Behaviours;

namespace Server.Core.Helpers
{
    /// <summary>
    /// Lobby parameters and interaction cooldown calculations.
    /// </summary>
    public static class LobbyParams
    {
        /// <summary>
        /// Initial number of entities in the lobby.
        /// </summary>
        public const int NUM_INITIAL_ENTITIES = 750;

        /// <summary>
        /// Maximum number of entities allowed in the lobby.
        /// </summary>
        public const int MAX_ENTITIES = 5000;

        /// <summary>
        /// Number of lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;

        /// <summary>
        /// Initial number of groups in the lobby.
        /// </summary>
        /// <remarks>Used in calculating entities per group to simulate every update.</remarks>
        public const int INITIAL_NUMBER_OF_GROUPS = 4;

        /// <summary>
        /// Hunger change per stats update.
        /// </summary>
        public const int HUNGER_CHANGE = 1;

        /// <summary>
        /// Health change per stats update.
        /// </summary>
        public const int HEALTH_CHANGE = 2;

        /// <summary>
        /// Cycles per stats change.
        /// </summary>
        public const int CYCLES_PER_STATS_CHANGE = 16;

        /// <summary>
        /// Calculates interaction cooldown in cycles based on the number of entities in the world and interaction type.
        /// </summary>
        /// <param name="entitiesCount"> The number of entities in the world. </param>
        /// <param name="interactionTypeEnum"> The type of interaction. </param>
        /// <returns> The calculated cooldown in cycles. </returns>
        public static int InteractionCooldownInCycles(int entitiesCount, InteractionTypeEnum interactionTypeEnum)
        {
            const int modifier = 8;
            if (entitiesCount >= 1000)
                if(interactionTypeEnum == InteractionTypeEnum.Move)
                    return 2 * modifier;
                else
                    return 1 * modifier;

            switch (interactionTypeEnum)
            {
                case InteractionTypeEnum.None:
                    return 1 * modifier;

                case InteractionTypeEnum.Move:
                    if (entitiesCount >= 750)
                        return 2 * modifier;
                    if (entitiesCount >= 500)
                        return 2 * modifier;
                    else if (entitiesCount >= 200)
                        return 3 * modifier;
                    return 4 * modifier;

                case InteractionTypeEnum.Attack:
                    if (entitiesCount >= 500)
                        return 2 * modifier;
                    else if (entitiesCount >= 200)
                        return 2 * modifier;
                    return 3 * modifier;

                case InteractionTypeEnum.Eat:
                    if (entitiesCount >= 500)
                        return 1 * modifier;
                    else if (entitiesCount >= 200)
                        return 2 * modifier;
                    return 3 * modifier;

                case InteractionTypeEnum.Reproduce:
                    if (entitiesCount >= 500)
                        return 4 * modifier;
                    else if (entitiesCount >= 200)
                        return 4 * modifier;
                    return 5 * modifier;

                default:
                    return 1 * modifier;
            }
        }

    }

}