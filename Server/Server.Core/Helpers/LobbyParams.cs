using Server.Core.Behaviours;

namespace Server.Core.Helpers
{
    public class LobbyParams
    {
        public const int NUM_INITIAL_ENTITIES = 500;
        public const double LOBBY_UPDATES_PER_SECOND = 64;
        public const int INITIAL_NUMBER_OF_GROUPS = 32;
        public const int HUNGER_CHANGE = 1;
        public const int HEALTH_CHANGE = 2;
        public const int CYCLES_PER_STATS_CHANGE = 2;

        /// <summary>
        /// Calculates interaction cooldown in cycles based on the number of entities in the world and interaction type.
        /// </summary>
        /// <param name="entitiesCount"></param>
        /// <param name="interactionTypeEnum"></param>
        /// <returns></returns>
        public static int InteractionCooldownInCycles(int entitiesCount, InteractionTypeEnum interactionTypeEnum)
        {
            if (entitiesCount >= 1000)
                return 0;

            switch (interactionTypeEnum)
            {
                case InteractionTypeEnum.None:
                    return 0;

                case InteractionTypeEnum.Move:
                    {
                        if (entitiesCount >= 500)
                            return 2;
                        else if (entitiesCount >= 200)
                            return 3;
                        return 4;
                    }

                case InteractionTypeEnum.Attack:
                    if (entitiesCount >= 500)
                        return 0;
                    else if (entitiesCount >= 200)
                        return 1;
                    return 2;

                case InteractionTypeEnum.Eat:
                    if (entitiesCount >= 500)
                        return 0;
                    else if (entitiesCount >= 200)
                        return 1;
                    return 2;

                case InteractionTypeEnum.Reproduce:
                    if (entitiesCount >= 500)
                        return 0;
                    else if (entitiesCount >= 200)
                        return 1;
                    return 2;

                default:
                    return 0;
            }
        }
    }
}
