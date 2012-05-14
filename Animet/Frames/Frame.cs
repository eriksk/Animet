using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Animet.Animations;

namespace Animet.Frames
{
    public class Frame : INameable
    {
        public List<FramePart> parts;
        public Texture2D texture;

        public Frame(Texture2D texture, List<FramePart> parts)
        {
            this.texture = texture;
            this.parts = parts; 
        }

        public string Name
        {
            get;
            set;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].Draw(sb, texture, position);
            }
        }
    }
}
