using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Animet.Frames;

namespace AnimetEditor.Forms
{
    class FrameContainer
    {
        public Rectangle rect;
        public Texture2D pixel;
        public Color bgColor = Color.Black * 0.5f;
        public SpriteFont font;
        public float fontHeight;
        private Frame frame;

        public FrameContainer(int x, int y)
        {
            rect = new Rectangle(x, y, 512, 512);
        }

        public FrameContainer Load(ContentManager content, GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            font = content.Load<SpriteFont>(@"fonts/font");
            fontHeight = font.MeasureString("HEIGHT").Y;

            return this;
        }

        public void SetFrame(Frame frame)
        {
            this.frame = frame;
        }

        public void SetSource(Rectangle source)
        {
            frame.parts[0].Source = source;
        }

        MouseState om, m;
        KeyboardState key, oldkey;
        public void Update(float dt)
        {
            om = m;
            m = Mouse.GetState();
            oldkey = key;
            key = Keyboard.GetState();

            if (rect.Contains(m.X, m.Y))
            {
                if (m.RightButton == ButtonState.Pressed)
                {
                    rect.X += (int)((m.X - om.X));
                    rect.Y += (int)((m.Y - om.Y));
                }

                if (frame != null)
                {
                    if (m.LeftButton == ButtonState.Pressed)
                    {
                        if (key.IsKeyDown(Keys.LeftShift))
                        {
                            // rotate
                            frame.parts[0].rotation += (m.Y - om.Y) * 0.005f;
                        }
                        else
                        {
                            // move
                            frame.parts[0].position += new Vector2(m.X - om.X, m.Y - om.Y);
                        }
                    }
                    if (m.MiddleButton == ButtonState.Pressed)
                    {
                        // scale
                        frame.parts[0].scale += (m.Y - om.Y) * 0.005f;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rect, bgColor);
            sb.DrawOutline(pixel, rect, Color.Black);
            if (frame != null)
            {
                sb.DrawOutline(pixel, new Rectangle(rect.Left, rect.Center.Y, rect.Width, 1), Color.Black * 0.5f);
                sb.DrawOutline(pixel, new Rectangle(rect.Center.X, rect.Top, 1, rect.Height), Color.Black * 0.5f);
                frame.Draw(sb, new Vector2(rect.Center.X, rect.Center.Y));
                for (int i = 0; i < frame.parts.Count; i++)
                {
                    sb.DrawString(font, (i + 1) + "", new Vector2(rect.X + 2, rect.Y + (i * fontHeight)), Color.White);
                }
            }
        }
    }
}
