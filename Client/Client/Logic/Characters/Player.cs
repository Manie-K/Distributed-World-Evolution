using Client.Common;
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
        public WorldEntityDTO TargetEntity;
        public WorldEntityDTO PlayerDTO;

        private Text playerName;
        private Vector2 playerNameOffset;
        private ModuleDTO playerModule;
        private readonly WorldMap map;
        private readonly ClientManager clientManager;

        private enum InteractionType
        {
            Attack,
            Gather,
            Tame
        }

        public Player(Vector2 position, Color color, Text playerName, ref AnimationTexturesLoader ATL, Vector2 spriteDrawingOffset, WorldMap map = null, ClientManager clientManager = null)
            : base(position, color, 140, 108, 150f, ref ATL, 0, spriteDrawingOffset)
        {
            this.playerName = playerName;
            playerNameOffset = new Vector2(35 + spriteDrawingOffset.X, -3 + spriteDrawingOffset.Y);
            this.map = map;
            this.clientManager = clientManager;
            PlayerDTO = null;
            TargetEntity = null;
            playerModule = null;
        }

        public override void Update(GameTime gameTime, InputManager inputManager)
        {
            if (playerModule == null && PlayerDTO != null)
            { 
                playerModule = clientManager.Modules.Where(m => m.DatabaseID == PlayerDTO.ModuleID).FirstOrDefault();
            }

            if (PlayerDTO != null && PlayerDTO.State.Health <= 0)
            {
                Position = Vector2.One * 5;
                UpdateAnimation();
                return;
            }

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Vector2 newPosition = Position + (movement * speed * delta);

                if (newPosition.X < 0) newPosition.X = 0;
                else if (newPosition.X >= map.MapWidth * map.TileSize) newPosition.X = map.MapWidth * map.TileSize - 1;

                if (newPosition.Y < 0) newPosition.Y = 0;
                else if (newPosition.Y >= map.MapHeight * map.TileSize) newPosition.Y = map.MapHeight * map.TileSize - 1;

                int tileID = map.GetTileIdAtPosition(newPosition.X, newPosition.Y);
                if (map.TilesetData.First(m => m.Id == tileID).Walkable)
                {
                    Position = newPosition;
                }

                am.SetAnimationWithDuration(1, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(0, CurrentDirection, 1, 36);
            }

            if (inputManager.CheckIfPressingKey(Keys.Space) && am.GetAcctualAnimationIndex() != 2)
            {
                HandleInteraction(InteractionType.Attack);
                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);               
            }
            if (inputManager.CheckIfPressingKey(Keys.E))
            {
                HandleInteraction(InteractionType.Gather);
            }
            if (inputManager.CheckIfPressingKey(Keys.R))
            {
                HandleInteraction(InteractionType.Tame);
            }

            UpdateAnimation();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(am.GetAcctualTexture(), Rect, am.GetFrame(), Color.White);
            playerName.Draw(spriteBatch, Position + playerNameOffset);
        }

        private void UpdateAnimation()
        {
            if (am.GetAcctualAnimationIndex() == 2)
            {
                speed = 70f;
                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);
            }
            else speed = 200f;

            am.Update();
        }

        private void HandleInteraction(InteractionType interactionType)
        {
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
