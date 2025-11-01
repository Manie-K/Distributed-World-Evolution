using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Plants
{
    public class PlantsAssetsManager
    {
        private static PlantsAssetsManager instance;
        private Texture2D spritesheet; 
        private PlantsAssetsManager() 
        {
            
        }

        public static PlantsAssetsManager GetInstance()
        {
            if (instance == null)
            {
                instance = new PlantsAssetsManager();
            }
            return instance;
        }
        public void Load(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>("Plants/Flowers_With_Outline_Spritesheet");
        }

        public Texture2D GetSpritesheet()
        {
            return spritesheet;
        }
    }
}
