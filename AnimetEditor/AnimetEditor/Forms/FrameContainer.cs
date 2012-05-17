using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Animet.Frames;
using AnimtetEditor.Utils;

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
        private int selectedPart = -1;

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
            if (frame != null)
            {
                selectedPart = frame.parts.Count - 1;
            }
            else
            {
                selectedPart = -1;
            }
        }

        public void SetSource(Rectangle source)
        {
            if (selectedPart != -1)
            {
                frame.parts[selectedPart].Source = source;
            }
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
                    if (selectedPart != -1)
                    {
                        if (m.LeftButton == ButtonState.Pressed)
                        {
                            if (key.IsKeyDown(Keys.LeftShift))
                            {
                                // rotate
                                frame.parts[selectedPart].rotation += (m.Y - om.Y) * 0.005f;
                            }
                            else
                            {
                                if (key.IsKeyDown(Keys.LeftControl))
                                {
                                    // move all
                                    for (int i = 0; i < frame.parts.Count; i++)
                                    {
                                        frame.parts[i].position += new Vector2(m.X - om.X, m.Y - om.Y);
                                    }
                                }
                                else
                                {
                                    // move
                                    frame.parts[selectedPart].position += new Vector2(m.X - om.X, m.Y - om.Y);
                                }
                            }
                        }
                        if (m.MiddleButton == ButtonState.Pressed)
                        {
                            // scale
                            frame.parts[selectedPart].scale += (m.Y - om.Y) * 0.005f;
                        }
                        if (key.IsKeyDown(Keys.LeftControl))
                        {
                            if (key.IsKeyDown(Keys.NumPad1))
                            {
                                frame.parts[selectedPart].scale = 1f;
                            }
                            if (key.IsKeyDown(Keys.NumPad0))
                            {
                                frame.parts[selectedPart].rotation = 0f;
                            }
                            if (key.IsKeyDown(Keys.Delete))
                            {
                                frame.parts.RemoveAt(selectedPart);
                                selectedPart = -1;
                            }
                            if (key.IsKeyDown(Keys.A) && oldkey.IsKeyUp(Keys.A))
                            {
                                frame.parts.Add(new FramePart(Vector2.Zero, 0f, 1f));
                                selectedPart = frame.parts.Count - 1;
                            }
                            if (key.IsKeyDown(Keys.D) && oldkey.IsKeyUp(Keys.D))
                            {
                                if (selectedPart != -1)
                                {
                                    frame.parts.Add(frame.parts[selectedPart].Clone());
                                    selectedPart = frame.parts.Count - 1;
                                }
                            }
                        }
                    }
                    if (m.ScrollWheelValue != om.ScrollWheelValue)
                    {
                        if (key.IsKeyDown(Keys.LeftControl))
                        {
                            if (frame.parts.Count > 0)
                            {
                                int target = selectedPart + (m.ScrollWheelValue - om.ScrollWheelValue) / 100;
                                if (target > frame.parts.Count - 1)
                                {
                                    target = 0;
                                }
                                else if (target < 0)
                                {
                                    target = frame.parts.Count - 1;
                                }

                                // swap
                                FramePart temp = frame.parts[selectedPart];
                                frame.parts[selectedPart] = frame.parts[target];
                                frame.parts[target] = temp;

                                selectedPart = target;
                            }
                        }
                        else
                        {
                            if (frame.parts.Count > 0)
                            {
                                selectedPart += (m.ScrollWheelValue - om.ScrollWheelValue) / 100;
                                if (selectedPart > frame.parts.Count - 1)
                                {
                                    selectedPart = 0;
                                }
                                else if (selectedPart < 0)
                                {
                                    selectedPart = frame.parts.Count - 1;
                                }
                            }
                        }
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
                Vector2 pos = new Vector2(rect.Center.X, rect.Center.Y);
                frame.Draw(sb, pos);
                for (int i = 0; i < frame.parts.Count; i++)
                {
                    if (selectedPart == i)
                    {
                        FramePart fp = frame.parts[selectedPart];
                        sb.DrawOutline(pixel, Util.RotateRectangle(fp.origin, pos + fp.position, fp.scale, fp.rotation), Color.White * 0.5f);
                    }
                }
            }
        }
    }
}
