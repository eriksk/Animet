using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animet.Scripts
{
    public enum KeyFrameScriptCommand
    {
        NONE,
        SET,
        GOTO,
        IFGOTO
    }

    public class KeyFrameScript
    {
        public KeyFrameScriptCommand command;

        public KeyFrameScript(string line)
        {
            command = KeyFrameScriptCommand.NONE;
            //TODO: parse script
        }

        public void Execute(IKeyFrameScriptActions obj)
        {
            switch (command)
            {
                case KeyFrameScriptCommand.NONE:
                    break;
                case KeyFrameScriptCommand.SET:
                    break;
                case KeyFrameScriptCommand.GOTO:
                    break;
                case KeyFrameScriptCommand.IFGOTO:
                    break;
            }
        }
    }
}
