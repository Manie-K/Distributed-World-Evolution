using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public abstract class ModuleParameter
    {
        protected Text moduleName;
        public string Description;
        public int Type;

        public ModuleParameter(ContentManager contentManager, Vector2 position, int type, string name, string description)
        {
            this.Type = type;
            this.Description = description;      
        }

        public abstract void Update(Vector2 position);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract bool CheckLeftClick(Vector2 position);
        public abstract int GetValue();
        public abstract string GetDescription();
    }
}
