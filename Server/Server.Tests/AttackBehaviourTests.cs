using Moq;
using Server.Core;
using Server.Core.Behaviours.AttackBehaviour;
using Server.Core.Data;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using Xunit;

namespace Server.Tests
{
    public class AttackBehaviourTests : IDisposable
    {
        private readonly WorldEntity attacker;
        private readonly WorldEntity target;
        private readonly WorldEntity targetSameModule;
        private readonly WorldEntity human;

        private readonly Module firstModule;
        private readonly Module secondModule;
        private readonly Module humanModule;

        private readonly Mock<IModuleService> mockService;
        private readonly Mock<ILobby> mockLobby;

        //Run before every test
        public AttackBehaviourTests()
        {
            mockLobby = new Mock<ILobby>();

            mockLobby
                .Setup(l => l.DestroyWorldEntity(It.IsAny<WorldEntity>()))
                .Callback<WorldEntity>(e =>
                {
                    e = null!;
                })
                .Returns(true);

            attacker = WorldEntity.CreateWorldEntity("Attacker", 1, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 100, 0), mockLobby.Object);
            target = WorldEntity.CreateWorldEntity("Target", 2, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 100, 0), mockLobby.Object);
            targetSameModule = WorldEntity.CreateWorldEntity("TargetSameModule", 1, 
                new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 100, 100, 0), mockLobby.Object);
            human = WorldEntity.CreateWorldEntity("Human", 3, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0), 15, 100, 0), mockLobby.Object);

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
                .WithID(2)
                .WithName("SecondModule")
                .IsOfficial(false)
                .WithDamage(30)
                .WithAgression(30)
                .WithReproductionNeed(50)
                .OfType(EntityTypeEnum.Animal)
                .Create();

            humanModule = new Module.ModuleBuilder()
               .WithID(3)
               .WithName("HumanModule")
               .IsOfficial(false)
               .WithDamage(25)
               .WithAgression(25)
               .WithReproductionNeed(25)
               .WithMaxHealth(100)
               .OfType(EntityTypeEnum.Human)
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
                .Returns(humanModule);
        }

        //Run after every test
        public void Dispose()
        {
            //no-op
        }


        [Fact]
        public void AllImplementationsHaveUniqueDatabaseIDs_Test()
        {
            //Arrange
            var behaviourTypes = typeof(AttackBehaviourBase).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(AttackBehaviourBase)) && !t.IsAbstract);

            var databaseIDs = new HashSet<int>();

            //Act & Assert
            foreach (var type in behaviourTypes)
            {
                var instance = (AttackBehaviourBase)Activator.CreateInstance(type)!;
                Assert.True(databaseIDs.Add(instance.DatabaseID), $"Duplicate DatabaseID {instance.DatabaseID} found in {type.Name}");
            }
        }

        [Fact]
        public void BaseAttackBehaviour_Execute_Test()
        {
            //Arrange
            AttackBehaviourBase behaviour = new AlwaysAttackBehaviour();

            int initialAttackerHealth = attacker.State.Health;
            int initialTargetHealth = target.State.Health;

            int expectedTargetHealth = initialTargetHealth - firstModule.Damage;
            int expectedAttackerHealth = initialAttackerHealth - (int)(secondModule.Damage * 0.5);

            //Act
            behaviour.Execute(attacker, target, mockService.Object);

            //Assert
            Assert.Equal(expectedTargetHealth, target.State.Health);
            Assert.Equal(expectedAttackerHealth, attacker.State.Health);
        }

        [Fact]
        public void AlwaysAttackBehaviour_CanExecute_Test()
        {
            //Arrange
            AlwaysAttackBehaviour behaviour = new AlwaysAttackBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, target, mockService.Object), "AlwaysAttackBehaviour should return true for CanExecute for different entities.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "AlwaysAttackBehaviour should return false for CanExecute for same entities.");
        }

        [Fact]
        public void AlwaysAttackAnimalsBehaviour_CanExecute_Test()
        {
            //Arrange
            AlwaysAttackAnimalsBehaviour behaviour = new AlwaysAttackAnimalsBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, target, mockService.Object), "AlwaysAttackAnimalsBehaviour should return true for CanExecute when target is an animal and different entity.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "AlwaysAttackAnimalsBehaviour should return false for CanExecute when target is an animal and the same entity.");
            Assert.False(behaviour.CanExecute(attacker, human, mockService.Object), "AlwaysAttackAnimalsBehaviour should return false for CanExecute when target is human entity.");
        }

        [Fact]
        public void AlwaysAttackHumanBehaviour_CanExecute_Test()
        {
            //Arrange
            AlwaysAttackHumansBehaviour behaviour = new AlwaysAttackHumansBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, human, mockService.Object), "AlwaysAttackAnimalsBehaviour should return true for CanExecute when target is human entity.");
            Assert.False(behaviour.CanExecute(attacker, target, mockService.Object), "AlwaysAttackAnimalsBehaviour should return false for CanExecute when target is an animal and different entity.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "AlwaysAttackAnimalsBehaviour should return false for CanExecute when target is an animal and the same entity.");
        }

        [Fact]
        public void GreaterAggressionAttackBehaviour_Execute_Test()
        {
            //Arrange
            GreaterAggressionAttackBehaviour behaviour = new GreaterAggressionAttackBehaviour();

            int initialTargetHealth = target.State.Health;
            int initialAttackerHealth = attacker.State.Health;

            int expectedTargetHealth = initialTargetHealth - firstModule.Damage;
            int expectedAttackerHealth = initialAttackerHealth; // Attacker does not take damage

            //Act
            behaviour.Execute(attacker, target, mockService.Object);

            //Assert
            Assert.Equal(expectedTargetHealth, target.State.Health);
            Assert.Equal(expectedAttackerHealth, attacker.State.Health);
        }

        [Fact]
        public void GreaterAggressionAttackBehaviour_CanExecute_Test()
        {
            //Arrange
            GreaterAggressionAttackBehaviour behaviour = new GreaterAggressionAttackBehaviour();
            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, target, mockService.Object), "GreaterAgressionAttackBehaviour should return true for CanExecute when attacker has greater aggression than target.");
            Assert.False(behaviour.CanExecute(target, attacker, mockService.Object), "GreaterAgressionAttackBehaviour should return false for CanExecute when attacker has lower aggression than target.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "GreaterAgressionAttackBehaviour should return false for CanExecute when entities are the same.");
        }

        [Fact]
        /// <summary>
        /// We don't simulate randomness in unit tests, so we just check that CanExecute always returns false when attacker and target are the same entities.
        /// </summary>
        public void Random25AttackBehaviour_CanExecute_Test()
        {
            //Arrange
            Random25AttackBehaviour behaviour = new Random25AttackBehaviour();

            //Act & Assert
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "Random25AttackBehaviour should return false for CanExecute when target is the same entity");
        }

        [Fact]
        /// <summary>
        /// We don't simulate randomness in unit tests, so we just check that CanExecute always returns false when attacker and target are the same entities.
        /// </summary>
        public void Random50AttackBehaviour_CanExecute_Test()
        {
            //Arrange
            Random50AttackBehaviour behaviour = new Random50AttackBehaviour();

            //Act & Assert
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "Random50AttackBehaviour should return false for CanExecute when target is the same entity");
        }

        [Fact]
        /// <summary>
        /// We don't simulate randomness in unit tests, so we just check that CanExecute always returns false when attacker and target are the same entities.
        /// </summary>
        public void Random75AttackBehaviour_CanExecute_Test()
        {
            //Arrange
            Random75AttackBehaviour behaviour = new Random75AttackBehaviour();

            //Act & Assert
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "Random75AttackBehaviour should return false for CanExecute when target is the same entity");
        }

        [Fact]
        public void EnemyLowHealthAttackBehaviour_Execute_Test()
        {
            //Arrange
            EnemyLowHealthAttackBehaviour behaviour = new EnemyLowHealthAttackBehaviour();

            int initialAttackerHealth = attacker.State.Health;
            int initialTargetHealth = human.State.Health;

            int expectedTargetHealth = initialTargetHealth - firstModule.Damage;
            int expectedAttackerHealth = initialAttackerHealth - (int)(humanModule.Damage * 0.25);

            //Act
            behaviour.Execute(attacker, human, mockService.Object);

            //Assert
            Assert.Equal(expectedTargetHealth, human.State.Health);
            Assert.Equal(expectedAttackerHealth, attacker.State.Health);
        }

        [Fact]
        public void EnemyLowHealthAttackBehaviour_CanExecute_Test()
        {
            //Arrange
            EnemyLowHealthAttackBehaviour behaviour = new EnemyLowHealthAttackBehaviour();
            human.State.Health = 20; // Set health below 30%.  20 of 100 is 20% < 30%.
            target.State.Health = secondModule.MaxHealth; // Set health above 30%.

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, human, mockService.Object), "EnemyLowHealthAttackBehaviour should return true for CanExecute when target health is below 30%.");
            Assert.False(behaviour.CanExecute(attacker, target, mockService.Object), "EnemyLowHealthAttackBehaviour should return false for CanExecute when target health is above 30%.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "EnemyLowHealthAttackBehaviour should return false for CanExecute when entities are the same.");
        }

        [Fact]
        public void OnlyAttackSelfSpecies_CanExecute_Test()
        {
            //Arrange
            OnlyAttackSelfSpiecesAttackBehaviour behaviour = new OnlyAttackSelfSpiecesAttackBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, targetSameModule, mockService.Object), "OnlyAttackSelfSpecies should return true for CanExecute when entities have the same module.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "OnlyAttackSelfSpecies should return false for CanExecute when entities are the same one.");
            Assert.False(behaviour.CanExecute(attacker, target, mockService.Object), "OnlyAttackSelfSpecies should return false for CanExecute when entities are different modules.");
        }

        [Fact]
        public void DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour_Execute_Test()
        {
            //Arrange 
            DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour behaviour = new DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour();

            int initialTargetHealth = target.State.Health;
            int initialAttackerHealth = attacker.State.Health;

            int expectedTargetHealth = initialTargetHealth - (int)(firstModule.Damage * ((firstModule.Agression - secondModule.Agression) / firstModule.Agression));
            int expectedAttackerHealth = initialAttackerHealth; // Attacker does not take damage

            //Act
            behaviour.Execute(attacker, target, mockService.Object);

            //Assert
            Assert.Equal(expectedTargetHealth, target.State.Health);
            Assert.Equal(expectedAttackerHealth, attacker.State.Health);
        }

        [Fact]
        public void DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour_CanExecute_Test()
        {
            //Arrange 
            DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour behaviour = new DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour();

            //Act & Assert
            Assert.True(behaviour.CanExecute(attacker, target, mockService.Object), 
                "DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour should return true for CanExecute when attacker has greater aggression than target.");

            Assert.False(behaviour.CanExecute(target, attacker, mockService.Object),
                "DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour should return false for CanExecute when attacker has lower aggression than target.");

            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object),
                "DifferentDamageCalculationAttackWhenAgressionGreaterAttackBehaviour should return false for CanExecute when there are the same entities.");
        }

        [Fact]
        public void NeverAttackBehaviour_Execute_Test()
        {
            //Arrange
            NeverAttackBehaviour behaviour = new NeverAttackBehaviour();

            int initialAttackerHealth = attacker.State.Health;
            int initialTargetHealth = target.State.Health;

            int expectedTargetHealth = initialTargetHealth;
            int expectedAttackerHealth = initialAttackerHealth;
            
            //Act
            behaviour.Execute(attacker, target, mockService.Object);

            //Assert
            Assert.Equal(expectedTargetHealth, target.State.Health);
            Assert.Equal(expectedAttackerHealth, attacker.State.Health);
        }

        [Fact]
        public void NeverAttackBehaviour_CanExecute_Test()
        {
            //Arrange
            NeverAttackBehaviour behaviour = new NeverAttackBehaviour();

            //Act & Assert
            Assert.False(behaviour.CanExecute(attacker, target, mockService.Object), "NeverAttackBehaviour should always return false.");
            Assert.False(behaviour.CanExecute(target, attacker, mockService.Object), "NeverAttackBehaviour should always return false.");
            Assert.False(behaviour.CanExecute(attacker, attacker, mockService.Object), "NeverAttackBehaviour should always return false.");
        }
    }
}
