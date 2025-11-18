using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class AssetsManager
    {
        private static AssetsManager instance;
        private Texture2D FlowersSpritesheet;
        private Texture2D[] CharactersIdleSpritesheet;
        private Texture2D[] CharactersAttackSpritesheet;
        private Texture2D[] CharactersDeathSpritesheet;
        private Texture2D[] AnimalsHealthBarTextures;
        private AssetsManager()
        {

        }

        public static AssetsManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetsManager();
            }
            return instance;
        }
        public void Load(ContentManager content)
        {
            FlowersSpritesheet = content.Load<Texture2D>("Plants/Flowers_With_Outline_Spritesheet");
            LoadCharacters(content);
        }

        private void LoadCharacters(ContentManager content)
        {
            CharactersIdleSpritesheet = new Texture2D[17];

            CharactersIdleSpritesheet[0] = content.Load<Texture2D>("Animations/Orc1/orc1_idle_full");
            CharactersIdleSpritesheet[1] = content.Load<Texture2D>("Animations/Orc2/orc2_idle_full");
            CharactersIdleSpritesheet[2] = content.Load<Texture2D>("Animations/Orc3/orc3_idle_full");
            CharactersIdleSpritesheet[3] = content.Load<Texture2D>("Animations/Plant1/Plant1_Idle_full");

            CharactersIdleSpritesheet[4] = content.Load<Texture2D>("Animations/Plant2/Plant2_Idle_full");
            CharactersIdleSpritesheet[5] = content.Load<Texture2D>("Animations/Plant3/Plant3_Idle_full");
            CharactersIdleSpritesheet[6] = content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full");
            CharactersIdleSpritesheet[7] = content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full");

            CharactersIdleSpritesheet[8] = content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full");
            CharactersIdleSpritesheet[9] = content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full");
            CharactersIdleSpritesheet[10] = content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full");
            CharactersIdleSpritesheet[11] = content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full");

            CharactersIdleSpritesheet[12] = content.Load<Texture2D>("Animations/Boar/Boar_Idle");
            CharactersIdleSpritesheet[13] = content.Load<Texture2D>("Animations/Player/Player_Idle");
            CharactersIdleSpritesheet[14] = content.Load<Texture2D>("Animations/Pig/Pig_Idle");
            CharactersIdleSpritesheet[15] = content.Load<Texture2D>("Animations/RabbitBrown/Rabbit_Brown_Idle");

            CharactersIdleSpritesheet[16] = content.Load<Texture2D>("Animations/RabbitWhite/Rabbit_Horned_Idle");

            CharactersAttackSpritesheet = new Texture2D[14];

            CharactersAttackSpritesheet[0] = content.Load<Texture2D>("Animations/Orc1/orc1_attack_full");
            CharactersAttackSpritesheet[1] = content.Load<Texture2D>("Animations/Orc2/orc2_attack_full");
            CharactersAttackSpritesheet[2] = content.Load<Texture2D>("Animations/Orc3/orc3_attack_full");
            CharactersAttackSpritesheet[3] = content.Load<Texture2D>("Animations/Plant1/Plant1_Attack_full");

            CharactersAttackSpritesheet[4] = content.Load<Texture2D>("Animations/Plant2/Plant2_Attack_full");
            CharactersAttackSpritesheet[5] = content.Load<Texture2D>("Animations/Plant3/Plant3_Attack_full");
            CharactersAttackSpritesheet[6] = content.Load<Texture2D>("Animations/Slime1/Slime1_Attack_full");
            CharactersAttackSpritesheet[7] = content.Load<Texture2D>("Animations/Slime2/Slime2_Attack_full");

            CharactersAttackSpritesheet[8] = content.Load<Texture2D>("Animations/Slime3/Slime3_Attack_full");
            CharactersAttackSpritesheet[9] = content.Load<Texture2D>("Animations/Vampire1/Vampires1_Attack_full");
            CharactersAttackSpritesheet[10] = content.Load<Texture2D>("Animations/Vampire2/Vampires2_Attack_full");
            CharactersAttackSpritesheet[11] = content.Load<Texture2D>("Animations/Vampire3/Vampires3_Attack_full");

            CharactersAttackSpritesheet[12] = content.Load<Texture2D>("Animations/Boar/Boar_Attack");
            CharactersAttackSpritesheet[13] = content.Load<Texture2D>("Animations/Player/Player_Attack");

            CharactersDeathSpritesheet = new Texture2D[12];

            CharactersDeathSpritesheet[0] = content.Load<Texture2D>("Animations/Orc1/orc1_death_full");
            CharactersDeathSpritesheet[1] = content.Load<Texture2D>("Animations/Orc2/orc2_death_full");
            CharactersDeathSpritesheet[2] = content.Load<Texture2D>("Animations/Orc3/orc3_death_full");
            CharactersDeathSpritesheet[3] = content.Load<Texture2D>("Animations/Plant1/Plant1_Death_full");

            CharactersDeathSpritesheet[4] = content.Load<Texture2D>("Animations/Plant2/Plant2_Death_full");
            CharactersDeathSpritesheet[5] = content.Load<Texture2D>("Animations/Plant3/Plant3_Death_full");
            CharactersDeathSpritesheet[6] = content.Load<Texture2D>("Animations/Slime1/Slime1_Death_full");
            CharactersDeathSpritesheet[7] = content.Load<Texture2D>("Animations/Slime2/Slime2_Death_full");

            CharactersDeathSpritesheet[8] = content.Load<Texture2D>("Animations/Slime3/Slime3_Death_full");
            CharactersDeathSpritesheet[9] = content.Load<Texture2D>("Animations/Vampire1/Vampires1_Death_full");
            CharactersDeathSpritesheet[10] = content.Load<Texture2D>("Animations/Vampire2/Vampires2_Death_full");
            CharactersDeathSpritesheet[11] = content.Load<Texture2D>("Animations/Vampire3/Vampires3_Death_full");

            AnimalsHealthBarTextures = new Texture2D[2];

            AnimalsHealthBarTextures[0] = content.Load<Texture2D>("Animations/AnimalsHealth/Animals_Bar_Frame");
            AnimalsHealthBarTextures[1] = content.Load<Texture2D>("Animations/AnimalsHealth/Animals_Bar_RedArea");
        }

        public Texture2D GetFlowersSpritesheet()
        {
            return FlowersSpritesheet;
        }

        public Texture2D GetCharacterTexture(int type,int index)
        {
            switch (type)
            {
                case 0:
                    return CharactersIdleSpritesheet[index];
                case 1:
                    return CharactersAttackSpritesheet[index];
                case 2:
                    return CharactersDeathSpritesheet[index];
                default:
                    return null;
            }
        }

        public Texture2D GetHealthBarTexture(int index)
        {
            switch (index)
            {
                case 0:
                    return AnimalsHealthBarTextures[0];
                case 1:
                    return AnimalsHealthBarTextures[1];
                default:
                    return null;
            }
        }
    }
}
