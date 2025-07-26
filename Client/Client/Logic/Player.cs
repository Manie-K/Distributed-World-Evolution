using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{
   
    public class Player : ColoredSprite
    {
        private float speed = 200f; 
        private Text text;
        private AnimationManager am;
        private Dictionary<string, Animation> animations;
        private string currentAnimation;

        public Player(Texture2D texture, Vector2 position, Color color,Text text)
            : base(texture, position, color)

        {
            this.text = text;
            animations = new Dictionary<string, Animation>()
            {
                { "idle", new Animation(6, 6, new Vector2(69, 44), 0, 0) },
                { "walk", new Animation(8, 6, new Vector2(69, 44), 1, 0) },
                { "attack", new Animation(12, 6, new Vector2(69, 44), 2, 3) }
            };

            currentAnimation = "idle";
            am = new AnimationManager(animations[currentAnimation]);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 movement = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (keyState.IsKeyDown(Keys.S)) movement.Y += 1;
            if (keyState.IsKeyDown(Keys.A)) movement.X -= 1;
            if (keyState.IsKeyDown(Keys.D)) movement.X += 1;

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                position += movement * speed * delta;
                ChangeAnimation("walk");
            }
            else
            {
                ChangeAnimation("idle");
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                ChangeAnimation("attack");
            }

            am.Update();

           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Rect, color);
            spriteBatch.Draw(texture, Rect, am.GetFrame(), Color.White);
            text.Draw(spriteBatch);
        }

        private void ChangeAnimation(string animationName)
        {
            if (currentAnimation == animationName)
                return;

            currentAnimation = animationName;
            am.SetAnimation(animations[currentAnimation]);
        }
    }
}
