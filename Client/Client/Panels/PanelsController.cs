using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Panels
{
    public class PanelsController
    {
        private GameManager gameManager;
        public BestiaryPanel BestiaryPanel { get; set; }
        public EscPanel EscPanel { get; set; }
        private int pickedPanel;
        private bool isBlocked;

        public PanelsController(GameManager gameManager) 
        {
            this.gameManager = gameManager;
            BestiaryPanel = new BestiaryPanel(gameManager);
            EscPanel = new EscPanel(gameManager);
            pickedPanel = -1;
            isBlocked = false;
        }

        public void Update()
        {
            isBlocked = false;
            if (gameManager.InputManager.CheckIfLeftClick())
            {
                switch (pickedPanel)
                {
                    case 0:
                        if (EscPanel.CheckLeftClick(gameManager.InputManager.GetMousePosition())) pickedPanel = -1;
                        break;
                    case 1:
                        if (BestiaryPanel.CheckLeftClick(gameManager.InputManager.GetMousePosition())) pickedPanel = -1;
                        break;
                }
            }

            switch (pickedPanel)
            {
                case 0:
                    if (EscPanel.Update(gameManager.InputManager.GetMousePosition()))
                    {
                        pickedPanel = -1;
                        isBlocked = true;
                    }
                    break;
                case 1:
                    if (BestiaryPanel.Update(gameManager.InputManager.GetMousePosition()))
                    {
                        pickedPanel = -1;
                        isBlocked = true;
                    }
                    break;
            }

            if (!isBlocked)
            {
                if (pickedPanel == -1 && gameManager.InputManager.CheckIfCanPressKey(Keys.Escape))
                {
                    pickedPanel = 0;
                }
                else if (pickedPanel == -1 && gameManager.InputManager.CheckIfCanPressKey(Keys.B))
                {
                    pickedPanel = 1;
                }
            }
        }


        public void Draw(SpriteBatch spritebatch)
        {
            switch (pickedPanel)
            {
                case 0:
                    EscPanel.Draw(spritebatch);
                    break;
                case 1:
                    BestiaryPanel.Draw(spritebatch);
                    break;
            }
        }
    }
}
