using Xunit;
using Server.Core.Behaviours.AttackBehaviour;
using Server.Core;

namespace Server.Tests
{
    public class AttackBehaviourTests : IDisposable
    {
        //Run before every test
        public AttackBehaviourTests()
        {
        
        }

        //Run after every test
        public void Dispose()
        {

        }


        [Fact]
        public void AllImplementationsHaveUniqueDatabaseIDsTest()
        {
            var behaviourTypes = typeof(AttackBehaviourBase).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(AttackBehaviourBase)) && !t.IsAbstract);

            var databaseIDs = new HashSet<int>();
            foreach (var type in behaviourTypes)
            {
                var instance = (AttackBehaviourBase)Activator.CreateInstance(type)!;
                Assert.True(databaseIDs.Add(instance.DatabaseID), $"Duplicate DatabaseID {instance.DatabaseID} found in {type.Name}");
            }
        }

        [Fact]
        public void AlwaysAttackBehaviourTest()
        {
            AlwaysAttackBehaviour behaviour = new AlwaysAttackBehaviour();
            WorldEntity attacker = WorldEntity.CreateWorldEntity("Attacker", 1, new EntityState(new SharedLibrary.Helpers.Position2D(0,0), 100, 100, 0));
            WorldEntity target = WorldEntity.CreateWorldEntity("Target", 2, new EntityState(new SharedLibrary.Helpers.Position2D(0,0), 100, 100, 0));
            WorldEntity targetSameModule = WorldEntity.CreateWorldEntity("TargetSameModule", 1, new EntityState(new SharedLibrary.Helpers.Position2D(0,0), 100, 100, 0));


        }
    }
}
