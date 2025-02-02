using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using MelonLoader;
using UnityEngine;
using UnityEngine.Video;

namespace BrotherMonkey;

public class Heart
{
    [RegisterTypeInIl2Cpp(false)]
    public class HeartUI : MonoBehaviour
    {
        public static HeartUI instance = null;


        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }

        public static void CreatePanel()
        {
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 0, 0), VanillaSprites.BrownPanel);
                instance = panel.AddComponent<HeartUI>();
                var image = panel.AddImage(new("Image_", -2050, 1200, 150), ModContent.GetTextureGUID<BrotherMonkey>("BlackHeart"));
            }
        }
    }
}
