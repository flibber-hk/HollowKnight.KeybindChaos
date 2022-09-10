using InControl;
using Modding.Converters;
using Newtonsoft.Json;

namespace KeybindChaos
{
    public class GlobalSettings
    {
        /// <summary>
        /// Whether keybinds should change after enough time has elapsed.
        /// </summary>
        public bool TimerEnabled = true;
        /// <summary>
        /// Time before keybinds reset.
        /// </summary>
        public float ResetTime = 60f;

        // TODO - other triggers?

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
