using System;
using Satchel.BetterMenus;

namespace KeybindChaos
{
    internal static class ModMenu
    {
        private static Menu MenuRef;

        /// <summary>
        /// Event raised when any setting that could affect the keybind randomization triggers is changed
        /// </summary>
        public static event Action MenuSettingsChanged;

        public static MenuScreen GetMenuScreen(MenuScreen modListMenu)
        {
            MenuRef ??= new Menu(KeybindChaos.instance.GetName(), new Element[]
            {
                new HorizontalOption
                (
                    name: "Timer Active",
                    description: string.Empty,
                    values: new[]{ "False", "True" },
                    applySetting: val =>
                    {
                        KeybindChaos.GS.TimerSettings.Enabled = val == 1;

                        MenuRef.Find("Timer Duration").isVisible = KeybindChaos.GS.TimerSettings.Enabled;
                        MenuRef.Find("Timer Audio").isVisible = KeybindChaos.GS.TimerSettings.Enabled;
                        MenuRef.Update();

                        MenuSettingsChanged?.Invoke();
                    },
                    loadSetting: () => KeybindChaos.GS.TimerSettings.Enabled ? 1 : 0
                ),
                new HorizontalOption
                (
                    name: "Timer Duration",
                    description: string.Empty,

                    // TODO - if they change this in the GS file manually, add the new number as an option
                    values: new[]{ "15s", "30s", "1min", "2min", "5min", "10min" },
                    applySetting: val =>
                    {
                        KeybindChaos.GS.TimerSettings.ResetTime = val switch
                        {
                            0 => 15,
                            1 => 30,
                            2 => 60,
                            3 => 120,
                            4 => 300,
                            5 => 600,
                            _ => 60
                        };
                        MenuSettingsChanged?.Invoke();
                    },
                    loadSetting: () => KeybindChaos.GS.TimerSettings.ResetTime switch
                    {
                        10 => 0,
                        30 => 1,
                        60 => 2,
                        120 => 3,
                        300 => 4,
                        600 => 5,
                        _ => 2
                    }
                ) { isVisible = KeybindChaos.GS.TimerSettings.Enabled },
                new HorizontalOption
                (
                    name: "Timer Audio",
                    description: "Whether to play sound near the end of the timer",
                    values: new[]{ "False", "True" },
                    applySetting: val => 
                    {
                        KeybindChaos.GS.TimerSettings.Audio = val == 1;
                        MenuSettingsChanged?.Invoke();
                    },
                    loadSetting: () => KeybindChaos.GS.TimerSettings.Audio ? 1 : 0
                ) { isVisible = KeybindChaos.GS.TimerSettings.Enabled },
                new HorizontalOption
                (
                    name: "Display binds",
                    description: string.Empty,
                    values: new[]{ "False", "True" },
                    applySetting: val =>
                    {
                        KeybindChaos.GS.KeybindDisplay = val == 1;
                    },
                    loadSetting: () => KeybindChaos.GS.KeybindDisplay ? 1 : 0

                ),
                new KeyBind("Manual Trigger", KeybindChaos.GS.Binds.ManualTrigger),
                new MenuButton("Reset", "Recover the saved binds", _ => KeybindPermuter.RestoreBinds())
            });

            return MenuRef.GetMenuScreen(modListMenu);
        }
    }
}
