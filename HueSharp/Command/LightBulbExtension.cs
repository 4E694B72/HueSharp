using System.Collections.Generic;

namespace HueSharp.Command
{
    public static class LightBulbExtension
    {
        public static LightBulbCommand TurnOn(this LightBulbCommand command)
        {
            command.On = true;
            return command;
        }

        public static LightBulbCommand TurnOff(this LightBulbCommand command)
        {
            command.On = false;
            return command;
        }

        public static LightBulbCommand SendToBulb(this LightBulbCommand command, string bulbID)
        {
            if(command.Bulbs == null)
            {
                command.Bulbs = new List<string>();
            }
            command.Bulbs.Add(bulbID);
            return command;
        }

        public static LightBulbCommand SendToGroup(this LightBulbCommand command, string bulbID)
        {
            if (command.Groups == null)
            {
                command.Groups = new List<string>();
            }
            command.Groups.Add(bulbID);
            return command;
        }
    }
}
