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
        protected int currentFrame;
        protected int runCount;

        public Animation(List<KeyFrame> keyFrames)
        {
            this.keyFrames = keyFrames;
            Reset();
        }

        public string Name
        {
            get;
            set;
        }

        public int RunCount
        {
            get { return runCount; }
        }

        public List<KeyFrame> Keyframes
        {
            get { return keyFrames; }
        }

        public void Reset()
        {
            currentFrame = 0;
            for (int i = 0; i < keyFrames.Count; i++)
            {
                keyFrames[i].Reset();
            }
            runCount = 0;
        }

        public void Update(float dt)
        {
            if (keyFrames[currentFrame].Done)
            {
                keyFrames[currentFrame].Reset();
                currentFrame++;
                if (currentFrame >= keyFrames.Count)
                {
                    currentFrame = 0;
                    runCount++;
                }
            }
            keyFrames[currentFrame].Update(dt);
        }

        public void Draw(SpriteBatch sb, Vector2 position, bool flipped = false)
        {
            if (currentFrame <= keyFrames.Count - 1)
            {
                KeyFrame kf = keyFrames[currentFrame];
                KeyFrame nextFrame = keyFrames[currentFrame + 1 > keyFrames.Count - 1 ? 0 : currentFrame + 1];
                kf.DrawLerped(sb, position, nextFrame, flipped);
            }
        }
    }
}
