using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Client
{
    public class SwitchPageMapSelection
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private Text mapName;
        private List<MapData> maps;
        private int pageNumber;

        public SwitchPageMapSelection(SpriteFont fontNumbers, Vector2 position, SelectedMapData data, ContentManager contentManager)
        {
            this.contentManager = contentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, null, "", position, 69, 52, Color.White);
            pageButtons[1] = new Button(null, null, "", new Vector2(position.X + 636, position.Y), 69, 52, Color.White);
            pageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 343, position.Y + 183), 20, 20);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            maps = new List<MapData>();

            InitializeRows();

            mapName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), maps[0].Name, true, new Vector2(510, 106), 260, 26);
            mapName.SetTextColor(Color.White);
            SetPageNumber(data);
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (pageButtons[0].CheckLeftClick(clickPosition))
            {
                if (pageNumber > 1) pageNumber--;
            }
            else if (pageButtons[1].CheckLeftClick(clickPosition))
            {
                if (pageNumber < maps.Count) pageNumber++;
            }

            mapName.SetText(maps[pageNumber -1 ].Name);

            pageNumberText.SetText(pageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
            pageNumberText.Draw(spriteBatch);
            mapName.Draw(spriteBatch);
            maps[pageNumber - 1].Draw(spriteBatch);
        }

        public void InitializeRows()
        {
            maps.Add(new MapData(contentManager.Load<Texture2D>("UI/MapSelection/Standard"), "Standard"));
            maps.Add(new MapData(contentManager.Load<Texture2D>("UI/MapSelection/TwoBridges"), "Two Bridges"));
        }

        public void SetSelectedData(ref SelectedMapData data)
        {
            data.ChangeData(pageNumber - 1, mapName.GetText());
        }

        public void SetPageNumber(SelectedMapData data)
        {
            pageNumber = data.index + 1;

            mapName.SetText(maps[pageNumber - 1].Name);
            pageNumberText.SetText(pageNumber.ToString());
        }

    }
}
