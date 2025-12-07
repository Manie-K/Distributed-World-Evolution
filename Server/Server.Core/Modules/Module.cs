using Server.Core.Behaviours;
using Server.Core.Behaviours.AttackBehaviour;
using Server.Core.Behaviours.EatBehaviour;
using Server.Core.Behaviours.MoveBehaviour;
using Server.Core.Behaviours.ReproduceBehaviour;
using Server.Core.Data;
using Server.Core.Exceptions;
using Server.Core.Helpers;
using Server.Core.Services;
using SharedLibrary.DTOs.ModuleDTO;

namespace Server.Core.Modules
{
    /// <summary>
    /// Class representing a module with various attributes and behaviours.
    /// </summary>
    public class Module
    {
        /// <summary>
        /// ID of the module.
        /// </summary>  
        public int ID { get; init; }

        /// <summary>
        /// Determines if the module is official.
        /// </summary> 
        public bool Official { get; init; }

        /// <summary>
        /// Name of the module.
        /// </summary> 
        public string Name { get; init; }

        /// <summary>
        /// Damage value of the module.
        /// </summary> 
        public int Damage { get; init; }

        /// <summary>
        /// Aggression level of the module.
        /// </summary> 
        public int Agression { get; init; }

        /// <summary>
        /// Reproduction need of the module.
        /// </summary> 
        public int ReproductionNeed { get; init; }

        /// <summary>
        /// Maximum hunger of the module.
        /// </summary> 
        public int MaxHunger { get; init; }

        /// <summary>
        /// Maximum health of the module.
        /// </summary> 
        public int MaxHealth { get; init; }

        /// <summary>
        /// Type of the module.
        /// </summary> 
        public EntityTypeEnum Type { get; init; }

        /// <summary>
        /// ID for graphical representation of the module.
        /// </summary> 
        public int GraphicalRepresentationID { get; init; }

        /// <summary>
        /// Behaviours associated with the module.
        /// </summary> 
        private readonly Dictionary<Type, IBehaviour> behaviours;

        /// <summary>
        /// Creates a Module instance from a database entity.
        /// </summary> 
        /// <param name="dbEntity"> The database entity representing the module. </param>
        /// <returns> The created Module instance. </returns>
        public static Module CreateFromDBEntity(ModuleDBEntity dbEntity)
        {
            List<IBehaviour> behaviours = dbEntity.BehaviourIDs
                                        .Select(BehaviourService.Instance.GetBehaviourInstanceByID)
                                        .Where(b => b != null)
                                        .ToList();

            Module module = new Module.ModuleBuilder()
                                .WithID(dbEntity.ID)
                                .WithName(dbEntity.Name)
                                .IsOfficial(dbEntity.Official)
                                .OfType(dbEntity.Type)
                                .WithDamage(dbEntity.Damage)
                                .WithAgression(dbEntity.Agression)
                                .WithReproductionNeed(dbEntity.ReproductionNeed)
                                .WithMaxHunger(dbEntity.MaxHunger)
                                .WithMaxHealth(dbEntity.MaxHealth)
                                .WithGraphicsId(dbEntity.GraphicalRepresentationID)
                                .WithBehaviours(behaviours)
                                .Create();

            return module;
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="id"> ID of the module. </param>
        /// <param name="name"> Name of the module. </param>
        /// <param name="official"> Whether the module is official. </param>
        /// <param name="damage"> Damage value of the module. </param>
        /// <param name="aggresion"> Aggression level of the module. </param>
        /// <param name="reproductionNeed"> Reproduction need of the module. </param>
        /// <param name="maxHunger"> Maximum hunger of the module. </param>
        /// <param name="maxHelath"> Maximum health of the module. </param>
        /// <param name="type"> Type of the module. </param>
        /// <param name="graphicsId"> ID for graphical representation of the module. </param>
        private Module(int id, string name, bool official, int damage, int aggresion, int reproductionNeed, int maxHunger, int maxHelath, EntityTypeEnum type, int graphicsId)
        {
            ID = id;
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

        /// <summary>
        /// Adds a behaviour to the module.
        /// </summary> 
        /// <param name="behaviour"> The behaviour to add. </param>
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

        /// <summary>
        /// Retrieves a behaviour of the specified type from the module.
        /// </summary> 
        /// <param name="typeEnum"> The type of behaviour to retrieve. </param>
        public IBehaviour GetBehaviourOfType(InteractionTypeEnum typeEnum)
        {
            IBehaviour? found;
            switch (typeEnum)
            {
                case InteractionTypeEnum.None:
                    throw new BehaviourImplementationNotFoundException();

                case InteractionTypeEnum.Move:
                    behaviours.TryGetValue(typeof(MoveBehaviourBase), out found);
                    return found ?? throw new BehaviourImplementationNotFoundException();
                case InteractionTypeEnum.Attack:
                    behaviours.TryGetValue(typeof(AttackBehaviourBase), out found);
                    return found ?? throw new BehaviourImplementationNotFoundException();
                case InteractionTypeEnum.Eat:
                    behaviours.TryGetValue(typeof(EatBehaviourBase), out found);
                    return found ?? throw new BehaviourImplementationNotFoundException();
                case InteractionTypeEnum.Reproduce:
                    behaviours.TryGetValue(typeof(ReproduceBehaviourBase), out found);
                    return found ?? throw new BehaviourImplementationNotFoundException();
                default:
                    throw new BehaviourImplementationNotFoundException();
            }
        }

        /// <summary>
        /// Creates a DTO representation of the module.
        /// </summary> 
        /// <returns> The DTO representation of the module. </returns>
        public ModuleDTO ToDTO()
        {
            List<BehaviourDTO> behaviourDTOs = new List<BehaviourDTO>();
            foreach (var behaviour in behaviours.Values)
            {
                behaviourDTOs.Add(behaviour.ToDTO());
            }

            return new ModuleDTO(ID, Name, Official, Damage, Agression, ReproductionNeed, MaxHunger, MaxHealth, behaviourDTOs, Type, GraphicalRepresentationID);
        }

        #region BUILDER

        public class ModuleBuilder
        {
            public int id;
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
                id = -999;
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
                var module = new Module(id, name, official, damage, aggresion, reproductionNeed, maxHunger, maxHelath, type, graphicsId);
                foreach (var behaviour in behaviours)
                {
                    module.AddBehaviour(behaviour);
                }
                return module;
            }

            public ModuleBuilder WithID(int id)
            {
                this.id = id;
                return this;
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