using Server.Core.Behaviours;
using Server.Core.Exceptions;
using Server.Core.Helpers;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Modules
{
    public class Module
    {
        // We should try to place non-dynamic data here. All the dynamic data will be stored in WorldEntity object instances.
        public bool Official { get; init; }
        public string Name { get; init; }
        public int Damage { get; private set; }
        public int Agression { get; private set; }
        public int ReproductionNeed { get; private set; }
        public int MaxHunger { get; private set; }
        public int MaxHealth { get; private set; }
        public EntityTypeEnum Type { get; init; }
        public int GraphicalRepresentationID { get; private set; }

        private readonly Dictionary<Type, IBehaviour> behaviours;

        private Module(string name, bool official, int damage, int aggresion, int reproductionNeed, int maxHunger, int maxHelath, EntityTypeEnum type, int graphicsId)
        {
            Name = name;
            Official = official;
            Damage = damage;
            Agression = aggresion;
            ReproductionNeed = reproductionNeed;
            MaxHunger = maxHunger;
            MaxHealth = maxHelath;
            Type = type;
            behaviours = new Dictionary<Type, IBehaviour>();
            GraphicalRepresentationID = graphicsId;
        }

        private void AddBehaviour(IBehaviour behaviour)
        {
            Type? type = TypeHelpers.GetFirstAbstractParentType(behaviour.GetType());

            if (type == null)
            {
                throw new ArgumentException("Behaviour must extend an abstract base class.");
            }

            if (behaviours.ContainsKey(type))
            {
                throw new ArgumentException($"Behaviour of type {type.Name} already added to module.");
            }

            behaviours[type] = behaviour;
        }

        public IBehaviour GetBehaviourOfType(Type type)
        {
            if (!type.IsInterface || typeof(IBehaviour).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must be an interface that extends IBehaviour.");
            }

            behaviours.TryGetValue(type, out IBehaviour? found);
            return found ?? throw new BehaviourImplementationNotFoundException();
        }

        public ModuleDTO ToDTO()
        {
            return new ModuleDTO(0, Name, Official, Damage, Agression, ReproductionNeed, null, Type, GraphicalRepresentationID);
        }

        #region BUILDER

        public class ModuleBuilder
        {
            private string name;
            private bool official;
            private int damage;
            private int aggresion;
            private int reproductionNeed;
            private int maxHunger;
            private int maxHelath;
            private readonly List<IBehaviour> behaviours;
            private EntityTypeEnum type;
            private int graphicsId;

            public ModuleBuilder()
            {
                name = "Default ModuleDTO";
                official = false;
                damage = 0;
                aggresion = 0;
                reproductionNeed = 0;
                maxHunger = 100;
                maxHelath = 100;
                behaviours = new List<IBehaviour>();
                type = EntityTypeEnum.Animal;
                graphicsId = 0;
            }

            public Module Create()
            {
                var module = new Module(name, official, damage, aggresion, reproductionNeed, maxHunger, maxHelath, type, graphicsId);
                foreach (var behaviour in behaviours)
                {
                    module.AddBehaviour(behaviour);
                }
                return module;
            }

            public ModuleBuilder OfType(EntityTypeEnum type)
            {
                this.type = type;
                return this;
            }

            public ModuleBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public ModuleBuilder IsOfficial(bool official)
            {
                this.official = official;
                return this;
            }

            public ModuleBuilder WithDamage(int damage)
            {
                this.damage = damage;
                return this;
            }
            public ModuleBuilder WithAgression(int aggresion)
            {
                this.aggresion = aggresion;
                return this;
            }

            public ModuleBuilder WithReproductionNeed(int reproductionNeed)
            {
                this.reproductionNeed = reproductionNeed;
                return this;
            }

            public ModuleBuilder WithMaxHunger(int maxHunger)
            {
                this.maxHunger = maxHunger;
                return this;
            }

            public ModuleBuilder WithMaxHealth(int maxHealth)
            {
                this.maxHelath = maxHealth;
                return this;
            }

            public ModuleBuilder WithGraphicsId(int graphicsId)
            {
                this.graphicsId = graphicsId;
                return this;
            }

            public ModuleBuilder WithBehaviour(IBehaviour behaviour)
            {
                behaviours.Add(behaviour);
                return this;
            }

            public ModuleBuilder WithBehaviours(List<IBehaviour> behaviours)
            {
                this.behaviours.AddRange(behaviours);
                return this;
            }
        }

        #endregion
    
    }
}