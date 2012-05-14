using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimetEditor.Forms
{
    class TextureContainer
    {
        private Rectangle rect;
        public Texture2D texture;
        private Texture2D pixel;
        private string asset;

        public TextureContainer(int x, int y, string asset)
        {
            this.asset = asset;
            rect = new Rectangle(x, y, 0, 0);
        }

        public TextureContainer Load(ContentManager content, GraphicsDevice graphics)
        {
            texture = content.Load<Texture2D>(asset);
            rect.Width = texture.Width;
            rect.Height = texture.Height;

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            return this;
        }

        MouseState om, m;
        public void Update(float dt)
        {
            om = m;
            m = Mouse.GetState();

            if (rect.Contains(m.X, m.Y))
            {
                if (m.RightButton == ButtonState.Pressed)
                {
                    rect.X += (int)((m.X - om.X));
                    rect.Y += (int)((m.Y - om.Y));
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rect, Color.Black * 0.2f);
            sb.DrawOutline(pixel, rect, Color.Black);
            sb.Draw(texture, rect, Color.White * 0.8f);
        }
    }
}
