using System.Collections.Generic;
using System.Linq;
using BossHandlerNamespace;
using MelonLoader;
using BTD_Mod_Helper;
using BrotherMonkey;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector2 = Il2CppAssets.Scripts.Simulation.SMath.Vector2;

[assembly: MelonInfo(typeof(BrotherMonkey.BrotherMonkey), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BrotherMonkey;


public class BrotherMonkey : BloonsTD6Mod
{
    public static bool brotherMonkey = true;
    
    public static Vector2 bossPos;

    public static bool GameStarted = false;

    public static Bloon boss;
    
    public static bool R5Plate = false;
    
    public static bool R5Plate1 = false;
    
    public static Tower brotherMonkeyTower = null;
    
    public static Tower rodrick = null;
    
    public static Tower zephyr = null;
    
    public static bool r5 = false;
    
    public static bool r30 = false;
    
    public static List<Entity> PressurePlate = new List<Entity>
    {
        new Entity(), 
        new Entity(),
    };
    
    public static List<Vector2> PressurePlatePos = new List<Vector2>
    {
        new Vector2(999, 999), 
        new Vector2(), 
    };
    
    public static List<Vector2> HealthChargerPos = new List<Vector2>
    {
        new Vector2(999, 999), 
    };
    
    public static List<Entity> HealthCharger = new List<Entity>
    {
        new Entity(), 
    };
    
    public override void OnApplicationStart()
    {
        ModHelper.Msg<BrotherMonkey>("BrotherMonkey loaded!");
    }

    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (GameStarted == false)
        {
            GameStarted = true;
        }
        
        if (tower.towerModel.baseId.Contains("rodrick"))
        {
            rodrick = tower;
            brotherMonkeyTower = tower;
        }
        
        if (tower.towerModel.baseId.Contains("zephyr"))
        {
            zephyr = tower;
            brotherMonkeyTower = tower;
        }

        if (Bosses.SellAll)
        {
            tower.worth = 0;
            tower.SellTower();
        }
    }

    public override void OnTowerSelected(Tower tower)
    {
        if (tower.towerModel.baseId.Contains("rodrick"))
        {
            brotherMonkeyTower = tower;
        }
        
        if (tower.towerModel.baseId.Contains("zephyr"))
        {
            brotherMonkeyTower = tower;
        }
    }

    public override void OnTowerDeselected(Tower tower)
    {
        if (tower.towerModel.baseId.Contains("BrotherMonkey"))
        {
        }
    }

    public override void OnNewGameModel(GameModel result, IReadOnlyList<ModModel> mods)
    {
        Story.StoryMessage[] messages = [
            new("Hi bro im rodrick the dartling bro! you should me down", "Rodrick", Story.StoryPortrait.Bro1),
            new("Hi rodrick i also exist!", "Zephyr", Story.StoryPortrait.Bro2),
            new("Right you should also place down my brother zephyr!", "Rodrick", Story.StoryPortrait.Bro1),
            new("Place down towers...", "You", Story.StoryPortrait.Player),
            new("I've also gave you the ability to move! (Select One Of Use And Use Your Movement Hotkey Defind In Mod Settins)", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
            new("Wait actually?! thats so cool", "You", Story.StoryPortrait.PlayerNoWay),
            new("I let popping write this so hope you enjoy brainrot!", "Tewtiy", Story.StoryPortrait.Tewtiy)
        ];

        Story.StoryUI.CreatePanel(messages);
    }
    
    public override void OnUpdate()
    {
        if (InGame.instance != null && GameStarted)
        {
            foreach (var bloon in InGame.instance.GetBloons())
            {
                if (bloon != null)
                {
                    if (bloon.IsMutatedBy("BloonHexModded"))
                    {
                        BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1").GetBehavior<BuffBloonSpeedModel>();
                        buff.speedBoost = 0.85f;
                        var mutator = buff.Mutator;
                        bloon.AddMutator(mutator, 180);
                    }
                }
            }
            
            var tower = brotherMonkeyTower;
            
            if (tower != null)
            {
                
                int y = (int)tower.Position.Y;
                int x = (int)tower.Position.X;

                int rody = 0;
                int rodx = 0;
                int Zephyrx = 0;
                int Zephyry = 0; 

                if (rodrick != null)
                {
                    int Rody = (int)rodrick.Position.Y;
                    int Rodx = (int)rodrick.Position.X;

                    rody = Rody;
                    rodx = Rodx;
                }

                if (zephyr != null)
                {
                    int zephyry = (int)zephyr.Position.Y;
                    int zephyrx = (int)zephyr.Position.X;

                    Zephyrx = zephyrx;
                    Zephyry = zephyry;
                }
                
                UnityEngine.Vector2 towerPosition = new UnityEngine.Vector2(x, y);
                
                UnityEngine.Vector2 zephyrPos = new UnityEngine.Vector2(Zephyrx, Zephyry);

                UnityEngine.Vector2 RodPos = new UnityEngine.Vector2(rodx, rody);

                if (PressurePlatePos[0] != new Vector2(999, 999))
                {
                    if (PressurePlate[0] != null && PressurePlate[1] != null)
                    {
                        if (UnityEngine.Vector2.Distance(RodPos, new UnityEngine.Vector2(PressurePlatePos[0].x, PressurePlatePos[0].y)) <= 30)
                        {
                            R5Plate = true;
                        }
                        else
                        {
                            R5Plate = false;
                        }

                        if (UnityEngine.Vector2.Distance(zephyrPos,
                                new UnityEngine.Vector2(PressurePlatePos[1].x, PressurePlatePos[1].y)) <= 10)
                        {
                            R5Plate1 = true;
                        }
                        else
                        {
                            R5Plate1 = false;
                        }

                        if (R5Plate == true && R5Plate1 == true)
                        {
                            PressurePlatePos[0] = new Vector2(999, 999);
                            PressurePlatePos[1] = new Vector2(999, 999);

                            PressurePlate[0].Destroy();
                            PressurePlate[1].Destroy();

                            PressurePlate[0] = null;
                            PressurePlate[1] = null;


                            if (r30 == false)
                            {
                                Gift.GiftUI.CreatePanel(500, 30);
                            }
                            else if (r30 == true)
                            {
                                Story.StoryMessage[] messages =
                                [
                                    new("Haha you fool you believe anything!", "Division",
                                        Story.StoryPortrait.DivisidonIcon),
                                    new("WAIT WHAT NOOO", "Zephyr", Story.StoryPortrait.Bro2),
                                    new("WHY IS THERE 3 ZOMGSSSS", "Rodrick", Story.StoryPortrait.Bro1),
                                ];

                                Story.StoryUI.CreatePanel(messages);

                                InGame.instance.SpawnBloons(BloonType.sZomg, 3, 50);
                            }
                            //PopupScreen.instance?.ShowOkPopup("Both Pressure Plate were triggered");
                        }
                    }
                }

                
                foreach (Vector2 p in HealthChargerPos)
                {
                    if (p != null)
                    {
                        if (p != new Vector2(999, 999))
                        {
                            if (UnityEngine.Vector2.Distance(towerPosition, new UnityEngine.Vector2(p.x, p.y)) <= 10)
                            {
                                foreach (Entity e in HealthCharger)
                                {
                                    if (e != null)
                                    {
                                        Bosses.HealthCharger = null;
                                        HealthCharger[0] = null;
                                        HealthChargerPos[0] = new(99999, 99999);
                                        e.Destroy();
                                        MelonLogger.Msg("Destroyed");
                                    }
                                }
                            }  
                        }
                    }
                } 

                if (WalkFoward.IsPressed())
                {
                    if (!(y >= 116))
                    {
                        if (Bosses.HalvedWS)
                        {
                            tower.PositionTower(new Vector2(x, y += 1));
                        }
                        else
                        {
                            tower.PositionTower(new Vector2(x, y += 2));
                        }
                    }
                    
                }
                else if (WalkBackward.IsPressed())
                {
                    if (!(y <= -109))
                    {
                        if (Bosses.HalvedWS)
                        {
                            tower.PositionTower(new Vector2(x, y -= 1));
                        }
                        else
                        {
                            tower.PositionTower(new Vector2(x, y -= 2));
                        }
                    }
                }
                else if (WalkLeft.IsPressed())
                {
                    if (!(x >= 145))
                    {
                        if (Bosses.HalvedWS)
                        {
                            tower.PositionTower(new Vector2(x += 1, y));
                        }
                        else
                        {
                            tower.PositionTower(new Vector2(x += 2, y));
                        }
                    }
                }
                else if (WalkRight.IsPressed())
                {
                    if (!(x <= -145))
                    {
                        if (Bosses.HalvedWS)
                        {
                            tower.PositionTower(new Vector2(x -= 1, y));
                        }
                        else
                        {
                            tower.PositionTower(new Vector2(x -= 2, y));
                        }
                    }
                }
            }
        }
    }
        
    public static readonly ModSettingHotkey WalkFoward = new(KeyCode.Z)
    {
        description = "",
    };
    
    public static readonly ModSettingHotkey WalkBackward = new(KeyCode.S)
    {
        description = "",
    };
    
    public static readonly ModSettingHotkey WalkLeft = new(KeyCode.D)
    {
        description = "",
    };
    
    public static readonly ModSettingHotkey WalkRight = new(KeyCode.Q)
    {
        description = "",
    };
}


public class zephyr : ModTower
{
    public override string Portrait => "Bro2";
    public override string Icon => "Bro2";

    public override TowerSet TowerSet => TowerSet.Magic;
    public override string BaseTower => TowerType.DartMonkey;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 5;
    public override int BottomPathUpgrades => 0;
    public override string Description => "";

    public override string DisplayName => "Zephyr";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.GetAttackModel().RemoveWeapon(towerModel.GetWeapon());

        towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].Duplicate());
        var attackModel = towerModel.GetWeapon();
        attackModel.projectile.pierce *= 1.2f;
        attackModel.rate += 1f;
        towerModel.ApplyDisplay<CorvusDisplay>();
        towerModel.canAlwaysBeSold = false;
        towerModel.blockSelling = true;
    }

    public override int ShopTowerCount => 1;
}

public class rodrick : ModTower
{
    public override string Portrait => "Bro1";
    public override string Icon => "Bro1";

    public override TowerSet TowerSet => TowerSet.Military;
    public override string BaseTower => TowerType.DartlingGunner;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 5;
    public override int BottomPathUpgrades => 0;
    public override string Description => "";

    public override string DisplayName => "Rodrick";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        var attackModel = towerModel.GetWeapon();
        attackModel.projectile.GetDamageModel().damage =+ 1;
        attackModel.rate += 0.2f;
        towerModel.canAlwaysBeSold = false;
        towerModel.blockSelling = true;
        towerModel.GetDescendants<RandomEmissionModel>().ForEach(e => e.angle = 0);
        towerModel.ApplyDisplay<RodrickDisplay>();
    }

    public override int ShopTowerCount => 1;
}

public class CorvusDisplay : ModDisplay
{
    public override string BaseDisplay => GetDisplay(TowerType.WizardMonkey, 3, 0, 0);
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        SetMeshOutlineColor(node, Color.blue);
        SetMeshOutlineColor(node, Color.blue, 1);
        SetMeshTexture(node, "Zephyr000");
        SetMeshTexture(node, "Zephyr000", 1);
    }
}

public class RodrickDisplay : ModDisplay
{
    public override string BaseDisplay => GetDisplay(TowerType.SniperMonkey, 0, 3, 0);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    { 
        SetMeshTexture(node , "Rodrick000");
        SetMeshTexture(node , "Rodrick000", 1);
        SetMeshOutlineColor(node, Color.red);
        SetMeshOutlineColor(node, Color.red, 1);
    }
}

[HarmonyPatch(typeof(Simulation), nameof(Simulation.RoundStart))]
public class RoundStartPacth
{
    [HarmonyPostfix]
    public static void Postfix(Simulation __instance)
    {
        {
            if (__instance.GetCurrentRound() == 4)
            {
                BrotherMonkey.r5 = true;
                
                Story.StoryMessage[] messages = [
                    new("I've placed 2 pressure plate on the map a red one and blue one both bros should be sitting on a pressure plates for a reward!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("This seems really cool am in!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("Wait What?", "Rodrick", Story.StoryPortrait.Bro1),
                ];

                Story.StoryUI.CreatePanel(messages);
                
                int entityX = 0;
                int entityY = 100;
                int entityX1 = 100;
                int entityY1 = 0;
                Entity entity = InGame.instance.bridge.simulation.SpawnEffect(ModContent.CreatePrefabReference<RodrickPlate>(), new Il2CppAssets.Scripts.Simulation.SMath.Vector3(entityX, entityY));
                BrotherMonkey.PressurePlate[0] = entity;
                
                Entity entity1 = InGame.instance.bridge.simulation.SpawnEffect(ModContent.CreatePrefabReference<zephyrPlate>(), new Il2CppAssets.Scripts.Simulation.SMath.Vector3(entityX1, entityY1));
                BrotherMonkey.PressurePlate[1] = entity1;
                
                BrotherMonkey.PressurePlatePos[0] = new Il2CppAssets.Scripts.Simulation.SMath.Vector2(entityX, entityY);
                
                BrotherMonkey.PressurePlatePos[1] = new Il2CppAssets.Scripts.Simulation.SMath.Vector2(entityX1, entityY1);
            }

            if (__instance.GetCurrentRound() == 6)
            {
                Story.StoryMessage[] messages = [
                    new("I've put you in this simulation where you can move but there is 1 issue lets just say bloon tookover the simulation and i now have no control over the simulation", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("You stand no chance against us!", "Division", Story.StoryPortrait.DivisidonIcon),
                ];
                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 9)
            {
                BrotherMonkey.r5 = false;

                if (BrotherMonkey.PressurePlate[0] != null)
                {
                    Story.StoryMessage[] messages = [
                        new("You didnt press the pressure plate in time", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                        new("L",  "You", Story.StoryPortrait.PlayerSad),
                    ];
                    
                    BrotherMonkey.PressurePlate[0].Destroy();
                    BrotherMonkey.PressurePlate[1].Destroy();

                    BrotherMonkey.PressurePlatePos[0] = new Vector2(999, 999);
                    BrotherMonkey.PressurePlatePos[1] = new Vector2(999, 999);

                    BrotherMonkey.PressurePlate[0] = null;
                    BrotherMonkey.PressurePlate[1] = null;

                    Story.StoryUI.CreatePanel(messages);
                }
                else if(BrotherMonkey.PressurePlate[0] == null)
                {
                    Story.StoryMessage[] messages = [
                        new("You pressed the pressure plate in time gg", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                        new("W", "You", Story.StoryPortrait.PlayerNoWay),
                    ];

                    Story.StoryUI.CreatePanel(messages);
                }
            }

            if (__instance.GetCurrentRound() == 14)
            {
                Story.StoryMessage[] messages = [
                    new("This first boss is all about teamwork the only way to damage it is to have the brother close to the boss otherwise all damage will get cancelled", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("This Mechanic Seems Really Advanced Did Datjanedoe make this?", "You", Story.StoryPortrait.PlayerNoWay),
                    new("No this was made by doctor monkey", "Rodrick", Story.StoryPortrait.Bro1),
                    new("No you idiot it was made by mattcy1", "Zephyr", Story.StoryPortrait.Bro2),
                    new("Can you stopp fighting and focus on the boss?", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Right", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Ye teamwork is key we need to focus", "Zephyr", Story.StoryPortrait.Bro2),
                    new("I've gained a bit of control back and can change your cash but only a tad bit so here take 1k", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Quit yapping", "You", Story.StoryPortrait.Player),
                ];

                InGame.instance.AddCash(1000);
                
                Story.StoryUI.CreatePanel(messages);
            }
            
            if (__instance.GetCurrentRound() == 19)
            {
                Story.StoryMessage[] messages = [
                    new("Ha! You wont be able to defeat me! Oh crap, you have the power of teamwork on your side!", "Division", Story.StoryPortrait.DivisidonIcon),
                    new("The only way we can win is if we work together to defeat this boss.", "Zpehyr", Story.StoryPortrait.Bro2),
                    new("You’re right! We may be weak, but if we stand by each other’s side, this is gonna be a piece of cake!", "Rodrick", Story.StoryPortrait.Bro1),
                ];

                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 29)
            {
                BrotherMonkey.r30 = true;
                
                Story.StoryMessage[] messages = [
                    new("I've placed 2 new pressure plate on the map be sure to press them", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Again? can you be orignal", "Zpehyr", Story.StoryPortrait.Bro2),
                    new("For real", "Rodrick", Story.StoryPortrait.Bro1),
                ];

                Story.StoryUI.CreatePanel(messages);
                
                int entityX = 0;
                int entityY = 100;
                int entityX1 = 50;
                int entityY1 = 25;
                Entity entity = InGame.instance.bridge.simulation.SpawnEffect(ModContent.CreatePrefabReference<RodrickPlate>(), new Il2CppAssets.Scripts.Simulation.SMath.Vector3(entityX, entityY));
                BrotherMonkey.PressurePlate[0] = entity;
                
                Entity entity1 = InGame.instance.bridge.simulation.SpawnEffect(ModContent.CreatePrefabReference<zephyrPlate>(), new Il2CppAssets.Scripts.Simulation.SMath.Vector3(entityX1, entityY1));
                BrotherMonkey.PressurePlate[1] = entity1;
                
                BrotherMonkey.PressurePlatePos[0] = new Il2CppAssets.Scripts.Simulation.SMath.Vector2(entityX, entityY);
                
                BrotherMonkey.PressurePlatePos[1] = new Il2CppAssets.Scripts.Simulation.SMath.Vector2(entityX1, entityY1);
            }
            
            if (__instance.GetCurrentRound() == 39)
            {
                Story.StoryMessage[] messages = [
                    new("Hm, this seems like a nice place to chill", "Charge-o-matic", Story.StoryPortrait.ChargeomaticIcon),
                    new("And a perfect time to set up my charging stations! I’m feeling pretty weak at the moment…", "Charge-o-matic", Story.StoryPortrait.ChargeomaticIcon),
                    new("I hope nobody presses their big red buttons to immediately deactivate them…", "Charge-o-matic", Story.StoryPortrait.ChargeomaticIcon),
                ];

                Story.StoryUI.CreatePanel(messages);
            }
            
            if (__instance.GetCurrentRound() == 49)
            {
                Story.StoryMessage[] messages = [
                    new("So uhm...", "DoctorMonkey", Story.StoryPortrait.DoctorMonkey),
                    new("Yeah what’s up?", "The bros", Story.StoryPortrait.Bro1),
                    new("So I was doing some tests on a red bloon, and now it looks like this monster with big stretchy teeth and ominous eyes… And all the rubber is converted into this goopy red fluid… It’s pretty cursed, it even tried eating me! It’s a really dangerous threat, even though it’s only a few inches wide…", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Were doomed!", "Zephyr", Story.StoryPortrait.Bro2),
                ];

                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 50)
            {
                Story.StoryMessage[] messages = [
                    new("Any new updates on upcoming enemies, Doctor Monkey?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Ah, yes, indeed! On round 60 comes a rather annoying enemy, but it shouldn’t be too difficult.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("It’s called Crystili!", "Doctor Monkey", Story.StoryPortrait.CrystalIcon),
                    new("It actually looks pretty sick ngl", "Zephyr", Story.StoryPortrait.Bro2),
                    new("No clue what it does tho", "Rodrick", Story.StoryPortrait.Bro1),
                    new("THATS BECAUSE I HAVENT FRICKIN EXPLAINED IT YET- okay, doctor monkey, take deep breaths… in… out… in… out…", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Okay, I'm fine now. Now, it’s time to tell y'all what this thing actually does, chat.", "Doctor Monkey", Story.StoryPortrait.CrystalIcon),
                    new("It uhh… it grows crystals from the ground while you attack it. So it blocks your view from attacking it.", "Doctor Monkey", Story.StoryPortrait.CrystalIcon),
                    new("How many crystals does it spawn?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("You don’t want to know.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("That’s a little concerning.", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Shlawg just deploy an Apex Plasma Master and you’re good, like *insert mewing emoji here because I don't think you can use emojis in this dialogue*", "Zephyr", Story.StoryPortrait.Bro2),
                    new("Wow, that's such a cool emoji, Zephyr. I should try it out sometime.", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Can we focus on something more productive, please?", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey)
                ];

                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 59)
            {
                Story.StoryMessage[] messages = [
                    new("*shiny, clinky rock noises*", "CRYSTILI", Story.StoryPortrait.CrystalIcon),
                    new("-DUDE YOU SCARED ME!!!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("idc haha get poopied on heheheha", "CRYSTILI", Story.StoryPortrait.CrystalIcon),
                    new("I didn’t know a shiny red crystal could be so immature.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                ];

                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 64)
            {
                Story.StoryMessage[] messages =
                [
                    new("this is really boring…", "Rodrick", Story.StoryPortrait.Bro1),
                    new("If you want I can give you some news on some other things.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("like what", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Well, I did some research through the fourth wall, and found out some things in the real world.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("alr chat stuff just got interesting!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("For example, the person who’s writing all of this dialogue is a {My lawyer has advised me not to complete this sentence}.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("DAMNNNNNNNNN THATS CRAZYYY BROOO", "Zephyr", Story.StoryPortrait.Bro2),
                    new("Dude this is so embarrassing…", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Also, the same person writing all of this, Popping Productions, is currently working on another mod with Jonyboylovespie.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("I dont know if this will still be relevant in the future but continue.", "Rodrick", Story.StoryPortrait.Bro1),
                    new("That mod turns the youtuber Tewtiy into a custom tower-", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Hasn’t that been done before?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("well yes but this time Tewtiy’s gonna be a cat maid.", "Doctor Monkey", Story.StoryPortrait.Tewtiy),
                    new("...", "Zephyr", Story.StoryPortrait.Bro2),
                    new("...what", "Zephyr", Story.StoryPortrait.Bro2),
                    new("That mod’s gonna turn out so cursed dude…", "Rodrick", Story.StoryPortrait.Bro1),
                    new("This also explains why Popping Productions is a {My lawyer has advised me not to complete this sentence}...", "Zephyr", Story.StoryPortrait.Bro2),
                    new("So far I don’t have a plan for what boss is to come on round 80..", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Let’s hope it’s nothing at all!", "Zephyr", Story.StoryPortrait.Bro2),

                ];
                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 76)
            {
                Story.StoryMessage[] messages = [
                    new("GUYS I NEED YOUR HELP IMMEDIATELY!!!!!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("What is it?!?!?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("I was soaring through a nearby galaxy with my space shuttle when I discovered a nearby planetary system, I named it TD-45.3.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("There was a highly advanced alien species on that planet… and I think I might’ve just disturbed them…", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("This can’t be good.", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Do note that these creatures have access to technology that’s equivalent to monkey technology in 65,974 years.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("dude", "Zephyr", Story.StoryPortrait.Bro2),
                    new("how the DFJHSDKFJDSLK are we supposed to deal with this", "Zephyr", Story.StoryPortrait.Bro2),
                    new("But they do have one weakness, plasma.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("why?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Their religion doesn’t believe in the 4th state of matter so their defenses lack resistance against it.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("oh cool so stuff like the dart paragon and wizard paragon deal more damage", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Indeed. Significantly more damage.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                ];

                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 79)
            {
                Story.StoryMessage[] messages = [
                    new("Shwabble dabble glibble glabble schribble shwap glab!!!", "???????", Story.StoryPortrait.QueenIcon),
                    new("dude who is this thing?!?!?!", "Rodrick", Story.StoryPortrait.Bro1),
                    new("“Scientific name, A5-867 Bloonimarimaro, or more informally known as the Superintelligent Queen.”", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("eww also was what it just say a Globgogabgalab reference?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("most definitely.", "Zephyr", Story.StoryPortrait.Bro2),
                ];

                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 87)
            {
                Story.StoryMessage[] messages = [
                    new("im bored", "Rodrick", Story.StoryPortrait.Bro1),
                    new("wanna hear a riddle?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("sure", "Rodrick", Story.StoryPortrait.Bro1),
                    new("alright… *makes hand motions while explaining the riddle*", "Zephyr", Story.StoryPortrait.Bro2),
                    new("-It bists from the ears", "Zephyr", Story.StoryPortrait.Bro2),
                    new("-It mists from the nose", "Zephyr", Story.StoryPortrait.Bro2),
                    new("-It busts out the mouth", "Zephyr", Story.StoryPortrait.Bro2),
                    new("what is it??", "Zephyr", Story.StoryPortrait.Bro2),
                    new("WHAT DOES *BISTING* EVEN MEAN?!?!?!", "Rodrick", Story.StoryPortrait.Bro1),
                    new("guess the answer to the riddle", "Zephyr", Story.StoryPortrait.Bro2),
                    new("uhhh…. Earwax, saliva, and snot mixed together?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("nope", "Zephyr", Story.StoryPortrait.Bro2),
                    new("THEN WHAT IS IT??!!", "Rodrick", Story.StoryPortrait.Bro1),
                    new("it’s mashed potatoes", "Zephyr", Story.StoryPortrait.Bro2),
                    new("uhhhh… how does that make any sense", "Rodrick", Story.StoryPortrait.Bro1),
                    new("wanna hear another one?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("do i have a choice", "Rodrick", Story.StoryPortrait.Bro1),
                    new("okay okay so it crawls under your skin, it grows in your toes, and it spawns in your eyeballs.", "Zephyr", Story.StoryPortrait.Bro2),
                    new("uhh… umm… is it like toasters or something?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("HOORAYY U GOT IT CORRECT!!!! Toasters!! uwu!!!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("i do not like this game at all", "Rodrick", Story.StoryPortrait.Bro1),
                ];
                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 90)
            {
                Story.StoryMessage[]messages = [
                    new("*sniff sniff* I smell chemical burns.", "Rodrick", Story.StoryPortrait.Bro1),
                ];

                Story.StoryUI.CreatePanel(messages);
            }
            
            if (__instance.GetCurrentRound() == 94)
            {
                Story.StoryMessage[] messages =
                [
                    new("Uff-da, it's tough work out here, dontcha think Zephyr?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("No yeah, especially considering we're leading the entire Monkey army!", "Zephyr",
                        Story.StoryPortrait.Bro2),
                    new("You betcha! Y'know what I've been craving this whole fight?", "Rodrick",
                        Story.StoryPortrait.Bro1),
                    new("Lemme guess, some tater tot hotdish?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("How'd you know man?!", "Rodrick", Story.StoryPortrait.Bro1),
                    new("It's pretty obvious how stereotypically Minnesotan you two are.", "Doctor Monkey",
                        Story.StoryPortrait.DoctorMonkey),
                    new("Oh, for Pete's sake, Doctor Monkey, give us a break!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("No time for breaks. We’re approaching the final round.", "Doctor Monkey",
                        Story.StoryPortrait.DoctorMonkey),
                    new("WOOHOOO BABYYYY!!! THAT’S WHAT I'M TALKIN' ABOUT!!!! YEAA-", "Rodrick",
                        Story.StoryPortrait.Bro1),
                    new("No time for celebration either. There’s one final challenge to complete here, guys.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("...is it another boss bloon?", "Zephyr", Story.StoryPortrait.Bro2),
                    new("What do you think dude?", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Just give us the specifics.", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Well, it seems to be some sort of…", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("...amalgamation of everything we’ve encountered before…", "Doctor Monkey",
                        Story.StoryPortrait.FinalBossIcon),
                    new("Like all the boss bloons combined into one big gooey weird-ahh lookin’ thing?", "Rodrick",
                        Story.StoryPortrait.Bro1),
                    new("Indeed you are correct.", "Doctor Monkey", Story.StoryPortrait.FinalBossIcon),
                    new("Good grief!", "Zephyr", Story.StoryPortrait.Bro2),
                    new(
                        "This anomaly I’ll name the 'AMALGAMATION-666-999', or abbreviated as the 'A-6-9', accidentally passed through my cloning machine prototype and gathered a second version of itself from a parallel universe, as shown in the diagram to the right.",
                        "Doctor Monkey", Story.StoryPortrait.FinalBossIcon),
                    new("JESUS, MARY AND JOSEPH!", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Geez Louise! You can’t reasonably expect us to deal with this… right?", "Zephyr",
                        Story.StoryPortrait.Bro2),
                    new("Would you two stop saying Minnesota slang for one second?!", "Doctor Monkey",
                        Story.StoryPortrait.DoctorMonkey),
                    new("Ok, ok, fine, we’ll stop as long as you tell us how to deal with this monstrosity.", "Zephyr",
                        Story.StoryPortrait.Bro2),
                    new(
                        "The A-6-9 and its parallel clone are able to swap positions randomly, while one moves from the entrance and the other spawns by the exit.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("What’s the purpose of that if they’re both the same?", "Rodrick", Story.StoryPortrait.Bro1),
                    new(
                        "Well, both of their health pools are linked to the same dimensional value, but damaging the Parallel version causes the health pool to increase instead of decrease.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Wait, so if it just keeps switching, doesn’t that mean that the net HP won’t change at all?",
                        "Rodrick", Story.StoryPortrait.Bro1),
                    new(
                        "Yeah we won’t be able to damage it at all! It’ll just stay the same and keep gaining more and less HP! This is impossible guys!!!",
                        "Zephyr", Story.StoryPortrait.Bro2),
                    new(
                        "Relax, relax. There are ways to avoid this. Time abilities while your towers are targeting the normal version of the boss, and change targeting so they maneuver around the parallel bloon when that side comes around.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new(
                        "Alright fine, but also won’t it spawn a bunch of other bosses? You said it’s an amalgamation of all the previous enemies we’ve encountered on our journey.",
                        "Zephyr", Story.StoryPortrait.Bro2),
                    new("Yeah, plus parallel versions too right?!?", "Rodrick", Story.StoryPortrait.Bro1),
                    new(
                        "Indeed! The normal versions are weaker than normal, and the parallel versions are dramatically weaker, but are like, 20 times as fast.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("*faints*", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Dude, Rodrick, relax! They’re so weak we can destroy them in a couple of hits!", "Zephyr",
                        Story.StoryPortrait.Bro2),
                    new("I think he fainted dude. Just leave him alone for a bit. Anywho…", "Doctor Monkey",
                        Story.StoryPortrait.DoctorMonkey),
                    new("Prepare yourselves, brothers. The final challenge awaits you in just a few rounds!",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Oh, one more question.", "Zephyr", Story.StoryPortrait.Bro2),
                    new("Ye wassup?", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new(
                        "Doesn’t the interaction of matter and antimatter make an absolutely massive explosion that results in pure photons being emitted?",
                        "Zephyr", Story.StoryPortrait.Bro2),
                    new("Oh crap! If we don’t defeat them in time, then we’re all gonna die!!!", "Zephyr",
                        Story.StoryPortrait.Bro2),
                    new(
                        "I’ve prepared both of you with special shields that will protect you from this blast. It’s 100% confirmed that this event will occur, even when the boss is destroyed they will eventually merge and combust into a pure energy form.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("*slowly starts to get up again* …huh? What about a pure energy form..? I… I need a Jucy Lucy…",
                        "Rodrick", Story.StoryPortrait.Bro1),
                    new("So what happens after that? Do all the monkeys die and we’re the only two survivors?",
                        "Zephyr", Story.StoryPortrait.Bro2),
                    new(
                        "Yes, but the boss will be much weaker by then, only consisting of pure photons. Use teamwork, agility, and strategy to defeat the boss using the power of unity!",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Anything else we should know?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Do *YOU* even know anything else about this thing?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Nope.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new(
                        "But do know, avoid ALL of A-6-9’s second form’s attacks! And under NO circumstances shall it let you be stunned! Move away from it. Every second you become stunned you lose valuable time to damage the boss, and there’s only two of you, we can’t sacrifice one of you.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Wait, have you come up with a name for it yet…?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("Uhhhh… no? But that’s not impor-", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("...C-can I name it…? Pwease?? >////< uwu I’ve been such a good bo-", "Rodrick",
                        Story.StoryPortrait.Bro1),
                    new(
                        "Don’t you fricking dare say that ever again, Rodrick, or I’ll cast a spell on you that forces you to forever watch Skibidi Toilet episodes 1 through 74 on repeat.",
                        "Zephyr", Story.StoryPortrait.Bro2),
                    new("I think that’s an adequate punishment. But back to what Rodrick said, yes, you can name it.",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("Ooh! Ooh! What about SUPERNOVA??", "Rodrick", Story.StoryPortrait.Bro1),
                    new("How about UNIFIED SUPERNOVA?", "Zephyr", Story.StoryPortrait.Bro2),
                    new(
                        "Unified Supernova it is. Be prepared to fight the Unified Supernova after defeating the Amalgamation-666-999!",
                        "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("We’re totally screwed.", "Rodrick", Story.StoryPortrait.Bro1),
                ];
                
                Story.StoryUI.CreatePanel(messages);
            }

            if (__instance.GetCurrentRound() == 99)
            {
                Story.StoryMessage[] messages = [
                    new("oh $%&#...", "Rodrick", Story.StoryPortrait.Bro1),
                    new("it’s here…", "Zephyr", Story.StoryPortrait.Bro2),
                    new("If you need me, I’ll be on Mars.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                    new("da flip?!?!?", "Rodrick", Story.StoryPortrait.Bro1),
                    new("what if we need your help?!?!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("HELLO!?!?!", "Zephyr", Story.StoryPortrait.Bro2),
                    new("i think we lost connection with him bro…", "Rodrick", Story.StoryPortrait.Bro1),
                    new("we’re so screwed…", "Rodrick", Story.StoryPortrait.Bro1),
                ];

                
                Story.StoryUI.CreatePanel(messages);
            }
        }
    }
}

public class RodrickPlate : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;
    
    public override Il2CppAssets.Scripts.Simulation.SMath.Vector3 PositionOffset => new(0, 0, 200);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, "rodrickPlate");
    }
}

public class zephyrPlate : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;
    
    public override Il2CppAssets.Scripts.Simulation.SMath.Vector3 PositionOffset => new(0, 0, 200);

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, "zephyrPlate");
    }
}
public class Prefab : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, "ChargingStation");
    }
}