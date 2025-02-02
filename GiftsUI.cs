using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using MelonLoader;
using UnityEngine;
using UnityEngine.Video;

namespace BrotherMonkey;

public class Gift
{
    [RegisterTypeInIl2Cpp(false)]
    public class GiftUI : MonoBehaviour
    {
        public static GiftUI instance = null;


        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }

        public static void CreatePanel(int Cash, double Lives)
        {
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 0, 0), VanillaSprites.BrownPanel);
                instance = panel.AddComponent<GiftUI>();
                var image = panel.AddImage(new("Image_", 0, 0, 1000), VanillaSprites.GiftBlue);
                var Claim = image.AddButton(new("Button_", 0, -500, 450, 450 / 2), VanillaSprites.GreenBtnLong, new System.Action(() =>
                {
                    InGame.instance.AddCash(Cash);
                    InGame.instance.AddHealth(Lives);
                    instance.Close();
                    PopupScreen.instance?.ShowOkPopup($"You were rewarded with {Cash}$ and {Lives} Lives");
                }));
                Claim.AddText(new("Title_", 0, 0, 300, 150), "CLAIM!", 70);
            }
        }
    }
}
