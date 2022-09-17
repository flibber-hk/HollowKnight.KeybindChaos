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
        public bool Audio = true;
    }

    public class RandomizableBinds
    {
        public bool Jump { get; set; } = true;
        public bool Attack { get; set; } = true;
        public bool Dash { get; set; } = true;
        public bool Focus { get; set; } = true;
        public bool Superdash { get; set; } = true;
        public bool DreamNail { get; set; } = true;
        public bool QuickCast { get; set; } = true;
        public bool QuickMap { get; set; } = true;
    }

    public class GlobalSettings
    {
        public TimerSettings TimerSettings = new();

        // TODO - other triggers?

        public bool KeybindDisplay = true;

        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KCBinds Binds = new();

        public RandomizableBinds RandomizableBinds = new();

        /// <summary>
        /// Make sure nullable members are not null.
        /// </summary>
        public void Verify()
        {
            TimerSettings ??= new();
            Binds ??= new();
            RandomizableBinds ??= new();
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
