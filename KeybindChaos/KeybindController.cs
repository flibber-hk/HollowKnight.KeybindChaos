using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using MagicUI.Core;
using MagicUI.Elements;
using Modding;
using RandomizerCore.Extensions;
using UnityEngine;

namespace KeybindChaos
{
    public class KeybindController : MonoBehaviour
    {
        private readonly Modding.ILogger _logger = new SimpleLogger($"{nameof(KeybindChaos)}:{nameof(KeybindController)}");

        private List<PlayerAction> _storedBindings;

        private LayoutRoot layout;
        private TextObject displayTimer;
        private float time;

        void Start()
        {
            _storedBindings = Retrieve();

            if (layout == null)
            {
                layout = new(true, "KC Timer");

                displayTimer = new(layout)
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 40,
                    Font = UI.TrajanBold,
                    Text = ""
                };
            }

            time = 60f;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                time = 1000f;
                RandomizeBinds();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Assign(_storedBindings);
                time = 1000f;
            }

            time -= Time.deltaTime;
            displayTimer.Text = string.Format("{0:0}s", time);
            if (time < 0)
            {
                RandomizeBinds();
                time = 60f;
            }
        }

        void OnDestroy()
        {
            layout?.Destroy();
            layout = null;

            Assign(_storedBindings);
            AllowSavingBinds();
        }

        public void RandomizeBinds()
        {
            PreventSavingBinds();

            List<PlayerAction> actions = Retrieve();
            KeybindChaos.rng.PermuteInPlace(actions);
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
            _logger.Log("Not sending button binds to game settings");
        }
        private void PreventSavingKeyBinds(On.InputHandler.orig_SendKeyBindingsToGameSettings orig, InputHandler self)
        {
            _logger.Log("Not sending keybinds to game settings");
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
            KeybindChaos.instance.Log(InputHandler.Instance.inputActions.attack.Name);

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
