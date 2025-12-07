using Server.Core.Behaviours;

namespace Server.Core.Helpers
{
    public class LobbyParams
    {
        public const int NUM_INITIAL_ENTITIES = 750;
        public const int MAX_ENTITIES = 5000;
        public const double LOBBY_UPDATES_PER_SECOND = 64;
        public const int INITIAL_NUMBER_OF_GROUPS = 4;
        public const int HUNGER_CHANGE = 1;
        public const int HEALTH_CHANGE = 2;
        public const int CYCLES_PER_STATS_CHANGE = 16;

        /// <summary>
        /// Calculates interaction cooldown in cycles based on the number of entities in the world and interaction type.
        /// </summary>
        /// <param name="entitiesCount"></param>
        /// <param name="interactionTypeEnum"></param>
        /// <returns></returns>
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
