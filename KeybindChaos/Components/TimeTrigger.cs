using Modding.Utils;
using System.Text;
using UnityEngine;

namespace KeybindChaos.Components
{
    public class TimeTrigger : MonoBehaviour
    {
        private TextDisplay _dm;
        private float _time;

        private void GetDisplayText(StringBuilder sb)
        {
            if (!KeybindChaos.GS.TimerEnabled) return;

            sb.AppendFormat("Keybind Reset: {0:0}s\n", _time);
        }

        void Awake()
        {
            _dm = gameObject.GetOrAddComponent<TextDisplay>();
        }

        void Start()
        {
            _time = KeybindChaos.GS.ResetTime;
            TextDisplay.OnBuildDisplayText += GetDisplayText;
            ModMenu.MenuSettingsChanged += OnSettingsChanged;
        }

        private void OnSettingsChanged()
        {
            _time = KeybindChaos.GS.ResetTime;
        }

        void Update()
        {
            if (!KeybindChaos.GS.TimerEnabled) return;

            _time -= Time.deltaTime;
            if (_time < 0f)
            {
                _time += KeybindChaos.GS.ResetTime;
                KeybindPermuter.RandomizeBinds();
            }
            _dm.SetText();
        }

        void OnDestroy()
        {
            TextDisplay.OnBuildDisplayText -= GetDisplayText;
            ModMenu.MenuSettingsChanged -= OnSettingsChanged;
        }
    }
}
