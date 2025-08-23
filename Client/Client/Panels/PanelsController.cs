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
        public Bestiary_Panel Bestiary_Panel { get; set; }
        public Esc_Panel Esc_Panel { get; set; }
        private int pickedPanel;
        private bool isBlocked;

        public PanelsController(GameManager gameManager) 
        {
            this.gameManager = gameManager;
            Bestiary_Panel = new Bestiary_Panel(gameManager);
            Esc_Panel = new Esc_Panel(gameManager);
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
                        if (Esc_Panel.CheckLeftClick(gameManager.InputManager.GetMousePosition())) pickedPanel = -1;
                        break;
                    case 1:
                        if (Bestiary_Panel.CheckLeftClick(gameManager.InputManager.GetMousePosition())) pickedPanel = -1;
                        break;
                }
            }

            switch (pickedPanel)
            {
                case 0:
                    if (Esc_Panel.Update(gameManager.InputManager.GetMousePosition()))
                    {
                        pickedPanel = -1;
                        isBlocked = true;
                    }
                    break;
                case 1:
                    if (Bestiary_Panel.Update(gameManager.InputManager.GetMousePosition()))
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
                    Esc_Panel.Draw(spritebatch);
                    break;
                case 1:
                    Bestiary_Panel.Draw(spritebatch);
                    break;
            }
        }
    }
}
