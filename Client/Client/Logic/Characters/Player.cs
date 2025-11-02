using Client.Common;
using Client.Logic;
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

        public Player(Vector2 position, Color color, Text playerName, WorldMap map = null, ClientManager clientManager = null)
            : base(position, color, 130, 108, 150f, 8, 7)
        {
            this.playerName = playerName;
            playerNameOffset = new Vector2(35 + SpriteDrawingOffset.X, -3 + SpriteDrawingOffset.Y);
            this.map = map;
            this.clientManager = clientManager;
            TargetEntity = null;
            am = new AnimationManager(13);
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
                SetNewAnimation(0);

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
                SetNewAnimation(1);

                TargetEntity = clientManager.Entities.FirstOrDefault(e => e.Value.State.Position.Equals(map.GetTilePosition2D(Position.X, Position.Y))).Value;
                if (TargetEntity != null)
                {
                    TargetEntity.State.Health -= 1;
                }        
            }

            if (inputManager.CheckIfPressingKey(Keys.O))
            {
                SetNewAnimation(2);
            }
            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 13), GetPosition(), GetSourceRectangle(), Color.White);
            playerName.Draw(spriteBatch, Position + playerNameOffset);
        }
    }
}
