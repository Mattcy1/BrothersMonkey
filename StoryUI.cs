using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Map;
using Il2CppNinjaKiwi.GUTS;
using UnityEngine;
using Player = Il2CppNinjaKiwi.LiNK.Lobbies.Player;

namespace BrotherMonkey;

public class Story
{
    public struct StoryMessage(string message, string name, StoryPortrait portrait, Action onMessage = null)
    {
        public string Message = message;
        public string Name = name;
        public StoryPortrait Portrait = portrait;
        public Action OnMessage = onMessage;
    }

    public enum StoryPortrait
    {
        Bro1,
        Bro2,
        DivisidonIcon,
        DoctorMonkey,
        Player,
        PlayerNoWay,
        PlayerSad,
        ChargeomaticIcon,
        FinalBossIcon,
        QueenIcon,
        CrystalIcon,
        Tewtiy
    }


    [RegisterTypeInIl2Cpp(false)]
    public class StoryUI : MonoBehaviour
    {
        public static StoryUI instance = null;

        public static ModHelperText NameText = null;
        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }

        public static List<Action> LastMessageActions;

        public static void CreatePanel(StoryPortrait portrait, string name, string text, Action closeAction = null, bool runLastCloseAction = true)
        {
            CreatePanel(new StoryMessage(text, name, portrait), closeAction, runLastCloseAction);
        }

        public static void CreatePanel(StoryMessage msg, Action closeAction = null, bool runLastCloseAction = true)
        {
            if (InGame.instance != null)
            {
                if(instance != null)
                {
                    instance.Close();
                    if(runLastCloseAction)
                        foreach(Action action in LastMessageActions)
                        {
                            action.Invoke();
                        }
                }
                LastMessageActions = [closeAction];
                if (msg.OnMessage != null)
                {
                    LastMessageActions.Add(msg.OnMessage);
                }

                msg.OnMessage?.Invoke();

                RectTransform rect = InGame.instance.uiRect;
                var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, -1000, 1250, 600), VanillaSprites.BrownPanel);
                instance = panel.AddComponent<StoryUI>();
                var image = panel.AddImage(new("Image_", 1000, 0, 750, 750), ModContent.GetTextureGUID<BrotherMonkey>(msg.Portrait.ToString()));
                var text_ = panel.AddText(new("Title_", 0, 0, 1150, 500), $"{msg.Message}");
                if (msg.Name == "Doctor Monkey")
                {
                    var Name = panel.AddText(new ("Name_", -250, 300, 500, 250), $"{msg.Name}");
                    Name.transform.localScale *= 2f;
                    NameText = Name;
                }
                else if(msg.Name == "You")
                {
                    var Name = panel.AddText(new ("Name_", -400, 300, 500, 250), $"{Game.LiNKDisplayName}");
                    Name.transform.localScale *= 2f;
                    NameText = Name;
                }
                else
                {
                    var Name = panel.AddText(new ("Name_", -400, 300, 500, 250), $"{msg.Name}");
                    Name.transform.localScale *= 2f;
                    NameText = Name;
                }
                text_.Text.enableAutoSizing = text_;
                

                var btn = panel.AddButton(new("CloseBtn", 625, 300, 100), VanillaSprites.CloseBtn, new Action(() => { instance.Close(); closeAction?.Invoke(); }));
            }
        }

        public static void CreatePanel(StoryPortrait[] portraits, string[] texts, string[] name, Action closeAction = null, bool runLastCloseAction = true)
        {
            StoryMessage[] msgs = new StoryMessage[texts.Length];

            for (int i = 0; i < texts.Length; i++)
            {
                StoryPortrait? emotionToUse = portraits[i];
                emotionToUse ??= StoryPortrait.Bro1;

                msgs[i] = new(texts[i], name[i], (StoryPortrait)emotionToUse);
            }

            if (msgs.Length == 1)
            {
                CreatePanel(msgs[0], closeAction, runLastCloseAction);
            }
            else
            {
                CreatePanel(msgs, closeAction, runLastCloseAction);
            }
        }

        public static void CreatePanel(StoryMessage[] msgs, Action closeAction = null, bool runLastCloseAction = true)
        {
            if (instance != null)
            {
                instance.Close();
                if (runLastCloseAction)
                    foreach (Action action in LastMessageActions)
                    {
                        action.Invoke();
                    }
            }
            LastMessageActions = [closeAction];
            foreach (var msg in msgs)
            {
                if (msg.OnMessage != null)
                {
                    LastMessageActions.Add(msg.OnMessage);
                }
            }

            RectTransform rect = InGame.instance.uiRect;
            var panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, -1000, 1250, 600), VanillaSprites.BrownPanel);
            instance = panel.AddComponent<StoryUI>();
            var image = panel.AddImage(new("Image_", 1000, 0, 750, 750), ModContent.GetTextureGUID<BrotherMonkey>(msgs[0].Portrait.ToString()));
            var text_ = panel.AddText(new("Title_", 0, 0, 1150, 500), $"{msgs[0].Message}");
            if (msgs[0].Name == "Doctor Monkey")
            {
                var Name = panel.AddText(new ("Name_", -250, 300, 500, 250), $"{msgs[0].Name}");
                Name.transform.localScale *= 2f;
                NameText = Name;
            }
            else if(msgs[0].Name == "You")
            {
                var Name = panel.AddText(new ("Name_", -400, 300, 500, 250), $"{Game.LiNKDisplayName}");
                Name.transform.localScale *= 2f;
                NameText = Name;
            }
            else
            {
                var Name = panel.AddText(new ("Name_", -400, 300, 500, 250), $"{msgs[0].Name}");
                Name.transform.localScale *= 2f;
                NameText = Name;
            }
            text_.Text.enableAutoSizing = text_;

            msgs[0].OnMessage?.Invoke();

            var newMsgs = msgs.Skip(1).ToArray();

            if (newMsgs.Length == 1)
            {
                var btn = panel.AddButton(new("NextBtn", 625, 300, 100), VanillaSprites.ContinueBtn, new Action(() => { CreatePanel(newMsgs[0], closeAction, false); }));
            }
            else
            {
                var btn = panel.AddButton(new("NextBtn", 625, 300, 100), VanillaSprites.ContinueBtn, new Action(() => { CreatePanel(newMsgs, closeAction, false); }));
            }
        }
    }
}
