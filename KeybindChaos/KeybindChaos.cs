using KeybindChaos.Components;
using Modding;
using Modding.Utils;
using Satchel;
using System.Reflection;
using UnityEngine.SceneManagement;

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
            Assembly asm = GetType().Assembly;
            string version = asm.GetName().Version.ToString();
            string hash = asm.GetAssemblyHash();
            return $"{version}-{hash}";
        }
        
        public override void Initialize()
        {
            Log("Initializing Mod...");

            On.HeroController.Start += EnterFileSetup;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ResetOnReturnToMenu;
        }

        private void ResetOnReturnToMenu(Scene _, Scene scene)
        {
            if (scene.name != Constants.MENU_SCENE) return;

            KeybindPermuter.RestoreBinds();
        }

        private void EnterFileSetup(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);

            self.gameObject.GetOrAddComponent<TimeTrigger>();
            self.gameObject.GetOrAddComponent<TextDisplay>();
            self.gameObject.GetOrAddComponent<KeybindDisplay>();
        }

        public bool ToggleButtonInsideMenu => false;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return ModMenu.GetMenuScreen(modListMenu);
        }
    }
}