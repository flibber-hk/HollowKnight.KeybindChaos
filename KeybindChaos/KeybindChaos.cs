using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;
using MagicUI.Core;
using MagicUI.Elements;
using Modding;
using RandomizerCore.Extensions;
using UnityEngine;
using Random = System.Random;

namespace KeybindChaos
{
    public class KeybindChaos : Mod
    {
        internal static KeybindChaos instance;

        internal static Random rng = new();
        
        public KeybindChaos() : base(null)
        {
            instance = this;

            On.InputHandler.SendKeyBindingsToGameSettings += InputHandler_SendKeyBindingsToGameSettings;
            On.InputHandler.SendButtonBindingsToGameSettings += InputHandler_SendButtonBindingsToGameSettings;
        }

        private void InputHandler_SendButtonBindingsToGameSettings(On.InputHandler.orig_SendButtonBindingsToGameSettings orig, InputHandler self)
        {
            Log("Not sending button binds to game settings");
        }

        private void InputHandler_SendKeyBindingsToGameSettings(On.InputHandler.orig_SendKeyBindingsToGameSettings orig, InputHandler self)
        {
            Log("Not sending keybinds to game settings");
        }

        public override string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        
        public override void Initialize()
        {
            Log("Initializing Mod...");

            On.HeroController.Start += OnHeroControllerStart;
        }

        private void OnHeroControllerStart(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);

            self.gameObject.AddComponent<KeybindController>();
        }
    }
}