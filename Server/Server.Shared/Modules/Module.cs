using Server.Shared.Exceptions;
using Server.Shared.Helpers;

namespace Server.Shared.Modules
{
    public class Module
    {
        // We should try to place non-dynamic data here. All the dynamic data will be stored in WorldEntity object instances.
        public string Name { get; init; }
        public string Version { get; init; }
        public string Author { get; init; }
        public object Stats { get; init; } //TODO: Define a proper structure for stats.


        private readonly Dictionary<Type, List<IBehaviour>> behaviours;

        private Module(string name, string version, string author, object stats)
        {
            Name = name;
            Version = version;
            Author = author;
            Stats = stats;

            behaviours = new Dictionary<Type, List<IBehaviour>>();
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
            if (!behaviours.ContainsKey(type))
            {
                behaviours[type] = new List<IBehaviour>();
            }

            behaviours[type].Add(behaviour);
        }

        public IEnumerable<IBehaviour> GetBehavioursOfType(Type type)
        {
            if(!type.IsInterface || typeof(IBehaviour).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must be an interface that extends IBehaviour.");
            }

            behaviours.TryGetValue(type, out List<IBehaviour>? found);
            return found ?? throw new BehaviourImplementationNotFoundException();
        }


        #region BUILDER

        public class ModuleBuilder
        {
            private string name;
            private string version;
            private string author;
            private object stats;
            private List<IBehaviour> behaviours;

            public ModuleBuilder()
            {
                name = "Name";
                version = "0.0.1";
                author = "Unknown";
                stats = new { };
                behaviours = new List<IBehaviour>();
            }

            public Module Create()
            {
                var module = new Module(name, version, author, stats);
                foreach (var behaviour in behaviours)
                {
                    module.AddBehaviour(behaviour);
                }
                return module;
            }

            public ModuleBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }
            public ModuleBuilder WithVersion(string version)
            {
                this.version = version;
                return this;
            }

            public ModuleBuilder WithAuthor(string author)
            {
                this.author = author;
                return this;
            }

            public ModuleBuilder WithStats(object stats)
            {
                this.stats = stats;
                return this;
            }

            public ModuleBuilder WithBehaviour(IBehaviour behaviour)
            {
                behaviours.Add(behaviour);
                return this;
            }

            public ModuleBuilder WithBehaviours(IEnumerable<IBehaviour> behaviours)
            {
                this.behaviours.AddRange(behaviours);
                return this;
            }
        }

        #endregion
    
    }
}