using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NibLib.Animations;
using AnimetEditor.Utils;

namespace AnimetEditor.Forms
{
    class TextBox
    {
        public Rectangle rect;
        public Texture2D pixel;
        public Color bgColor = Color.Black * 0.5f;
        public SpriteFont font;
        public float padding = 8;
        public float fontHeight;
        public bool showing, showCursor;
        private INameable nameable;

        public TextBox()
        {
        }

        public TextBox LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            font = content.Load<SpriteFont>(@"fonts/font");
            fontHeight = font.MeasureString("HEIGHT").Y;

            return this;
        }

        public void Show(INameable nameable)
        {
            this.nameable = nameable;
            showing = true;

            rect = new Rectangle((1280 / 2) - 250, 720/2, 500, 200);
        }

        KeyboardState oldkey, key;
        float cursorTime = 0f;
        public void Update(float dt)
        {
            oldkey = key;
            key = Keyboard.GetState();

            cursorTime -= dt;
            if (cursorTime < 0f)
            {
                cursorTime = 500f;
                showCursor = !showCursor;
            }

            if (key.IsKeyDown(Keys.Enter))
            {
                showing = false;
            }

            // input
            GatherKeyStrokes();
        }

        private void GatherKeyStrokes()
        {
            string text = nameable.Name;

            Keys[] oldPressed = oldkey.GetPressedKeys();
            Keys[] newPressed = key.GetPressedKeys();

            if (KeyClicked(Keys.Back))
            {
                if (text != null && text.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }
            }

            for (int i = 0; i < newPressed.Length; i++)
			{
			    if(!oldPressed.Contains(newPressed[i]))
                {
                    text += KeyUtils.ConvertKeyToChar(newPressed[i], key.IsKeyDown(Keys.RightShift));
                }
			}
            
            nameable.Name = text;
        }

        private bool KeyClicked(Keys keys)
        {
            return oldkey.IsKeyUp(keys) && key.IsKeyDown(keys);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rect, bgColor);
            sb.DrawOutline(pixel, rect, Color.Black);
            sb.DrawString(font, nameable.Name + (showCursor ? "" : "|"), new Vector2(rect.X + padding, rect.Y + padding), Color.White);
        }
    }
}
