using InControl;
using JetBrains.Annotations;
using Modding;
using RandomizerCore.Extensions;
using System;
using System.Collections.Generic;

namespace KeybindChaos
{
    /// <summary>
    /// Class to help with randomizing the binds.
    /// </summary>
    [PublicAPI]
    public static class KeybindPermuter
    {
        [PublicAPI] public static event Action OnRandomize;
        [PublicAPI] public static event Action OnRestore;

        private static bool _isRandomized = false;
        private static List<PlayerAction> _storedBindings = null;
        private static readonly ILogger _logger = new SimpleLogger($"{nameof(KeybindChaos)}:{nameof(KeybindPermuter)}");

        private static readonly Random _rng = new();

        private static bool _preventSavingBinds = false;
        private static void PreventSavingBinds()
        {
            if (_preventSavingBinds) return;
            On.InputHandler.SendKeyBindingsToGameSettings += PreventSavingKeyBinds;
            On.InputHandler.SendButtonBindingsToGameSettings += PreventSavingButtonBinds;
            _preventSavingBinds = true;
        }
        private static void AllowSavingBinds()
        {
            On.InputHandler.SendKeyBindingsToGameSettings -= PreventSavingKeyBinds;
            On.InputHandler.SendButtonBindingsToGameSettings -= PreventSavingButtonBinds;
            _preventSavingBinds = false;
        }
        private static void PreventSavingButtonBinds(On.InputHandler.orig_SendButtonBindingsToGameSettings orig, InputHandler self)
        {
            _logger.Log("Not sending button binds to game settings");
        }
        private static void PreventSavingKeyBinds(On.InputHandler.orig_SendKeyBindingsToGameSettings orig, InputHandler self)
        {
            _logger.Log("Not sending keybinds to game settings");
        }

        [PublicAPI]
        public static void RandomizeBinds() => RandomizeBinds(_rng);


        [PublicAPI]
        public static void RandomizeBinds(Random rng)
        {
            if (!_isRandomized)
            {
                _isRandomized = true;
                _storedBindings = RetrieveBinds();
                PreventSavingBinds();
            }

            List<PlayerAction> actions = RetrieveBinds();
            rng.PermuteInPlace(actions);
            AssignBinds(actions);

            OnRandomize?.Invoke();
        }

        [PublicAPI]
        public static void RestoreBinds()
        {
            if (!_isRandomized) return;

            AssignBinds(_storedBindings);
            _storedBindings = null;
            _isRandomized = false;
            AllowSavingBinds();

            OnRestore?.Invoke();
        }

        /// <summary>
        /// Return a list of the current binds, in order.
        /// </summary>
        public static List<PlayerAction> RetrieveBinds()
        {
            List<PlayerAction> actions = new();

            if (KeybindChaos.GS.RandomizableBinds.Jump) actions.Add(InputHandler.Instance.inputActions.jump);
            if (KeybindChaos.GS.RandomizableBinds.Attack) actions.Add(InputHandler.Instance.inputActions.attack);
            if (KeybindChaos.GS.RandomizableBinds.Dash) actions.Add(InputHandler.Instance.inputActions.dash);
            if (KeybindChaos.GS.RandomizableBinds.Focus) actions.Add(InputHandler.Instance.inputActions.cast);
            if (KeybindChaos.GS.RandomizableBinds.Superdash) actions.Add(InputHandler.Instance.inputActions.superDash);
            if (KeybindChaos.GS.RandomizableBinds.DreamNail) actions.Add(InputHandler.Instance.inputActions.dreamNail);
            if (KeybindChaos.GS.RandomizableBinds.QuickCast) actions.Add(InputHandler.Instance.inputActions.quickCast);
            if (KeybindChaos.GS.RandomizableBinds.QuickMap) actions.Add(InputHandler.Instance.inputActions.quickMap);

            return actions;
        }

        /// <summary>
        /// Assign the ordered list of actions to the player actions.
        /// </summary>
        private static void AssignBinds(List<PlayerAction> actions)
        {
            int index = 0;

            if (KeybindChaos.GS.RandomizableBinds.Jump)
            {
                InputHandler.Instance.inputActions.jump = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.Attack)
            {
                InputHandler.Instance.inputActions.attack = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.Dash)
            {
                InputHandler.Instance.inputActions.dash = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.Focus)
            {
                InputHandler.Instance.inputActions.cast = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.Superdash)
            {
                InputHandler.Instance.inputActions.superDash = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.DreamNail)
            {
                InputHandler.Instance.inputActions.dreamNail = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.QuickCast)
            {
                InputHandler.Instance.inputActions.quickCast = actions[index];
                index++;
            }
            if (KeybindChaos.GS.RandomizableBinds.QuickMap)
            {
                InputHandler.Instance.inputActions.quickMap = actions[index];
                index++;
            }
        }
    }
}
