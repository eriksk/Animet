using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NibLib.Scripts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NibLib.Animations;

namespace NibLib.Frames
{
    public class KeyFrame : INameable
    {
        public Frame frameRef;
        protected KeyFrameScript[] scripts;
        public string[] editorScripts;
        protected float current;
        public float duration;

        public KeyFrame(Frame frameRef, float duration, KeyFrameScript[] scripts)
        {
            this.frameRef = frameRef;
            this.duration = duration;
            this.scripts = scripts;
            editorScripts = new string[0];
        }
        public KeyFrame(Frame frameRef, float duration, string[] editorscripts, KeyFrameScript[] scripts)
        {
            this.frameRef = frameRef;
            this.duration = duration;
            this.editorScripts = editorscripts;
            this.scripts = scripts;
        }

        public string Name
        {
            get
            {
                return frameRef.Name;
            }
            set
            {
            }
        }

        public void Reset()
        {
            current = 0f;
        }

        public bool Done
        {
            get { return current / duration >= 1.0f; }
        }

        public void Update(float dt)
        {
            current += dt;
            if (current > duration)
            {
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].Execute();
                }
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position, bool flipped)
        {
            frameRef.Draw(sb, position, flipped);
        }

        public void DrawLerped(SpriteBatch sb, Vector2 position, KeyFrame nextFrame, bool flipped)
        {
            frameRef.DrawLerped(sb, position, nextFrame.frameRef, current / duration, flipped);
        }
    }
}
