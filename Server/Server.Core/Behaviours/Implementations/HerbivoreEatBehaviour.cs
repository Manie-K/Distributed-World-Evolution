using Server.Core.Modules;

namespace Server.Core.Behaviours.Implementations
{
    public class HerbivoreEatBehaviour : IEatBehaviour
    {
        public int DatabaseID => 702;

        public EntityTypeEnum Type => EntityTypeEnum.Animal;

        public bool CanEat(WorldEntity entity, WorldEntity food)
        {
            Module entModule = ModuleService.Instance.GetModuleById(entity.ModuleID);
            Module foodModule = ModuleService.Instance.GetModuleById(food.ModuleID);

            return entModule.Type == EntityTypeEnum.Animal && foodModule.Type == EntityTypeEnum.Plant;
        }

        public void Eat(WorldEntity entity, WorldEntity food)
        {
            throw new NotImplementedException();
        }
    }
}