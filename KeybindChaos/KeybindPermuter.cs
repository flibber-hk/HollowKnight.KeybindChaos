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
            return new()
            {
                InputHandler.Instance.inputActions.jump,
                InputHandler.Instance.inputActions.attack,
                InputHandler.Instance.inputActions.dash,
                InputHandler.Instance.inputActions.cast,
                InputHandler.Instance.inputActions.superDash,
                InputHandler.Instance.inputActions.dreamNail,
                InputHandler.Instance.inputActions.quickCast,
                InputHandler.Instance.inputActions.quickMap
            };
        }

        /// <summary>
        /// Assign the ordered list of actions to the player actions.
        /// </summary>
        private static void AssignBinds(List<PlayerAction> actions)
        {
            InputHandler.Instance.inputActions.jump = actions[0];
            InputHandler.Instance.inputActions.attack = actions[1];
            InputHandler.Instance.inputActions.dash = actions[2];
            InputHandler.Instance.inputActions.cast = actions[3];
            InputHandler.Instance.inputActions.superDash = actions[4];
            InputHandler.Instance.inputActions.dreamNail = actions[5];
            InputHandler.Instance.inputActions.quickCast = actions[6];
            InputHandler.Instance.inputActions.quickMap = actions[7];
        }
    }
}
