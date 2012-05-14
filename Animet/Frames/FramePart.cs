using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Animet.Frames
{
    public class FramePart
    {
        public Vector2 position, origin;
        private Rectangle source;
        public float rotation, scale;

        public FramePart(Vector2 position, float rotation, float scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            Source = new Rectangle(0,0,64,64);
        }

        public Rectangle Source
        {
            get { return source; }
            set 
            {
                source = value;
                origin.X = source.Width / 2f;
                origin.Y = source.Height / 2f;
            }
        }

        public void Draw(SpriteBatch sb, Texture2D texture, Vector2 position)
        {
            sb.Draw(
                texture,
                this.position + position,
                source,
                Color.White,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);
        }
    }
}
