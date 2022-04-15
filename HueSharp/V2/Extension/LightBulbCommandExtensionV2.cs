using HueSharp.Command;

namespace HueSharp.V2.Extension
{
    public static class LightBulbCommandExtensionV2
    {
        public static LightBulbCommand SetAlert(this LightBulbCommand command, bool activateAlert)
        {
            command.Alert = (activateAlert) ? "breathe" : "none";
            return command;
        }

        public static LightBulbCommand SetEffect(this LightBulbCommand command, LightEffect effect)
        {
            command.Effect = effect.ToString().ToLower();
            return command;
        }
    }
}
