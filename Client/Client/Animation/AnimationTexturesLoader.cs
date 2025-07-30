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
        private AnimationTexture[][] AnimationsStore;
        private int AnimationsAmount;

        public AnimationTexturesLoader(ContentManager content)
        {
            AnimationsAmount = 60;
            AnimationsStore = new AnimationTexture[AnimationsAmount][];

            for (int i = 0; i < AnimationsAmount; i++)
            {
                AnimationsStore[i] = new AnimationTexture[4];                                                                                                                             
            }

            LoadAllAnimations(content);
        }

        private void LoadAllAnimations(ContentManager content)
        {
            LoadPlayer(content);
            LoadEnemyPlant1(content);
            LoadPig(content);
            LoadBoar(content);
            LoadWhiteRabbit(content);
            LoadBrownRabbit(content);
            LoadEnemyPlant2(content);
            LoadEnemyPlant3(content);
            LoadSlime1(content);
            LoadSlime2(content);
            LoadSlime3(content);
            LoadOrc1(content);
            LoadOrc2(content);
            LoadOrc3(content);
            LoadVampire1(content);
            LoadVampire2(content);
            LoadVampire3(content);
        }


        private void LoadPlayer(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Idle/idle_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore[0] = IdleTextures;


            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Run/run_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore[1] = RunTextures;

            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_up"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_right"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_down"), new Vector2(130, 108), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Player/Attack/attack1_left"), new Vector2(130, 108), 8, 8, 0, 0)
            ];

            AnimationsStore[2] = AttackTextures;

        }


        private void LoadEnemyPlant1(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Attack_full"), new Vector2(100, 100), 7, 7, 2, 0)
            ];

            AnimationsStore[3] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[4] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[5] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant1/Plant1_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[6] = RunTextures;
        }


        private void LoadPig(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Idle"), new Vector2(173, 173), 4, 4, 1, 0)
            ];

            AnimationsStore[7] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Pig/Pig_Move"), new Vector2(173, 173), 6, 6, 1, 0)
            ];

            AnimationsStore[8] = RunTextures;
        }


        private void LoadBoar(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Idle"), new Vector2(160, 160), 4, 4, 1, 0)
            ];

            AnimationsStore[9] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Move"), new Vector2(160, 160), 6, 6, 1, 0)
            ];

            AnimationsStore[10] = RunTextures;

            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Boar/Boar_Attack"), new Vector2(160, 160), 6, 6, 1, 0)
            ];

            AnimationsStore[11] = AttackTextures;
        }


        private void LoadWhiteRabbit(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Idle"), new Vector2(192, 192), 4, 4, 1, 0)
            ];

            AnimationsStore[12] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/WhiteRabbit/Rabbit_Horned_Move"), new Vector2(192, 192), 6, 6, 1, 0)
            ];

            AnimationsStore[13] = RunTextures;
        }


        private void LoadBrownRabbit(ContentManager content)
        {
            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Idle"), new Vector2(192, 192), 4, 4, 1, 0)
            ];

            AnimationsStore[14] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 2, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/BrownRabbit/Rabbit_Brown_Move"), new Vector2(192, 192), 6, 6, 1, 0)
            ];

            AnimationsStore[15] = RunTextures;
        }


        private void LoadEnemyPlant2(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Attack_full"), new Vector2(100, 100), 7, 7, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Attack_full"), new Vector2(100, 100), 7, 7, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Attack_full"), new Vector2(100, 100), 7, 7, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Attack_full"), new Vector2(100, 100), 7, 7, 2, 0)
            ];

            AnimationsStore[16] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[17] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[18] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant2/Plant2_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[19] = RunTextures;
        }


        private void LoadEnemyPlant3(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Attack_full"), new Vector2(100, 100), 7, 7, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Attack_full"), new Vector2(100, 100), 7, 7, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Attack_full"), new Vector2(100, 100), 7, 7, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Attack_full"), new Vector2(100, 100), 7, 7, 2, 0)
            ];

            AnimationsStore[20] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[21] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[22] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/EnemyPlant3/Plant3_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[23] = RunTextures;
        }

        private void LoadSlime1(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Attack_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Attack_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Attack_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Attack_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[24] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[25] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full"), new Vector2(100, 100), 6, 6, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full"), new Vector2(100, 100), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full"), new Vector2(100, 100), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full"), new Vector2(100, 100), 6, 6, 2, 0)
            ];

            AnimationsStore[26] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime1/Slime1_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[27] = RunTextures;
        }

        private void LoadSlime2(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Attack_full"), new Vector2(100, 100), 11, 11, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Attack_full"), new Vector2(100, 100), 11, 11, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Attack_full"), new Vector2(100, 100), 11, 11, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Attack_full"), new Vector2(100, 100), 11, 11, 2, 0)
            ];

            AnimationsStore[28] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[29] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full"), new Vector2(100, 100), 6, 6, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full"), new Vector2(100, 100), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full"), new Vector2(100, 100), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full"), new Vector2(100, 100), 6, 6, 2, 0)
            ];

            AnimationsStore[30] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime2/Slime2_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[31] = RunTextures;
        }

        private void LoadSlime3(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Attack_full"), new Vector2(100, 100), 9, 9, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Attack_full"), new Vector2(100, 100), 9, 9, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Attack_full"), new Vector2(100, 100), 9, 9, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Attack_full"), new Vector2(100, 100), 9, 9, 2, 0)
            ];

            AnimationsStore[32] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Death_full"), new Vector2(100, 100), 10, 10, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Death_full"), new Vector2(100, 100), 10, 10, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Death_full"), new Vector2(100, 100), 10, 10, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Death_full"), new Vector2(100, 100), 10, 10, 2, 0)
            ];

            AnimationsStore[33] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full"), new Vector2(100, 100), 6, 6, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full"), new Vector2(100, 100), 6, 6, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full"), new Vector2(100, 100), 6, 6, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full"), new Vector2(100, 100), 6, 6, 2, 0)
            ];

            AnimationsStore[34] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Slime3/Slime3_Run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[35] = RunTextures;
        }

        private void LoadOrc1(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_attack_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_attack_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_attack_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_attack_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[36] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_death_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_death_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_death_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_death_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[37] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[38] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc1/orc1_run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[39] = RunTextures;
        }

        private void LoadOrc2(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_attack_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_attack_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_attack_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_attack_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[40] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_death_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_death_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_death_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_death_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[41] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[42] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc2/orc2_run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[43] = RunTextures;
        }

        private void LoadOrc3(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_attack_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_attack_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_attack_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_attack_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[44] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_death_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_death_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_death_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_death_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[45] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_idle_full"), new Vector2(100, 100), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_idle_full"), new Vector2(100, 100), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_idle_full"), new Vector2(100, 100), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_idle_full"), new Vector2(100, 100), 4, 4, 2, 0)
            ];

            AnimationsStore[46] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_run_full"), new Vector2(100, 100), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_run_full"), new Vector2(100, 100), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_run_full"), new Vector2(100, 100), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Orc3/orc3_run_full"), new Vector2(100, 100), 8, 8, 2, 0)
            ];

            AnimationsStore[47] = RunTextures;
        }

        private void LoadVampire1(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Attack_full"), new Vector2(110, 110), 12, 12, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Attack_full"), new Vector2(110, 110), 12, 12, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Attack_full"), new Vector2(110, 110), 12, 12, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Attack_full"), new Vector2(110, 110), 12, 12, 2, 0)
            ];

            AnimationsStore[48] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Death_full"), new Vector2(110, 110), 11, 11, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Death_full"), new Vector2(110, 110), 11, 11, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Death_full"), new Vector2(110, 110), 11, 11, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Death_full"), new Vector2(110, 110), 11, 11, 2, 0)
            ];

            AnimationsStore[49] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full"), new Vector2(110, 110), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full"), new Vector2(110, 110), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full"), new Vector2(110, 110), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full"), new Vector2(110, 110), 4, 4, 2, 0)
            ];

            AnimationsStore[50] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Run_full"), new Vector2(110, 110), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Run_full"), new Vector2(110, 110), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Run_full"), new Vector2(110, 110), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire1/Vampires1_Run_full"), new Vector2(110, 110), 8, 8, 2, 0)
            ];

            AnimationsStore[51] = RunTextures;
        }

        private void LoadVampire2(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Attack_full"), new Vector2(110, 110), 12, 12, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Attack_full"), new Vector2(110, 110), 12, 12, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Attack_full"), new Vector2(110, 110), 12, 12, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Attack_full"), new Vector2(110, 110), 12, 12, 2, 0)
            ];

            AnimationsStore[52] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Death_full"), new Vector2(110, 110), 11, 11, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Death_full"), new Vector2(110, 110), 11, 11, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Death_full"), new Vector2(110, 110), 11, 11, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Death_full"), new Vector2(110, 110), 11, 11, 2, 0)
            ];

            AnimationsStore[53] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full"), new Vector2(110, 110), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full"), new Vector2(110, 110), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full"), new Vector2(110, 110), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full"), new Vector2(110, 110), 4, 4, 2, 0)
            ];

            AnimationsStore[54] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Run_full"), new Vector2(110, 110), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Run_full"), new Vector2(110, 110), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Run_full"), new Vector2(110, 110), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire2/Vampires2_Run_full"), new Vector2(110, 110), 8, 8, 2, 0)
            ];

            AnimationsStore[55] = RunTextures;
        }

        private void LoadVampire3(ContentManager content)
        {
            AnimationTexture[] AttackTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Attack_full"), new Vector2(110, 110), 12, 12, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Attack_full"), new Vector2(110, 110), 12, 12, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Attack_full"), new Vector2(110, 110), 12, 12, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Attack_full"), new Vector2(110, 110), 12, 12, 2, 0)
            ];

            AnimationsStore[56] = AttackTextures;

            AnimationTexture[] DeathTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Death_full"), new Vector2(110, 110), 11, 11, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Death_full"), new Vector2(110, 110), 11, 11, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Death_full"), new Vector2(110, 110), 11, 11, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Death_full"), new Vector2(110, 110), 11, 11, 2, 0)
            ];

            AnimationsStore[57] = DeathTextures;


            AnimationTexture[] IdleTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full"), new Vector2(110, 110), 4, 4, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full"), new Vector2(110, 110), 4, 4, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full"), new Vector2(110, 110), 4, 4, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full"), new Vector2(110, 110), 4, 4, 2, 0)
            ];

            AnimationsStore[58] = IdleTextures;

            AnimationTexture[] RunTextures =
            [
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Run_full"), new Vector2(110, 110), 8, 8, 1, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Run_full"), new Vector2(110, 110), 8, 8, 3, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Run_full"), new Vector2(110, 110), 8, 8, 0, 0),
                new AnimationTexture(content.Load<Texture2D>("Animations/Vampire3/Vampires3_Run_full"), new Vector2(110, 110), 8, 8, 2, 0)
            ];

            AnimationsStore[59] = RunTextures;
        }

        public AnimationTexture GetTexture(int AnimaionIndex, int DirectionIndex)
        {
            return AnimationsStore[AnimaionIndex][DirectionIndex];
        }


    }
}
