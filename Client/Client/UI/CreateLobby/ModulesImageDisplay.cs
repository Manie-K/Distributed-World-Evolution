using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.CreateLobby
{
    public class ModulesImageDisplay
    {
        private Texture2D[] Images;
        private int AcctualPicked;
        private Rectangle rect;

        public ModulesImageDisplay(ContentManager contentManager)
        {
            rect = new Rectangle(583, 213, 66, 52);
            AcctualPicked = 1;

            InitializeImages(contentManager);
        }

        public void InitializeImages(ContentManager contentManager)
        {
            Images = new Texture2D[29];

            Images[0] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Plant1_Image");
            Images[1] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Plant2_Image");
            Images[2] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Pig_Image");
            Images[3] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Boar_Image");

            Images[4] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/White_Rabbit_Image");
            Images[5] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Rabbit_Brown_Image");
            Images[6] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Plant3_Image");
            Images[7] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Slime1_Image");

            Images[8] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Slime2_Image");
            Images[9] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Slime3_Image");
            Images[10] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Orc1_Image");
            Images[11] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Orc2_Image");

            Images[12] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Orc3_Image");
            Images[13] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Vampire1_Image");
            Images[14] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Vampire2_Image");
            Images[15] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Vampire3_Image");

            Images[16] = Images[15]; //DELETE
            //Images[16] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Player_Image");

            Images[17] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Cosmo_Image");
            Images[18] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Daffodil_Image");
            Images[19] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Daisy_Image");
            Images[20] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Lavender_Image");

            Images[21] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Lily_Image");
            Images[22] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/LilyOfTheValley_Image");
            Images[23] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Orchid_Image");
            Images[24] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Pansy_Image");

            Images[25] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Poppy_Image");
            Images[26] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Rose_Image");
            Images[27] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Sunflower_Image");
            Images[28] = contentManager.Load<Texture2D>("UI/CreateLobby/Modules_Images/Tulip_Image");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Images[AcctualPicked], rect, Color.White);
        }

        public void SetImage(int index)
        {
            AcctualPicked = index;
        }
    }
}
