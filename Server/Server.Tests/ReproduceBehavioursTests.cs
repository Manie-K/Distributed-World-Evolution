using Moq;
using Server.Core;
using Server.Core.Behaviours.ReproduceBehaviour;
using Server.Core.Lobby;
using Server.Core.Services;
using Server.Core.Helpers;
using Xunit;
using Server.Core.Modules;

namespace Server.Tests
{
    /// <summary>
    /// Tests for reproduce behaviours.
    /// </summary>
    /// <remarks>Due to the fact that reproduce behaviours are quite complex, we test only base and plant CanExecute implementations for now.</remarks>
    public class ReproduceBehavioursTests : IDisposable
    {
        private readonly WorldEntity entity;
        private readonly WorldEntity entity_2;
        private readonly WorldEntity target;
        private readonly WorldEntity plant;

        private readonly Module firstModule;
        private readonly Module secondModule;
        private readonly Module plantModule;

        private readonly Mock<IModuleService> mockService;
        private readonly Mock<ILobby> mockLobby;

        private readonly bool[][] walkableTiles;

        // Run before every test
        public ReproduceBehavioursTests()
        {
            mockLobby = new Mock<ILobby>();

            mockLobby
                .Setup(l => l.AddWorldEntity(It.IsAny<WorldEntity>()))
                .Callback<WorldEntity>(e =>
                {
                    e = null!;
                })
                .Returns(true);

            entity = WorldEntity.CreateWorldEntity("Entity", 1, new EntityState(new SharedLibrary.Helpers.Position2D(5, 5), 100, 100, 0), mockLobby.Object);
            entity_2 = WorldEntity.CreateWorldEntity("Entity2", 2, new EntityState(new SharedLibrary.Helpers.Position2D(2, 2), 100, 100, 0), mockLobby.Object);
            target = WorldEntity.CreateWorldEntity("Target", 1, new EntityState(new SharedLibrary.Helpers.Position2D(6, 6), 100, 100, 0), mockLobby.Object);
            plant = WorldEntity.CreateWorldEntity("Plant", 3, new EntityState(new SharedLibrary.Helpers.Position2D(7, 7), 15, 100, 0), mockLobby.Object);

            firstModule = new Module.ModuleBuilder()
                .WithID(1)
                .WithName("FirstModule")
                .IsOfficial(false)
                .WithDamage(30)
                .WithAgression(50)
                .WithReproductionNeed(50)
                .OfType(EntityTypeEnum.Animal)
                .Create();
            
            secondModule = new Module.ModuleBuilder()
                .WithID(1)
                .WithName("FirstModule")
                .IsOfficial(false)
                .WithDamage(30)
                .WithAgression(50)
                .WithReproductionNeed(0)
                .OfType(EntityTypeEnum.Animal)
                .Create();

            plantModule = new Module.ModuleBuilder()
               .WithID(3)
               .WithName("PlantModule")
               .IsOfficial(false)
               .WithDamage(25)
               .WithAgression(25)
               .WithReproductionNeed(50)
               .WithMaxHealth(100)
               .OfType(EntityTypeEnum.Plant)
               .Create();

            mockService = new Mock<IModuleService>();
            mockService
                .Setup(ms => ms.GetModuleById(1))
                .Returns(firstModule);

            mockService
                .Setup(ms => ms.GetModuleById(2))
                .Returns(secondModule);

            mockService
                .Setup(ms => ms.GetModuleById(3))
                .Returns(plantModule);

            walkableTiles = new bool[10][];
            for (int x = 0; x < 10; x++)
            {
                walkableTiles[x] = new bool[10];
                for (int y = 0; y < 10; y++)
                {

                    walkableTiles[x][y] = true;
                }
            }
        }

        // Run after every test
        public void Dispose()
        {
            //no-op
        }

        [Fact]
        public void AllImplementationsHaveUniqueDatabaseIDs_Test()
        {
            //Arrange
            var behaviourTypes = typeof(ReproduceBehaviourBase).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ReproduceBehaviourBase)) && !t.IsAbstract);

            var databaseIDs = new HashSet<int>();

            //Act & Assert
            foreach (var type in behaviourTypes)
            {
                var instance = (ReproduceBehaviourBase)Activator.CreateInstance(type)!;
                Assert.True(databaseIDs.Add(instance.DatabaseID), $"Duplicate DatabaseID {instance.DatabaseID} found in {type.Name}");
            }
        }

        [Fact]
        public void ReproduceBehaviourBase_CanExecute_Test() 
        {
            //Arrange
            ReproduceBehaviourBase behaviour = new ReproduceAccordingToReproductionNeedBehaviour(); //It doesn't override CanExecute, so we can test the base class.

            //Act & Assert
            Assert.True(behaviour.CanExecute(entity, target, mockService.Object, new Dictionary<string, object>
            {
                { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
            }));

            Assert.False(behaviour.CanExecute(entity, entity, mockService.Object, new Dictionary<string, object>//same entity
            {
                { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
            }));

            Assert.False(behaviour.CanExecute(entity, plant, mockService.Object, new Dictionary<string, object> //wrong module
            {
                { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
            }));

            Assert.False(behaviour.CanExecute(plant, entity, mockService.Object, new Dictionary<string, object> //reproduction need too low
            {
                { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
            }));
        }


        [Fact]
        public void PlantReproduceBehaviour_CanExecute_Test() 
        {
            //Arrange
            ReproduceBehaviourBase behaviour = new PlantReproduceBehaviour();
            
            //Act & Assert
            Assert.True(behaviour.CanExecute(plant, null, mockService.Object));
            Assert.False(behaviour.CanExecute(entity, null, mockService.Object));
        }
    }
}
