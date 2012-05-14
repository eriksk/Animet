using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animet.Frames;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Animet.Animations
{
    public class Animation : INameable
    {
        protected List<KeyFrame> keyFrames;
        protected List<Frame> frames;
        protected int currentFrame;

        public Animation(List<KeyFrame> keyFrames, List<Frame> frames)
        {
            this.keyFrames = keyFrames;
            this.frames = frames;
            Reset();
        }

        public string Name
        {
            get;
            set;
        }        

        public void Reset()
        {
            currentFrame = 0;
            for (int i = 0; i < keyFrames.Count; i++)
            {
                keyFrames[i].Reset();
            }
        }

        public void Update(float dt)
        {
            if (keyFrames[currentFrame].Done)
            {
                keyFrames[currentFrame].Reset();
                currentFrame++;
                if (currentFrame > keyFrames.Count)
                {
                    currentFrame = 0;
                }
            }
            keyFrames[currentFrame].Update(dt);
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            KeyFrame kf = keyFrames[currentFrame];
            kf.Draw(sb, position);
        }
    }
}
