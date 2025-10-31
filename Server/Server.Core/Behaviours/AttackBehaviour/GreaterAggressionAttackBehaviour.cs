using Server.Core.Exceptions;
using Server.Core.Helpers;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;

namespace Server.Core.Behaviours.AttackBehaviour
{
    /// <inheritdoc/>
    public class GreaterAggressionAttackBehaviour : AttackBehaviourBase
    {
        /// <inheritdoc/>
        public override int DatabaseID => 204;

        /// <inheritdoc/>
        public override string Description => "Attack only when the aggresion is greater than target's. Gives Damage to target, doesn't take any";

        /// <inheritdoc/>
        public override void Execute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            try
            {
                Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");

                target.State.Health -= attackerModule.Damage;
                if (target.State.Health <= 0)
                {

                    ILobby lobby = otherParams != null && otherParams.TryGetValue(CustomBehaviourParams.LOBBY_PARAM, out object? lobbyObj)
                        && lobbyObj is ILobby l ? l :
                            throw new ArgumentNullException("Lobby parameter is required for GreaterAggressionAttackBehaviour");

                    target.Die(lobby);
                }
            }
            catch (ModuleNotFoundException ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public override bool CanExecute(WorldEntity attacker, WorldEntity target, IModuleService moduleService, Dictionary<string, object>? otherParams = null)
        {
            Module attackerModule = moduleService.GetModuleById(attacker.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            Module targetModule = moduleService.GetModuleById(target.ModuleID) ?? throw new Exception($"Module with ID={attacker.ModuleID} not found!");
            return attacker.Id != target.Id && attackerModule.Agression > targetModule.Agression;
        }
    }
}
