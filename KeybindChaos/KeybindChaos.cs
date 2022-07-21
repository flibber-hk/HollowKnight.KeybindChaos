using Modding;

namespace KeybindChaos
{
    public class KeybindChaos : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod
    {
        internal static KeybindChaos instance;

        public static GlobalSettings GS = new();

        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
        public GlobalSettings OnSaveGlobal() => GS;

        
        public KeybindChaos() : base(null)
        {
            instance = this;
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

        public bool ToggleButtonInsideMenu => false;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return ModMenu.GetMenuScreen(modListMenu);
        }
    }
}