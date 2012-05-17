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

        private Vector2 pos = Vector2.Zero;
        public void Draw(SpriteBatch sb, Texture2D texture, Vector2 position, bool flipped = false)
        {
            pos.Y = this.position.Y;
            pos.X = flipped ? -this.position.X : this.position.X;
            sb.Draw(
                texture,
                position + pos,
                source,
                Color.White,
                flipped ? -rotation : rotation,
                origin,
                scale,
                flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                1f);
        }

        public FramePart Clone()
        {
            return new FramePart(position, rotation, scale)
            {
                Source = Source,
            };
        }

        public void DrawLerped(SpriteBatch sb, Texture2D texture, Vector2 position, FramePart nextPart, float process, bool flipped = false)
        {
            pos = Vector2.SmoothStep(this.position, nextPart.position, process);
            pos.X = flipped ? -pos.X : pos.X;

            sb.Draw(
                texture,
                position + pos,
                source,
                Color.White,
                flipped ? -MathHelper.SmoothStep(rotation, nextPart.rotation, process) : MathHelper.SmoothStep(rotation, nextPart.rotation, process),
                origin,
                MathHelper.SmoothStep(scale, nextPart.scale, process),
                flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                1f);
        }
    }
}
