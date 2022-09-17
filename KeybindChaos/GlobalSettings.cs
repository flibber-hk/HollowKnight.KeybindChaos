using InControl;
using Modding.Converters;
using Newtonsoft.Json;

namespace KeybindChaos
{
    public class TimerSettings
    {
        /// <summary>
        /// Whether keybinds should change after enough time has elapsed.
        /// </summary>
        public bool Enabled = true;
        /// <summary>
        /// Time before keybinds reset.
        /// </summary>
        public float ResetTime = 60f;
        /// <summary>
        /// Whether to play the audio when near the end of the countdown.
        /// </summary>
        public bool Audio = false;
    }

    public class GlobalSettings
    {
        public TimerSettings TS = new();

        // TODO - other triggers?

        public bool KeybindDisplay = true;

        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KCBinds Binds = new();

        /// <summary>
        /// Make sure nullable fields are not null.
        /// </summary>
        public void Verify()
        {
            TS ??= new();
            Binds ??= new();
        }
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
