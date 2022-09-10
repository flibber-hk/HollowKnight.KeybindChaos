using MagicUI.Core;
using MagicUI.Elements;
using System;
using System.Text;
using UnityEngine;

namespace KeybindChaos.Components
{
    /// <summary>
    /// Class to manage the display text in the top right.
    /// </summary>
    public class TextDisplay : MonoBehaviour
    {
        private LayoutRoot _layout;
        private TextObject _displayText;

        /// <summary>
        /// Event invoked when setting the display text.
        /// </summary>
        public static event Action<StringBuilder> OnBuildDisplayText;

        /// <summary>
        /// Set the text according to the result of the OnBuildDisplayText event.
        /// </summary>
        public void SetText()
        {
            StringBuilder sb = new();
            OnBuildDisplayText?.Invoke(sb);
            _displayText.Text = sb.ToString().Trim();
        }

        void Awake()
        {
            if (_layout != null)
            {
                return;
            }

            _layout = new(true, "KC Timer")
            {
                VisibilityCondition = () => true
            };

            _displayText = new(_layout, "KC Text Object")
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 40,
                Font = UI.TrajanBold,
                Text = string.Empty
            };
        }

        void Start()
        {
            SetText();
            ModMenu.MenuSettingsChanged += SetText;
        }

        void OnDestroy()
        {
            _layout?.Destroy();
            _layout = null;
            _displayText = null;
            ModMenu.MenuSettingsChanged -= SetText;
        }
    }
}
