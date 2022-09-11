using FStats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeybindChaos.Interop
{
    internal class DummyStatScreen : StatController
    {
        public int randomizationCount;

        public override void Initialize()
        {
            KeybindPermuter.OnRandomize += CountRandomization;
            API.OnBuildExtensionStats += API_OnBuildExtensionStats;
        }

        public override void Unload()
        {
            KeybindPermuter.OnRandomize -= CountRandomization;
            API.OnBuildExtensionStats -= API_OnBuildExtensionStats;
        }

        private void CountRandomization()
        {
            randomizationCount++;
        }
        private void API_OnBuildExtensionStats(Action<string> addEntry)
        {
            if (randomizationCount == 0) return;
            addEntry($"{randomizationCount} keybind shuffles");
        }

        public override IEnumerable<DisplayInfo> GetDisplayInfos() => Enumerable.Empty<DisplayInfo>();
    }
}
