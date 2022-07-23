using InControl;
using Modding.Converters;
using Newtonsoft.Json;

namespace KeybindChaos
{
    public class GlobalSettings
    {
        /// <summary>
        /// Whether the mod is enabled.
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// Time before keybinds reset - null for no timer.
        /// </summary>
        public float? ResetTime = 60f;

        public bool KeybindDisplay = true;

        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KCBinds Binds = new();
    }

    public class KCBinds : PlayerActionSet
    {
        // No default bind for this.
        public PlayerAction ManualTrigger;

        public KCBinds()
        {
            ManualTrigger = CreatePlayerAction(nameof(ManualTrigger));
        }
    }
}
