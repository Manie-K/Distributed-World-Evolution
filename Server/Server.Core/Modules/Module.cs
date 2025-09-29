using System;
using Server.Core.Behaviours;
using Server.Core.Exceptions;
using Server.Core.Helpers;

namespace Server.Core.Modules
{
    public class Module
    {
        // We should try to place non-dynamic data here. All the dynamic data will be stored in WorldEntity object instances.
        public int ID { get; init; } // TODO: Replace with a proper ID system
        public bool Official { get; init; }
        public string Name { get; init; }
        public int Damage { get; private set; }
        public int Aggresion { get; private set; }
        public int ReproductionNeed { get; private set; }
        public EntityTypeEnum Type { get; init; }


        private readonly Dictionary<Type, IBehaviour> behaviours;

        private Module(string name, bool official, int damage, int aggresion, int reproductionNeed, EntityTypeEnum type)
        {
            ID = new Random().Next(1, int.MaxValue);
            Name = name;
            Official = official;
            Damage = damage;
            Aggresion = aggresion;
            ReproductionNeed = reproductionNeed;
            Type = type;
            behaviours = new Dictionary<Type, IBehaviour>();
        }

        private void AddBehaviour(IBehaviour behaviour)
        {
            var types = InterfaceHelpers.GetDirectParentInterfaces(behaviour.GetType());
            
            if(types.Count == 0)
            {
                throw new ArgumentException("Behaviour must implement at least one behaviour interface.");
            }

            if(types.Count > 1)
            {
                throw new ArgumentException("Behaviour must implement only one behaviour interface.");
            }

            var type = types[0];
            if (behaviours.ContainsKey(type))
            {
                throw new ArgumentException($"Behaviour of type {type.Name} already added to module.");
            }

            behaviours[type] = behaviour;
        }

        public IBehaviour GetBehaviourOfType(Type type)
        {
            if(!type.IsInterface || typeof(IBehaviour).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must be an interface that extends IBehaviour.");
            }

            behaviours.TryGetValue(type, out IBehaviour? found);
            return found ?? throw new BehaviourImplementationNotFoundException();
        }


        #region BUILDER

        public class ModuleBuilder
        {
            private string name;
            private bool official;
            private int damage;
            private int aggresion;
            private int reproductionNeed;
            private List<IBehaviour> behaviours;
            private EntityTypeEnum type;

            public ModuleBuilder()
            {
                name = "Default Module";
                official = false;
                damage = 0;
                aggresion = 0;
                reproductionNeed = 0;
                behaviours = new List<IBehaviour>();
            }

            public Module Create()
            {
                var module = new Module(name, official, damage, aggresion, reproductionNeed, type);
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
            public ModuleBuilder WithAggresion(int aggresion)
            {
                this.aggresion = aggresion;
                return this;
            }

            public ModuleBuilder WithReproductionNeed(int reproductionNeed)
            {
                this.reproductionNeed = reproductionNeed;
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