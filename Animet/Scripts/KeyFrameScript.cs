using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NibLib.Scripts
{
    public abstract class KeyFrameScript
    {
        public KeyFrameScript(string line)
        {
        }

        public abstract void Execute();            
    }
}
