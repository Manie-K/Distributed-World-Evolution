using Client.Common;
using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.DTOs.EntitiesDTO;
using System.Linq;

namespace Client
{
    public class Player : Character
    {
        public WorldEntityDTO TargetEntity;

        private Text playerName;
        private Vector2 playerNameOffset;
        private readonly WorldMap map;
        private readonly ClientManager clientManager;

        public Player(Vector2 position, Color color, Text playerName, ref AnimationTexturesLoader ATL, WorldMap map = null, ClientManager clientManager = null)
            : base(position, color, 140, 108, 150f, ref ATL, 0, new Vector2(-68, -77))
        {
            this.playerName = playerName;
            playerNameOffset = new Vector2(-33, -80);
            this.map = map;
            this.clientManager = clientManager;
            TargetEntity = null;
        }

        public override void Update(GameTime gameTime, InputManager inputManager)
        {
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
                TargetEntity = clientManager.Entities.FirstOrDefault(e => e.Value.State.Position.Equals(map.GetTilePosition2D(Position.X, Position.Y))).Value;
                if (TargetEntity != null)
                {
                    TargetEntity.State.Health -= 1;
                }

                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);               
            }


            if (am.GetAcctualAnimationIndex() == 2)
            {
                speed = 70f;
                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);
            }
            else speed = 200f;
            

            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(am.GetAcctualTexture(), Rect, am.GetFrame(), Color.White);
            playerName.Draw(spriteBatch, Position + playerNameOffset);
        }
    }
}
