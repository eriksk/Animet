using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animet.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Animet.Frames
{
    public class KeyFrame
    {
        protected Frame frameRef;
        protected KeyFrameScript[] scripts;
        protected float current, duration;

        public KeyFrame(Frame frameRef, float duration, KeyFrameScript[] scripts)
        {
            this.frameRef = frameRef;
            this.duration = duration;
            this.scripts = scripts;
        }

        public void Reset()
        {
            current = 0f;
        }

        public bool Done
        {
            get { return current / duration > 1.0f; }
        }

        public void Update(float dt)
        {
            current += dt;
            if (current > duration)
            {
                for (int i = 0; i < scripts.Length; i++)
                {
                    //scripts[i].Execute();
                }
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            frameRef.Draw(sb, position);
        }
    }
}
