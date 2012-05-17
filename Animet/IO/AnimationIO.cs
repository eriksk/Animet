using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NibLib.Animations;
using System.IO;
using NibLib.Frames;
using System.Globalization;
using Microsoft.Xna.Framework;
using NibLib.Scripts;

namespace NibLib.IO
{
    public class AnimationIO
    {
        public static void Save(AnimationCollection coll, string path)
        {
            using (StreamWriter w = new StreamWriter(path, false))
            {
                // frames
                w.WriteLine(coll.frames.Count);
                for (int i = 0; i < coll.frames.Count; i++)
                {
                    Frame f = coll.frames[i];
                    w.WriteLine(f.Name);
                    w.WriteLine(f.parts.Count);
                    for (int j = 0; j < f.parts.Count; j++)
                    {
                        FramePart fp = f.parts[j];
                        w.WriteLine(fp.position.X.ToString(CultureInfo.InvariantCulture) + "," + fp.position.Y.ToString(CultureInfo.InvariantCulture));
                        w.WriteLine(fp.Source.X + "," + fp.Source.Y + "," + fp.Source.Width + "," + fp.Source.Height);
                        w.WriteLine(fp.rotation.ToString(CultureInfo.InvariantCulture));
                        w.WriteLine(fp.scale.ToString(CultureInfo.InvariantCulture));

                    }
                }

                // animations
                w.WriteLine(coll.animations.Count);
                for (int i = 0; i < coll.animations.Count; i++)
                {
                    Animation a = coll.animations[i];
                    w.WriteLine(a.Name);
                    w.WriteLine(a.Keyframes.Count);
                    for (int j = 0; j < a.Keyframes.Count; j++)
                    {
                        KeyFrame kf = a.Keyframes[j];
                        w.WriteLine(kf.duration.ToString(CultureInfo.InvariantCulture));
                        w.WriteLine(coll.frames.IndexOf(kf.frameRef));
                        w.WriteLine(kf.editorScripts.Length);
                        for (int x = 0; x < kf.editorScripts.Length; x++)
                        {
                            w.WriteLine(kf.editorScripts[x]);
                        }

                    }
                }
            }
        }

        public static AnimationCollection Load(string path)
        {
            List<Frame> frames = new List<Frame>();
            List<Animation> animations = new List<Animation>();

            using(StreamReader r = new StreamReader(path))
            {
                // frames
                int frameCount = int.Parse(r.ReadLine());
                for (int i = 0; i < frameCount; i++)
                {
                    string fName = r.ReadLine();
                    List<FramePart> fps = new List<FramePart>();
                    int fpCount = int.Parse(r.ReadLine());
                    for (int j = 0; j < fpCount; j++)
                    {
                        string[] pos = r.ReadLine().Split(',');
                        Vector2 position = new Vector2(float.Parse(pos[0], CultureInfo.InvariantCulture), float.Parse(pos[1], CultureInfo.InvariantCulture));
                        string[] src = r.ReadLine().Split(',');
                        Rectangle source = new Rectangle(int.Parse(src[0]), int.Parse(src[1]), int.Parse(src[2]),int.Parse(src[3]));
                        float rotation = float.Parse(r.ReadLine(), CultureInfo.InvariantCulture);
                        float scale = float.Parse(r.ReadLine(), CultureInfo.InvariantCulture);
                        fps.Add(new FramePart(position, rotation, scale) { Source = source });
                    }

                    Frame f = new Frame(null, fps) { Name = fName };    
                    frames.Add(f);
                }

                int animCount = int.Parse(r.ReadLine());
                for (int i = 0; i < animCount; i++)
                {
                    string animName = r.ReadLine();
                    List<KeyFrame> keyFrames = new List<KeyFrame>();
                    int kfCount = int.Parse(r.ReadLine());
                    for (int j = 0; j < kfCount; j++)
                    {
                        float duration = float.Parse(r.ReadLine(), CultureInfo.InvariantCulture);
                        int fIndex = int.Parse(r.ReadLine());
                        string[] scripts = new string[int.Parse(r.ReadLine())];
                        for (int x = 0; x < scripts.Length; x++)
                        {
                            scripts[x] = r.ReadLine();
                        }
                        List<KeyFrameScript> s = new List<KeyFrameScript>();
                        KeyFrame kf = new KeyFrame(frames[fIndex], duration, scripts);
                        keyFrames.Add(kf);
                    }

                    Animation a = new Animation(keyFrames) { Name = animName };
                    animations.Add(a);
                }
                
                AnimationCollection collection = new AnimationCollection(frames, animations);
                return collection;
            }
        }
    }
}