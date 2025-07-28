using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AnimationTexturesLoader
    {
        private List<AnimationTexture[]> AnimationsStore;

        public AnimationTexturesLoader(ContentManager content)
        {
            AnimationsStore = new List<AnimationTexture[]>();

            LoadAllAnimations(content);
        }

        private void LoadAllAnimations(ContentManager content)
        {
            LoadPlayer(content);
        }

        private void LoadPlayer(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_left"), new Vector2(130, 108), 8, 8, 0, 0),
            ];

            AnimationsStore.Add(IdleTextures);


            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_left"), new Vector2(130, 108), 8, 8, 0, 0),
            ];

            AnimationsStore.Add(RunTextures);

            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_left"), new Vector2(130, 108), 8, 8, 0, 0),
            ];

            AnimationsStore.Add(AttackTextures);

        }

        public AnimationTexture GetTexture(int AnimaionIndex, int DirectionIndex)
        {
            return AnimationsStore[AnimaionIndex][DirectionIndex];
        }


    }
}
