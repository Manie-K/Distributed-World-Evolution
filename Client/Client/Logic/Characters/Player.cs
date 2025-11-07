using Client.Common;
using Client.Logic;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Server.Core;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.DTOs.ModuleDTO;
using System;
using System.Linq;

namespace Client
{
    public class Player : Character
    {
        private const float PLAYER_ACTION_COOLDOWN = 2.0f;

        public WorldEntityDTO TargetEntity;
        public WorldEntityDTO PlayerDTO;

        private Text playerName;
        private Vector2 playerNameOffset;
        private ModuleDTO playerModule;
        private readonly WorldMap map;
        private readonly ClientManager clientManager;
        private float actionCooldown;

        private enum InteractionType
        {
            Attack,
            Gather,
            Tame
        }

        public Player(Vector2 position, Color color, Text playerName, Vector2 spriteDrawingOffset, WorldMap map = null, ClientManager clientManager = null)
            : base(position, color, 130, 108, 150f, 8, 7)
        {
            this.playerName = playerName;
            SpriteDrawingOffset = spriteDrawingOffset;
            playerNameOffset = new Vector2(29 + SpriteDrawingOffset.X, -3 + SpriteDrawingOffset.Y);
            this.map = map;
            this.clientManager = clientManager;
            PlayerDTO = null;
            TargetEntity = null;
            am = new AnimationManager(13);
            playerModule = null;
            actionCooldown = 0;
        }

        public override void Update(GameTime gameTime, InputManager inputManager)
        {
            if (map == null)
            {
                base.Update(gameTime, inputManager);
            }
            else
            {
                UpdatePlayer(gameTime, inputManager);
            }
        }

        private void UpdatePlayer(GameTime gameTime, InputManager inputManager)
        {
            if (playerModule == null && PlayerDTO != null)
            {
                playerModule = clientManager.Modules.Where(m => m.DatabaseID == PlayerDTO.ModuleID).FirstOrDefault();
            }

            if (PlayerDTO != null && PlayerDTO.State.Health <= 0)
            {
                Position = Vector2.One * 5;
                am.Update();
                return;
            }

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (actionCooldown > 0)
            {
                actionCooldown -= delta;
            }
            Vector2 movement = Vector2.Zero;

            if (inputManager.CheckIfPressingKey(Keys.W))
            {
                CurrentDirection = Direction.up;
                movement.Y -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.S))
            {
                CurrentDirection = Direction.down;
                movement.Y += 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.A))
            {
                CurrentDirection = Direction.left;
                movement.X -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.D))
            {
                CurrentDirection = Direction.right;
                movement.X += 1;
            }

            SetAnimation(0);

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Vector2 newPosition = Position + (movement * speed * delta);
                SetAnimation(0);

                if (newPosition.X < 0) newPosition.X = 0;
                else if (newPosition.X >= map.MapWidth * map.TileSize) newPosition.X = map.MapWidth * map.TileSize - 1;

                if (newPosition.Y < 0) newPosition.Y = 0;
                else if (newPosition.Y >= map.MapHeight * map.TileSize) newPosition.Y = map.MapHeight * map.TileSize - 1;

                int tileID = map.GetTileIdAtPosition(newPosition.X, newPosition.Y);
                if (map.TilesetData.First(m => m.Id == tileID).Walkable)
                {
                    Position = newPosition;
                }
            }

            if (inputManager.CheckIfPressingKey(Keys.Space))
            {
                HandleInteraction(InteractionType.Attack);
                SetAnimation(1);
            }
            if (inputManager.CheckIfPressingKey(Keys.E))
            {
                HandleInteraction(InteractionType.Gather);
            }
            if (inputManager.CheckIfPressingKey(Keys.R))
            {
                HandleInteraction(InteractionType.Tame);
            }

            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 13), GetPosition(), GetSourceRectangle(), Color.White);
            playerName.Draw(spriteBatch, Position + playerNameOffset);
        }

        private void HandleInteraction(InteractionType interactionType)
        {
            if (actionCooldown > 0) return;

            TargetEntity = clientManager.Entities.FirstOrDefault(e => e.Value.State.Position.Equals(map.GetTilePosition2D(Position.X, Position.Y))).Value;
            if (TargetEntity != null && TargetEntity.ModuleID != PlayerDTO.ModuleID)
            {
                switch (interactionType)
                {
                    case InteractionType.Attack:
                        AttackTarget();
                        break;
                    case InteractionType.Gather:
                        GatherTarget();
                        break;
                    case InteractionType.Tame:
                        TameTarget();
                        break;
                }
            }
            actionCooldown = PLAYER_ACTION_COOLDOWN;
        }

        private void AttackTarget()
        {
            TargetEntity.State.Health -= playerModule.Damage;
            Console.WriteLine("hp left: " + TargetEntity.State.Health);
        }

        private void GatherTarget()
        {
            EntityTypeEnum type = clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().Type;
            if (type == EntityTypeEnum.Plant)
            {
                TargetEntity.State.Health = 0;
                //TODO: add to inventory targetentity
                Console.WriteLine("Plant gathered");
            }
        }

        private void TameTarget()
        {
            EntityTypeEnum type = clientManager.Modules.Where(m => m.DatabaseID == TargetEntity.ModuleID).First().Type;
            if (type == EntityTypeEnum.Animal) //TODO: check if player has needed plant or smth
            {
                TargetEntity.State.Health = 0;
                //TODO: tame animal
                Console.WriteLine("Animal tamed");
            }
        }
    }
}
