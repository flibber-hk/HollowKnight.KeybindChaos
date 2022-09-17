using Modding.Utils;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KeybindChaos.Components
{
    public class TimeTrigger : MonoBehaviour
    {
        private TextDisplay _dm;
        private float _time;
        private AudioSource _as;
        private AudioClip _clip;

        private const float audioStartTime = 4f;
        private bool _startedAudio = false;

        private void GetDisplayText(StringBuilder sb)
        {
            if (!KeybindChaos.GS.TimerEnabled) return;

            if (_time < 10f)
            {
                sb.AppendFormat("Keybind Reset: {0:0.00}\n", _time);
            }
            else
            {
                sb.AppendFormat("Keybind Reset: {0:0}s\n", Mathf.Floor(_time));
            }
        }

        void Awake()
        {
            _dm = gameObject.GetOrAddComponent<TextDisplay>();
            _as = gameObject.GetComponent<AudioSource>();
            _clip = LoadTimerAudio();
        }

        private AudioClip LoadTimerAudio()
        {
            Assembly a = typeof(TimeTrigger).Assembly;
            using (Stream resFilestream = a.GetManifestResourceStream("KeybindChaos.Resources.countdown.wav"))
            {
                using MemoryStream ms = new();
                resFilestream.CopyTo(ms);
                byte[] ba = ms.ToArray();
                return Satchel.WavUtils.ToAudioClip(ba);
            }
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
                _startedAudio = false;
                KeybindPermuter.RandomizeBinds();
            }
            if (_time < audioStartTime && !_startedAudio)
            {
                _startedAudio = true;
                if (KeybindChaos.GS.TimerAudio)
                {
                    _as.PlayOneShot(_clip);
                }
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
