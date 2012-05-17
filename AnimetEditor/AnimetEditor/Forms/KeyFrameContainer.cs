﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Animet.Frames;
using Microsoft.Xna.Framework.Input;

namespace AnimetEditor.Forms
{
    class KeyFrameContainer
    {
        public Rectangle rect;
        public Texture2D pixel;
        public Color bgColor = Color.Black * 0.5f;
        public SpriteFont font;
        public float fontHeight;
        private KeyFrame frame;
        private Rectangle slider;

        public KeyFrameContainer()
        {

        }

        public KeyFrameContainer(int x, int y)
        {
            rect = new Rectangle(x, y, 128, 128);
        }

        public KeyFrameContainer Load(ContentManager content, GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            font = content.Load<SpriteFont>(@"fonts/font");
            fontHeight = font.MeasureString("HEIGHT").Y;

            return this;
        }

        public void SetKeyFrame(KeyFrame frame)
        {
            this.frame = frame;
            if (frame != null)
            {
                slider = new Rectangle((int)MathHelper.Lerp(0, rect.Width, frame.duration / 1000), rect.Width / 2, 24, 24);
            }
        }
        
        MouseState om, m;
        KeyboardState key, oldkey;
        public void Update(float dt)
        { om = m;
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
                        if (new Rectangle(rect.X + slider.X, rect.Y + slider.Y, slider.Width, slider.Height).Contains(m.X, m.Y))
                        {
                            slider.X += (m.X - om.X);
                            slider.X = (int)MathHelper.Clamp(slider.X, 0, rect.X + rect.Width);
                            frame.duration = (int)MathHelper.Lerp(0, 1000, ((float)slider.X) / ((float)rect.X + (float)rect.Width));
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rect, bgColor);
            sb.DrawOutline(pixel, rect, Color.Black);
            if(frame != null)
            {
                sb.DrawString(font, frame.frameRef.Name, new Vector2(rect.X + 8, rect.Y + fontHeight), Color.White);     
                sb.DrawString(font, frame.duration.ToString(), new Vector2(rect.X + 8, rect.Y + fontHeight * 2f), Color.White);
                sb.DrawOutline(pixel, new Rectangle(rect.X + slider.X, rect.Y + slider.Y, slider.Width, slider.Height), Color.Black);
                sb.DrawOutline(pixel, new Rectangle(rect.X, rect.Y + slider.Y, rect.Width, 1), Color.Black * 0.5f);
            }
        }
    }
}
