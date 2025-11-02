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
        private Texture2D[] CharactersSpritesheet;
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
            CharactersSpritesheet = new Texture2D[17];

            CharactersSpritesheet[0] = content.Load<Texture2D>("Animations/Player/Player_Idle");
            CharactersSpritesheet[1] = content.Load<Texture2D>("Animations/Boar/Boar_Idle");
            CharactersSpritesheet[2] = content.Load<Texture2D>("Animations/Orc1/orc1_idle_full");
            CharactersSpritesheet[3] = content.Load<Texture2D>("Animations/Orc2/orc2_idle_full");

            CharactersSpritesheet[4] = content.Load<Texture2D>("Animations/Orc3/orc3_idle_full");
            CharactersSpritesheet[5] = content.Load<Texture2D>("Animations/Pig/Pig_Idle");
            CharactersSpritesheet[6] = content.Load<Texture2D>("Animations/Plant1/Plant1_Idle_full");
            CharactersSpritesheet[7] = content.Load<Texture2D>("Animations/Plant2/Plant2_Idle_full");

            CharactersSpritesheet[8] = content.Load<Texture2D>("Animations/Plant3/Plant3_Idle_full");
            CharactersSpritesheet[9] = content.Load<Texture2D>("Animations/RabbitBrown/Rabbit_Brown_Idle");
            CharactersSpritesheet[10] = content.Load<Texture2D>("Animations/RabbitWhite/Rabbit_Horned_Idle");
            CharactersSpritesheet[11] = content.Load<Texture2D>("Animations/Slime1/Slime1_Idle_full");

            CharactersSpritesheet[12] = content.Load<Texture2D>("Animations/Slime2/Slime2_Idle_full");
            CharactersSpritesheet[13] = content.Load<Texture2D>("Animations/Slime3/Slime3_Idle_full");
            CharactersSpritesheet[14] = content.Load<Texture2D>("Animations/Vampire1/Vampires1_Idle_full");
            CharactersSpritesheet[15] = content.Load<Texture2D>("Animations/Vampire2/Vampires2_Idle_full");

            CharactersSpritesheet[16] = content.Load<Texture2D>("Animations/Vampire3/Vampires3_Idle_full");
        }

        public Texture2D GetFlowersSpritesheet()
        {
            return FlowersSpritesheet;
        }

        public Texture2D GetCharacterTexture(int index)
        {
            return CharactersSpritesheet[index];
        }
    }
}
