using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NibLib.Frames;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NibLib.Animations
{
    public class AnimationCollection
    {
        public List<Frame> frames;
        public List<Animation> animations;

        public AnimationCollection()
            : this(new List<Frame>(), new List<Animation>())
        {
        }
        public AnimationCollection(List<Frame> frames, List<Animation> animations)
        {
            this.frames = frames;
            this.animations = animations;
        }

        public void Load(ContentManager content)
        {
            foreach (Frame f in frames)
            {
                f.texture = content.Load<Texture2D>(@"gfx/tex");
            }
        }

        public Animation this[string name]
        {
            get
            {
                for (int i = 0; i < animations.Count; i++)
                {
                    if (animations[i].Name == name)
                    {
                        return animations[i];
                    }
                }
                return null;
            }
        }
    }
}