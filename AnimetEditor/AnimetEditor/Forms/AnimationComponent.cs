using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Animet.Animations;

namespace AnimetEditor.Forms
{
    class AnimationComponent
    {
        public Rectangle rect;
        public Texture2D pixel;
        public Color bgColor = Color.Black * 0.5f;
        public SpriteFont font;
        public float fontHeight;
        public int selected;
        private Animation anim;

        public AnimationComponent(int x, int y)
        {
            rect = new Rectangle(x, y, 512, 512);
        }

        public AnimationComponent Load(ContentManager content, GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            font = content.Load<SpriteFont>(@"fonts/font");
            fontHeight = font.MeasureString("HEIGHT").Y;

            return this;
        }

        public void SetAnimation(Animation anim)
        {
            this.anim = anim;
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

            try
            {
                if (anim != null)
                {
                    anim.Update(dt);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rect, bgColor);
            sb.DrawOutline(pixel, rect, Color.Black);
            if (anim != null)
            {
                anim.Draw(sb, new Vector2(rect.Center.X, rect.Center.Y));
            }
        }
    }
}
