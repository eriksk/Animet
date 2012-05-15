using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animet.Animations;
using System.IO;

namespace Animet.IO
{
    public class AnimationIO
    {
        public static void Save(AnimationCollection coll, string path)
        {
            using (StreamWriter w = new StreamWriter(path, false))
            {
                w.WriteLine(coll.frames.Count);

                // TODO: first frames then anims.
            }
        }

        public static AnimationCollection Load(string path)
        {
            return null;
        }
    }
}
