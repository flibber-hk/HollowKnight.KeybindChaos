using System.Collections.Generic;
using GlobalEnums;
using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using UnityEngine;

namespace KeybindChaos.Components
{
    public class KeybindDisplay : MonoBehaviour
    {
        private static readonly TextureLoader _textureLoader = new(typeof(KeybindDisplay).Assembly, "KeybindChaos.Resources");


        private LayoutRoot _layout;

        public void Start()
        {
            _layout = CreateLayout();
            KeybindPermuter.OnRandomize += Reset;
            KeybindPermuter.OnRestore += Reset;
        }

        public void OnDestroy()
        {
            _layout?.Destroy();
            _layout = null;
            KeybindPermuter.OnRandomize -= Reset;
            KeybindPermuter.OnRestore -= Reset;
        }

        public void Reset()
        {
            _layout?.Destroy();
            _layout = null;
            _layout = CreateLayout();
        }

        private static LayoutRoot CreateLayout()
        {
            LayoutRoot layout = new(persist: true, name: "KC bind display")
            {
                VisibilityCondition = () => !GameManager.instance.isPaused && KeybindChaos.GS.KeybindDisplay
            };

            GridLayout grid = new(layout, name: "KC bind grid")
            {
                RowDefinitions =
                {
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                },
                ColumnDefinitions =
                {
                    new GridDimension(1, GridUnit.AbsoluteMin),
                    new GridDimension(1, GridUnit.AbsoluteMin),
                },
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            foreach (ArrangableElement sprite in MakeImageSprites(layout))
            {
                grid.Children.Add(sprite);
            }

            foreach (ArrangableElement sprite in MakeKeybindSprites(layout))
            {
                grid.Children.Add(sprite);
            }

            return layout;
        }

        private static IEnumerable<ArrangableElement> MakeImageSprites(LayoutRoot layout)
        {
            int count = 0;
            foreach (string image in new[]
            {
                "Jump",
                "Slash",
                "Dash",
                "Focus",
                "Superdash",
                "Dreamnail",
                "Fireball",
                "Map"
            })
            {
                yield return new Image(layout, _textureLoader.GetTexture(image + ".png").ToSprite())
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                }.WithProp(GridLayout.Row, count).WithProp(GridLayout.Column, 0);
                count++;
            }
        }

        private static IEnumerable<ArrangableElement> MakeKeybindSprites(LayoutRoot layout)
        {
            int count = 0;

            foreach (InControl.PlayerAction action in KeybindPermuter.RetrieveBinds())
            {
                ButtonSkin skin = UIManager.instance.uiButtonSkins.GetButtonSkinFor(action);

                yield return new Image(layout, skin.sprite)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                }.WithProp(GridLayout.Row, count).WithProp(GridLayout.Column, 1);


                if (!string.IsNullOrEmpty(skin.symbol))
                {
                    int fontSize = skin.skinType switch
                    {
                        ButtonSkinType.SQUARE => skin.symbol.Length <= 2 ? 48 : 36,
                        ButtonSkinType.WIDE => 24,
                        _ => 48
                    };

                    yield return new TextObject(layout)
                    {
                        FontSize = fontSize,
                        Font = Modding.CanvasUtil.GetFont("Arial"),
                        Text = skin.symbol,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    }.WithProp(GridLayout.Row, count).WithProp(GridLayout.Column, 1);
                }

                count++;
            }
        }
    }
}
