using Modding;
using Satchel.BetterMenus;

namespace KeybindChaos
{
    internal static class ModMenu
    {
        private static Menu MenuRef;

        public static MenuScreen GetMenuScreen(MenuScreen modListMenu)
        {
            MenuRef ??= new Menu(KeybindChaos.instance.GetName(), new Element[]
            {
                new HorizontalOption
                (
                    name: "Mod active",
                    description: string.Empty,
                    values: new[]{ "Off", "On" },
                    applySetting: val =>
                    {
                        KeybindChaos.GS.Enabled = val != 1;
                        if (!KeybindChaos.GS.Enabled)
                        {
                            KeybindController.Instance?.Reset();
                        }
                    },
                    loadSetting: () => KeybindChaos.GS.Enabled ? 1 : 0
                ),
                new HorizontalOption
                (
                    name: "Timer duration",
                    description: string.Empty,
                    values: new[]{ "Disabled", "30s", "1min", "2min", "5min", "10min" },
                    applySetting: val => KeybindChaos.GS.ResetTime = val switch
                    {
                        0 => null,
                        1 => 30,
                        2 => 60,
                        3 => 120,
                        4 => 300,
                        5 => 600,
                        _ => null
                    },
                    loadSetting: () => KeybindChaos.GS.ResetTime switch
                    {
                        null => 0,
                        30 => 1,
                        60 => 2,
                        120 => 3,
                        300 => 4,
                        600 => 5,
                        _ => 2
                    }
                ),
                new KeyBind("Manual Trigger", KeybindChaos.GS.Binds.ManualTrigger),
                new MenuButton("Reset", "Recover the default binds", _ => KeybindController.Instance?.Reset())
            });

            return MenuRef.GetMenuScreen(modListMenu);
        }
    }
}
