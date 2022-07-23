using System.Collections.Generic;
using InControl;
using MagicUI.Core;
using MagicUI.Elements;
using Modding;
using RandomizerCore.Extensions;
using UnityEngine;
using Random = System.Random;

namespace KeybindChaos
{
    public class KeybindController : MonoBehaviour
    {
        internal static KeybindController Instance;

        private readonly Modding.ILogger _logger = new SimpleLogger($"{nameof(KeybindChaos)}:{nameof(KeybindController)}");

        private List<PlayerAction> _storedBindings;

        private readonly Random rng = new();

        private LayoutRoot layout;
        private TextFormatter<float> displayTimer;
        private float time;

        void Awake() => Instance = this;

        void Start()
        {
            _storedBindings = Retrieve();

            if (layout == null)
            {
                layout = new(true, "KC Timer")
                {
                    VisibilityCondition = () => { _logger.Log(KeybindChaos.GS.Enabled); return KeybindChaos.GS.Enabled; }
                };

                displayTimer = new(layout, 0, FormatTimeDisplay, "KC Time Formatter")
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Text = new(layout)
                    {
                        FontSize = 40,
                        Font = UI.TrajanBold,
                    }
                };
            }

            time = KeybindChaos.GS.ResetTime ?? -1f;
        }

        private static string FormatTimeDisplay(float t)
        {
            if (KeybindChaos.GS.ResetTime is null) return string.Empty;

            return string.Format("Keybind Reset: {0:0}s", t);
        }

        void Update()
        {
            if (!KeybindChaos.GS.Enabled) return;

            if (KeybindChaos.GS.Binds.ManualTrigger.WasPressed)
            {
                RandomizeBinds();
            }
            else if (KeybindChaos.GS.ResetTime is not null)
            {
                if (time == -1)
                {
                    time = KeybindChaos.GS.ResetTime.Value;
                }

                time -= Time.deltaTime;

                if (time < 0)
                {
                    RandomizeBinds();
                }
            }

            displayTimer.Data = time;
        }

        void OnDestroy()
        {
            layout?.Destroy();
            layout = null;

            Assign(_storedBindings);
            AllowSavingBinds();

            Instance = null;
        }

        public void RandomizeBinds()
        {
            time = KeybindChaos.GS.ResetTime ?? -1f;

            PreventSavingBinds();

            List<PlayerAction> actions = Retrieve();
            rng.PermuteInPlace(actions);
            Assign(actions);
        }

        private bool _preventSavingBinds = false;
        private void PreventSavingBinds()
        {
            if (_preventSavingBinds) return;
            On.InputHandler.SendKeyBindingsToGameSettings += PreventSavingKeyBinds;
            On.InputHandler.SendButtonBindingsToGameSettings += PreventSavingButtonBinds;
            _preventSavingBinds = true;
        }
        private void AllowSavingBinds()
        {
            On.InputHandler.SendKeyBindingsToGameSettings -= PreventSavingKeyBinds;
            On.InputHandler.SendButtonBindingsToGameSettings -= PreventSavingButtonBinds;
            _preventSavingBinds = false;
        }
        private void PreventSavingButtonBinds(On.InputHandler.orig_SendButtonBindingsToGameSettings orig, InputHandler self)
        {
            _logger.LogDebug("Not sending button binds to game settings");
        }
        private void PreventSavingKeyBinds(On.InputHandler.orig_SendKeyBindingsToGameSettings orig, InputHandler self)
        {
            _logger.LogDebug("Not sending keybinds to game settings");
        }

        public void Reset()
        {
            ResetTimer();
            ResetBinds();
        }
        public void ResetTimer()
        {
            time = KeybindChaos.GS.ResetTime ?? -1f;
        }
        public void ResetBinds()
        {
            Assign(_storedBindings);
            AllowSavingBinds();
        }


        public List<PlayerAction> Retrieve()
        {
            return new()
            {
                InputHandler.Instance.inputActions.attack,
                InputHandler.Instance.inputActions.superDash,
                InputHandler.Instance.inputActions.dreamNail,
                InputHandler.Instance.inputActions.jump,
                InputHandler.Instance.inputActions.dash,
                InputHandler.Instance.inputActions.cast,
                InputHandler.Instance.inputActions.quickCast,
                InputHandler.Instance.inputActions.quickMap
            };
        }
        public void Assign(List<PlayerAction> actions)
        {
            InputHandler.Instance.inputActions.attack = actions[0];
            InputHandler.Instance.inputActions.superDash = actions[1];
            InputHandler.Instance.inputActions.dreamNail = actions[2];
            InputHandler.Instance.inputActions.jump = actions[3];
            InputHandler.Instance.inputActions.dash = actions[4];
            InputHandler.Instance.inputActions.cast = actions[5];
            InputHandler.Instance.inputActions.quickCast = actions[6];
            InputHandler.Instance.inputActions.quickMap = actions[7];
        }
    }
}
