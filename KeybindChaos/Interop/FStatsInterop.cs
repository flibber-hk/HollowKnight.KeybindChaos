namespace KeybindChaos.Interop
{
    internal static class FStatsInterop
    {
        public static void HookFStats()
        {
            FStats.API.OnGenerateFile += reg => reg(new DummyStatScreen());
        }
    }
}
