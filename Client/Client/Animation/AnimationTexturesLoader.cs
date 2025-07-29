using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            LoadPlant1(content);
            LoadPig(content);
            LoadBoar(content);
            LoadWhiteRabbit(content);
            LoadBrownRabbit(content);
        }

        //0-2
        private void LoadPlayer(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore.Add(IdleTextures);


            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore.Add(RunTextures);

            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore.Add(AttackTextures);

        }

        //3-6
        private void LoadPlant1(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 2, 0)
            ];

            AnimationsStore.Add(AttackTextures);

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore.Add(DeathTextures);


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore.Add(IdleTextures);

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore.Add(RunTextures);
        }

        //7-8
        private void LoadPig(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 1, 0)
            ];

            AnimationsStore.Add(IdleTextures);

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 1, 0)
            ];

            AnimationsStore.Add(RunTextures);
        }

        //9-11
        private void LoadBoar(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 1, 0)
            ];

            AnimationsStore.Add(IdleTextures);

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 1, 0)
            ];

            AnimationsStore.Add(RunTextures);

            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 1, 0)
            ];

            AnimationsStore.Add(AttackTextures);
        }

        //12-13
        private void LoadWhiteRabbit(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 1, 0)
            ];

            AnimationsStore.Add(IdleTextures);

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 1, 0)
            ];

            AnimationsStore.Add(RunTextures);
        }

        //14-15
        private void LoadBrownRabbit(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 1, 0)
            ];

            AnimationsStore.Add(IdleTextures);

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 1, 0)
            ];

            AnimationsStore.Add(RunTextures);
        }

        public AnimationTexture GetTexture(int AnimaionIndex, int DirectionIndex)
        {
            return AnimationsStore[AnimaionIndex][DirectionIndex];
        }


    }
}
