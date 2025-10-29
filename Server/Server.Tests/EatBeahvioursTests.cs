using Moq;
using Server.Core;
using Server.Core.Behaviours.AttackBehaviour;
using Server.Core.Behaviours.EatBehaviour;
using Server.Core.Modules;
using Server.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Server.Tests
{
    //TODO: Check if plant is dead after Die() implementation
    public class EatBeahvioursTests
    {
        private readonly WorldEntity eaterEntity;
        private readonly WorldEntity normalPlantEntity;
        private readonly WorldEntity poisonousPlantEntity;

        private readonly Module eaterModule;
        private readonly Module normalPlantModule;
        private readonly Module poisonousPlantModule;

        private readonly Mock<IModuleService> mockService;

        //Run before every test
        public EatBeahvioursTests()
        {
            eaterEntity = WorldEntity.CreateWorldEntity("eaterEntity", 1, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 100, 0));
            normalPlantEntity = WorldEntity.CreateWorldEntity("normalPlantEntity", 2, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 50, 0));
            poisonousPlantEntity = WorldEntity.CreateWorldEntity("poisonousPlantEntity", 3, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 0, 0));

            eaterModule = new Module.ModuleBuilder()
                .WithName("eaterModule")
                .WithReproductionNeed(5)
                .WithMaxHealth(100)
                .WithMaxHunger(100)
                .OfType(EntityTypeEnum.Animal)
                .Create();

            normalPlantModule = new Module.ModuleBuilder()
                .WithName("normalPlantModule")
                .WithDamage(0)
                .WithMaxHealth(100)
                .WithMaxHunger(50)
                .OfType(EntityTypeEnum.Animal)
                .Create();

            poisonousPlantModule = new Module.ModuleBuilder()
                .WithName("poisonousPlantModule")
                .WithDamage(10)
                .WithMaxHealth(100)
                .WithMaxHunger(0)
                .OfType(EntityTypeEnum.Animal)
                .Create();

            mockService = new Mock<IModuleService>();
            mockService
                .Setup(ms => ms.GetModuleById(1))
                .Returns(eaterModule);

            mockService
                .Setup(ms => ms.GetModuleById(2))
                .Returns(normalPlantModule);

            mockService
                .Setup(ms => ms.GetModuleById(3))
                .Returns(poisonousPlantModule);
        }

        //Run after every test
        public void Dispose()
        {

        }

        [Fact]
        public void AllImplementationsHaveUniqueDatabaseIDsTest()
        {
            //Arrange
            var behaviourTypes = typeof(EatBehaviourBase).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(EatBehaviourBase)) && !t.IsAbstract);

            var databaseIDs = new HashSet<int>();

            //Act & Assert
            foreach (var type in behaviourTypes)
            {
                var instance = (EatBehaviourBase)Activator.CreateInstance(type)!;
                Assert.True(databaseIDs.Add(instance.DatabaseID), $"Duplicate DatabaseID {instance.DatabaseID} found in {type.Name}");
            }
        }

        [Fact]
        public void BaseAttackBehaviour_Execute_Test()
        {
            //Arrange
            EatBehaviourBase behaviour = new EatAlwaysBehaviour();

            int initialEaterHunger = eaterEntity.State.Hunger;
            int initialEaterHealth = eaterEntity.State.Health;

            int expectedEaterHunger = initialEaterHunger + normalPlantModule.MaxHunger;
            int expectedEaterHealth = initialEaterHealth - poisonousPlantModule.Damage;

            //Act
            behaviour.Execute(eaterEntity, normalPlantEntity, mockService.Object);
            behaviour.Execute(eaterEntity, poisonousPlantEntity, mockService.Object);

            //Assert
            Assert.Equal(expectedEaterHunger, eaterEntity.State.Hunger);
            Assert.Equal(expectedEaterHealth, eaterEntity.State.Health);
        }

        [Fact]
        public void EatAlwaysBehaviour_CanExecute_Test()
        {
            //Arrange
            EatAlwaysBehaviour behaviour = new EatAlwaysBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatAlwaysBehaviour should return true for CanExecute always.");
        }

        [Fact]
        public void EatNeverBehaviour_Execute_Test()
        {
            //Arrange
            EatNeverBehaviour behaviour = new EatNeverBehaviour();
            
            int initialEaterHunger = eaterEntity.State.Hunger;
            int initialEaterHealth = eaterEntity.State.Health;

            int expectedEaterHunger = initialEaterHunger;
            int expectedEaterHealth = initialEaterHealth;

            //Act
            behaviour.Execute(eaterEntity, normalPlantEntity, mockService.Object);
            behaviour.Execute(eaterEntity, poisonousPlantEntity, mockService.Object);

            //Assert
            Assert.Equal(expectedEaterHunger, eaterEntity.State.Hunger);
            Assert.Equal(expectedEaterHealth, eaterEntity.State.Health);
        }

        [Fact]
        public void EatNeverBehaviour_CanExecute_Test()
        {
            //Arrange
            EatNeverBehaviour behaviour = new EatNeverBehaviour();

            //Act & Assert
            Assert.False(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatNeverBehaviour should return false for CanExecute always.");
        }

        [Fact]
        public void EatWhenHungry25Behaviour_CanExecute_Test()
        {
            //Arrange, Act & Assert when hunger is not below 25% of organism maximum hunger
            EatWhenHungry25Behaviour behaviour = new EatWhenHungry25Behaviour();
            
            eaterEntity.State.Hunger = 25;

            Assert.False(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenHungry25Behaviour should return false for CanExecute when hunger is not below 25% of organism maximum hunger.");

            //Arrange, Act & Assert when hunger is below 25% of organism maximum hunger
            eaterEntity.State.Hunger = 24;

            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenHungry25Behaviour should return true for CanExecute when hunger is below 25% of organism maximum hunger.");
        }

        [Fact]
        public void EatWhenNotHealthy50Behaviour_CanExecute_Test()
        {
            //Arrange, Act & Assert when health is not below 50% of organism maximum health
            EatWhenNotHealthy50Behaviour behaviour = new EatWhenNotHealthy50Behaviour();

            eaterEntity.State.Health = 50;

            Assert.False(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenNotHealthy50Behaviour should return false for CanExecute when health is not below 50% of organism maximum health.");

            //Arrange, Act & Assert when hunger is below 25% of organism maximum health
            eaterEntity.State.Health = 49;

            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenNotHealthy50Behaviour should return true for CanExecute when health is below 50% of organism maximum health.");
        }

        [Fact]
        public void EatWhenHungerLesserThanReproductionNeed_CanExecute_Test()
        {
            //Arrange, Act & Assert when hunger is not lesser than reproduction need
            EatWhenHungerLesserThanReproductionNeed behaviour = new EatWhenHungerLesserThanReproductionNeed();

            eaterEntity.State.Hunger = 50;

            Assert.False(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenHungerLesserThanReproductionNeed should return false for CanExecute when Hunger/MaxHunger is not lesser than ReproductionNeed/MAX_REPRODUCTION_NEED.");

            //Arrange, Act & Assert when hunger is lesser than reproduction need
            eaterEntity.State.Hunger = 49;

            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWhenHungerLesserThanReproductionNeed should return true for CanExecute when Hunger/MaxHunger is lesser than ReproductionNeed/MAX_REPRODUCTION_NEED.");
        }

        [Fact]
        public void EatWithCureBehaviour_NormalPlant_Execute_Test()
        {
            //Arrange
            EatWithCureBehaviour behaviour = new EatWithCureBehaviour();

            int initialEaterHunger = eaterEntity.State.Hunger;
            int initialEaterHealth = eaterEntity.State.Health;

            int expectedEaterHunger = initialEaterHunger + normalPlantModule.MaxHunger;
            int expectedEaterHealth = initialEaterHealth + normalPlantModule.MaxHunger;

            //Act
            behaviour.Execute(eaterEntity, normalPlantEntity, mockService.Object);

            //Assert
            Assert.Equal(expectedEaterHunger, eaterEntity.State.Hunger);
            Assert.Equal(expectedEaterHealth, eaterEntity.State.Health);
        }

        [Fact]
        public void EatWithCureBehaviour_PoisonousPlant_Execute_Test()
        {
            //Arrange
            EatWithCureBehaviour behaviour = new EatWithCureBehaviour();

            int initialEaterHealth = eaterEntity.State.Health;

            int expectedEaterHealth = initialEaterHealth - 2 * poisonousPlantModule.Damage;

            //Act
            behaviour.Execute(eaterEntity, poisonousPlantEntity, mockService.Object);

            //Assert
            Assert.Equal(expectedEaterHealth, eaterEntity.State.Health);
        }

        [Fact]
        public void EatWithCureBehaviour_CanExecute_Test()
        {
            //Arrange
            EatWithCureBehaviour behaviour = new EatWithCureBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatWithCureBehaviour should return true for CanExecute always.");
        }

        [Fact]
        public void EatIfNotPoisonous_CanExecute_Test()
        {
            //Arrange
            EatIfNotPoisonous behaviour = new EatIfNotPoisonous();

            //Act & Assert
            Assert.True(behaviour.CanExecute(eaterEntity, normalPlantEntity, mockService.Object), "EatIfNotPoisonous should return true for CanExecute if plant is not poisonous.");
            Assert.False(behaviour.CanExecute(eaterEntity, poisonousPlantEntity, mockService.Object), "EatIfNotPoisonous should return false for CanExecute if plant is poisonous.");
        }

    }
}
