using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animet.Frames;
using Microsoft.Xna.Framework.Content;

namespace Animet.Animations
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
            //TODO: set all textures
        }
    }
}
