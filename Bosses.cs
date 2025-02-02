using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Unity.Scenes;
using Il2CppAssets.Scripts.Unity;

using static BossHandlerNamespace.BossHandler;
using Harmony;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using UnityEngine;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Simulation.Bloons;
using System.Runtime.InteropServices;
using BrotherMonkey;
using BTD_Mod_Helper.Api;using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Data.Boss;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Props;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Utils;
using UnityEngine.InputSystem.Utilities;
using Bosses = BossHandlerNamespace.Bosses;
using Color = UnityEngine.Color;
using Math = Il2CppSystem.Math;
using Random = System.Random;
using Vector2 = Il2CppAssets.Scripts.Simulation.SMath.Vector2;

namespace BossHandlerNamespace
{

    class ChargeomaticDisplay : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "Charge-o-matic";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.blue);
            }
        }
    }

    class RockyRocksTexture : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "crystalrock";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.red);
            }
        }
    }

    class SupernovaDisplay : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "UnifiedSupernova";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.white);
            }
        }
    }

    class FinalBoss1 : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "NormalAmalgamation-666-999";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.white);
            }
        }
    }

    class FinalBoss : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "ParellelAmalgamation-666-999";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.black);
            }
        }
    }

    class DivisidonDisplay : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "Divisidon";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.blue);
            }
        }
    }

    class CrystalDisplay : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "CrystalBoss";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.red);
            }
        }
    }


    class AlienDisplay : ModCustomDisplay
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "AlienQueen";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.ApplyOutlineShader();
                renderer.SetOutlineColor(Color.green);
            }
        }
    }

    class RedBad : ModDisplay
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("Bad").display.guidRef;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, "RedBadMain");
            SetMeshTexture(node, "RedBadMain", 1);
            SetMeshTexture(node, "RedBadMain", 2);
        }
    }

    internal class Bosses
    {
        public static bool SellAll = false;
        public static string CustomBoss = null;
        public static string tier = "";
        public static int fakeHealth = 5000;
        public static int fakeMaxHealth = 5000;
        public static bool canMerge = false;
        public static UnityEngine.Vector2 FianlBoss1Pos = new UnityEngine.Vector2(999, 999);
        public static UnityEngine.Vector2 FianlBossPos = new UnityEngine.Vector2(9999, 9999);
        public static Bloon FinalBoss = null;
        public static Bloon FinalBoss1 = null;
        public static float Perc;
        public static float Perc1;
        public static Bloon totem = null;
        public static Entity HealthCharger = null;
        public static int ExtraFakeHealth = 0;
        public static bool heal = false;
        public static readonly System.Random random = new System.Random();
        public static int bossState = 0;
        public static bool DamageActive = true;
        public static bool HalvedWS = false;
        
        [HarmonyPatch(typeof(TitleScreen), nameof(TitleScreen.Start))]
        public class TitleScreenInit
        {
            [HarmonyPostfix]

            public static void Postfix()
            {
                BloonModel Divisidon = CreateBossBase(5000, 0.7f);

                BloonModel Chargeomatic = CreateBossBase(5000000, 1.5f);

                BloonModel Crystyli = CreateBossBase(5000000, Game.instance.model.GetBloon("Lych1").speed);

                BloonModel RedBad = CreateBossBase(5000000, Game.instance.model.GetBloon("Bad").speed);

                BloonModel Bloonimarimaro = CreateBossBase(5000000, Game.instance.model.GetBloon("Vortex1").speed / 2);

                BloonModel Nova = CreateBossBase(5000000, Game.instance.model.GetBloon("Vortex1").speed);

                Nova.ApplyDisplay<SupernovaDisplay>();
                Divisidon.ApplyDisplay<DivisidonDisplay>();

                /* Registers the boss into BossHandler.

                When a bloon registered via BossRegistration spawns, it will run BossInit
                allowing you to run any other code the Bloon needs and/or start a monobehavior

                If the isMainBoss property is set to true (its true by default), the boss UI will display the bosses display name, icon, and health.
                If the description property has text, you can toggle seeing the description ingame.

                If continueRounds is greater than 0, then rounds will continue to be sent while the boss is on screen
                up to the continueRounds value. For example, if the boss spawns on round 40 with a continueRounds value of 9,
                the boss will allow rounds to be sent until round 49 is reached. Rounds continue as normal once the boss is popped
                This value is 0 by default.

                The returned BossRegistration object can be altered further.

                SizeX/SizeY: Dimensions of the description box
                UsesExtraInfo: Adds extra UI info under the health bar.
                ExtraInfoIcon: The icon used for the extra UI
                ExtraInfoText: The initial text next to the extra UI icon.

                You can change the initial text anytime by changing bossPanel.extraText
                */



                BossRegisteration divisidonRegisteration = new BossRegisteration(Divisidon, "DivisidonBoss",
                    "Divisidon", true, "DivisidonIcon", 10,
                    "A digital being with full and utter hatred for unity, hense the name divisidon. It's so far willed against the theme that it's weakened it's own ability to destroy Monkey kind, and now lives to be an empty machine of 1's and 0's. If it wasn't hardcoded so much against the simple theme of \"unity\", it could have had millions of HP, but here it stands with only 5000.",
                    20);

                TimeTriggerModel time = new TimeTriggerModel("SpawnTotem", 60, false, new string[] { "SpawnTotem" });

                TimeTriggerModel time1 = new TimeTriggerModel("ShootFireballDivisidon", 45, false,
                    new string[] { "ShootFireballDivisidon" });

                FireballActionModel targetModel = Game.instance.model.GetBloon("Blastapopoulos1")
                    .GetBehavior<FireballActionModel>().Duplicate();
                targetModel.fireballAmount = 1;
                targetModel.stunDuration = 20;
                targetModel.projectileSpeed /= 2;
                targetModel.actionId = "ShootFireballDivisidon";
                Divisidon.AddBehavior(time1);
                Divisidon.AddBehavior(targetModel);
                Divisidon.AddBehavior(time);

                divisidonRegisteration.usesHealthOverride = true;
                divisidonRegisteration.fakeHealth = 7500;
                divisidonRegisteration.fakeMaxHealth = 7500;
                divisidonRegisteration.SpawnOnRound(20);

                BossRegisteration ChargeoMaticRegisteration = new BossRegisteration(Chargeomatic, "ChargeoMaticBoss",
                    "Charge-o-matic", true, "ChargeomaticIcon", 10,
                    "", 20);

                TimeTriggerModel SpawnCharger =
                    new TimeTriggerModel("SpawnCharger", 15, false, new string[] { "SpawnCharger" });


                Chargeomatic.AddBehavior(SpawnCharger);
                ChargeoMaticRegisteration.usesHealthOverride = true;
                ChargeoMaticRegisteration.fakeHealth = 15000;
                ChargeoMaticRegisteration.fakeMaxHealth = 15000;
                ChargeoMaticRegisteration.SpawnOnRound(40);
                Chargeomatic.ApplyDisplay<ChargeomaticDisplay>();
                Chargeomatic.disallowCosmetics = true;


                BossRegisteration crystyliRegisteration = new BossRegisteration(Crystyli, "CrystyliBoss",
                    "Crystyli", true, "CrystyliIcon", 10,
                    "s a mischievous boss bloon that loves placing blockers around all of your monkeys to protect itself. If it’s protected for too long, it might start to recrystalyze and regenerate tons of HP.",
                    20);

                TimeTriggerModel HealSpeed = new TimeTriggerModel("HealSpeed", 20, false, new string[] { "HealSpeed" });


                TimeTriggerModel RockyRocks =
                    new TimeTriggerModel("RockyRocks", 20, false, new string[] { "RockyRocks" });

                TimeTriggerModel ToManyRockyRocks =
                    new TimeTriggerModel("ToManyRockyRocks", 1, false, new string[] { "ToManyRockyRocks" });

                CreatePropsOnBloonActionModel rocks = Game.instance.model.GetBloon("Blastapopoulos1")
                    .GetBehavior<CreatePropsOnBloonActionModel>().Duplicate();

                rocks.actionId = "RockyRocks";
                rocks.rockAmount = 20;
                rocks.rockDuration = 40f;

                CreatePropsOnBloonActionModel rocks1 = Game.instance.model.GetBloon("Blastapopoulos1")
                    .GetBehavior<CreatePropsOnBloonActionModel>().Duplicate();

                rocks1.actionId = "ToManyRockyRocks";
                rocks1.rockAmount = 3;
                rocks1.rockDuration = 3f;

                Crystyli.AddBehavior(HealSpeed);
                Crystyli.AddBehavior(RockyRocks);
                Crystyli.AddBehavior(rocks);
                Crystyli.AddBehavior(ToManyRockyRocks);
                Crystyli.AddBehavior(rocks1);
                crystyliRegisteration.usesHealthOverride = true;
                crystyliRegisteration.fakeHealth = 250000;
                crystyliRegisteration.fakeMaxHealth = 250000;
                crystyliRegisteration.SpawnOnRound(60);


                ChargeoMaticRegisteration.usesHealthOverride = true;
                ChargeoMaticRegisteration.fakeHealth = 15000;
                ChargeoMaticRegisteration.fakeMaxHealth = 15000;
                ChargeoMaticRegisteration.SpawnOnRound(40);
                Chargeomatic.ApplyDisplay<ChargeomaticDisplay>();
                Chargeomatic.disallowCosmetics = true;

                BossRegisteration badRegrisRegisteration = new BossRegisteration(RedBad, "RedBad",
                    "The Big Red Behemoth", true, "TheBigRedBehemothIcon", 10,
                    "", 20);

                badRegrisRegisteration.usesHealthOverride = true;
                badRegrisRegisteration.fakeHealth = 100000;
                badRegrisRegisteration.fakeMaxHealth = 5000000;
                badRegrisRegisteration.SpawnOnRound(50);
                badRegrisRegisteration.usesExtraInfo = true;
                RedBad.disallowCosmetics = true;



                TimeTriggerModel SellsTower =
                    new TimeTriggerModel("SellsTower", 30, false, new string[] { "SellsTower" });

                RedBad.AddBehavior(SellsTower);

                Bloonimarimaro.disallowCosmetics = true;

                BossRegisteration BloonimarimaroRegristration = new BossRegisteration(Bloonimarimaro, "Queen",
                    "A5-867 Bloonimarimaro, the Superintelligent Queen", true, "QueenIcon", 10,
                    "", 20);

                BloonimarimaroRegristration.usesHealthOverride = true;
                BloonimarimaroRegristration.fakeHealth = 35 * 100000;
                BloonimarimaroRegristration.fakeMaxHealth = 35 * 100000;
                BloonimarimaroRegristration.SpawnOnRound(80);

                TimeTriggerModel MoveTower =
                    new TimeTriggerModel("MoveTower", 15, false, new string[] { "MoveTower" });

                TimeTriggerModel SpawnBosses =
                    new TimeTriggerModel("SpawnBosses", 80, false, new string[] { "SpawnBosses" });

                Bloonimarimaro.AddBehavior(SpawnBosses);
                Bloonimarimaro.AddBehavior(MoveTower);

                BloonModel FinalBoss = CreateBossBase(10000000, 0.7f);

                BloonModel FinalBoss1 = CreateBossBase(10000000, 0.7f);

                BossRegisteration FinalBossRegristation = new BossRegisteration(FinalBoss, "TrueBoss",
                    "Amalgamation-666-999", true, "FinalBossIcon", 20,
                    "Spawns 1 Moabs For Each Health Over Max Health, Every Moabs Upgrades Every 20s", 20);

                BossRegisteration FinalBoss1Regristation = new BossRegisteration(FinalBoss1, "FalseBoss",
                    "Get Cloned", false, "", 20,
                    "", 20);

                FinalBoss1Regristation.SpawnOnRound(99999999);
                FinalBossRegristation.SpawnOnRound(100);

                FinalBoss1.ApplyDisplay<ChargeomaticDisplay>();

                TimeTriggerModel SpawnsDDT =
                    new TimeTriggerModel("SpawnsDDT", 30, false, new string[] { "SpawnsDDT" });

                FinalBoss.AddBehavior(SpawnsDDT);


                TimeTriggerModel UpgradesMoabs =
                    new TimeTriggerModel("UpgradesMoabs", 25, false, new string[] { "UpgradesMoabs" });

                FinalBoss.AddBehavior(UpgradesMoabs);

                TimeTriggerModel Swap =
                    new TimeTriggerModel("Swap", 40, false, new string[] { "Swap" });

                TimeTriggerModel FinalRocks =
                    new TimeTriggerModel("FinalRocks", 5, false, new string[] { "FinalRocks" });

                CreatePropsOnBloonActionModel FinalRockProps = Game.instance.model.GetBloon("Blastapopoulos1")
                    .GetBehavior<CreatePropsOnBloonActionModel>().Duplicate();

                FinalRockProps.actionId = "FinalRocks";
                FinalRockProps.rockAmount = 5;
                FinalRockProps.rockDuration = 40f;

                TimeTriggerModel FinalRocks1 =
                    new TimeTriggerModel("FinalRocks1", 5, false, new string[] { "FinalRocks1" });

                TimeTriggerModel FinalMoveTowers =
                    new TimeTriggerModel("FinalMoveTowers", 15, false, new string[] { "FinalMoveTowers" });

                TimeTriggerModel ChargingCharger =
                    new TimeTriggerModel("ChargingCharger", 45, false, new string[] { "ChargingCharger" });


                CreatePropsOnBloonActionModel FinalRockProps1 = Game.instance.model.GetBloon("Blastapopoulos1")
                    .GetBehavior<CreatePropsOnBloonActionModel>().Duplicate();

                FinalRockProps1.actionId = "FinalRocks1";
                FinalRockProps1.rockAmount = 5;
                FinalRockProps1.rockDuration = 40f;


                FinalBoss.AddBehavior(Swap);
                FinalBoss.AddBehavior(FinalRocks);
                FinalBoss.AddBehavior(FinalRockProps);
                FinalBoss.AddBehavior(FinalMoveTowers);
                FinalBoss.AddBehavior(ChargingCharger);
                FinalBoss1.AddBehavior(FinalRocks1);
                FinalBoss1.AddBehavior(FinalRockProps1);
                RedBad.ApplyDisplay<RedBad>();

                FinalBoss1Regristation.usesHealthOverride = true;
                FinalBossRegristation.usesHealthOverride = true;

                Crystyli.ApplyDisplay<CrystalDisplay>();
                Bloonimarimaro.ApplyDisplay<AlienDisplay>();
                FinalBoss.ApplyDisplay<FinalBoss1>();
                FinalBoss1.ApplyDisplay<FinalBoss>();

                BossRegisteration NovaRegis = new BossRegisteration(Nova, "NovaBoss",
                    "Unified Supernova", true, "NovaIcon", 20,
                    "",
                    20);

                foreach (var bloon in Game.instance.model.bloons)
                {
                    if (bloon.isBoss)
                    {
                        bloon.disallowCosmetics = true;
                    }
                }

                rocks.name = "ModdedRocks";
                rocks1.name = "ModdedRocks";
                FinalRockProps1.name = "ModdedRocks";
                FinalRockProps.name = "ModdedRocks";

                NovaRegis.usesHealthOverride = true;
                NovaRegis.fakeMaxHealth = 5000000;
                NovaRegis.fakeHealth = 5000000;
                
                TimeTriggerModel NovaTimer =
                    new TimeTriggerModel("Drain", 1, false, new string[] { "Drain" });
                TimeTriggerModel ImmuneState =
                    new TimeTriggerModel("ImmuneState", 25, false, new string[] { "ImmuneState" });
                TimeTriggerModel NovaDDT =
                    new TimeTriggerModel("NovaDDT", 20, false, new string[] { "NovaDDT" });
                TimeTriggerModel Unstun =
                    new TimeTriggerModel("Unstun", 5, false, new string[] { "Unstun" });
                TimeTriggerModel ActiveDamage =
                    new TimeTriggerModel("ActiveDamage", 10, false, new string[] { "ActiveDamage" });
                TimeTriggerModel SpawnsZomgs =
                    new TimeTriggerModel("SpawnsZomgs", 10, false, new string[] { "SpawnsZomgs" });
                Nova.AddBehavior(ActiveDamage);
                Nova.AddBehavior(Unstun);
                Nova.AddBehavior(ImmuneState);
                Nova.AddBehavior(NovaDDT);
                Nova.AddBehavior(NovaTimer);
                Nova.AddBehavior(SpawnsZomgs);
            }
        }

        public static void SpawnHealthCharged()
        {
            //-145 145 116 -109

            System.Random rand = new(System.Random.Shared.Next());

            var y = rand.Next(-100, 100);

            var x = rand.Next(-100, 100);

            int entityX = x;
            int entityY = y;
            Entity entity = InGame.instance.bridge.simulation.SpawnEffect(ModContent.CreatePrefabReference<Prefab>(),
                new Il2CppAssets.Scripts.Simulation.SMath.Vector3(entityX, entityY));
            HealthCharger = entity;
            BrotherMonkey.BrotherMonkey.HealthCharger[0] = entity;
            BrotherMonkey.BrotherMonkey.HealthChargerPos[0] = new Vector2(entityX, entityY);
        }

        public static void BossInit(Bloon bloon, BloonModel bloonModel, BossRegisteration registration)
        {
            if (bloonModel.id.Contains("Divisidon"))
            {
                DivisidonBehaviour mono = StartMonobehavior<DivisidonBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("ChargeoMaticBoss"))
            {
                ChargeomaticBehaviour mono = StartMonobehavior<ChargeomaticBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("Crystyli"))
            {
                ChargeomaticBehaviour.CrystalBehaviour mono =
                    StartMonobehavior<ChargeomaticBehaviour.CrystalBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("RedBad"))
            {
                ChargeomaticBehaviour.RedBadBehaviour mono =
                    StartMonobehavior<ChargeomaticBehaviour.RedBadBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("Queen"))
            {
                ChargeomaticBehaviour.QueenBehaviour mono =
                    StartMonobehavior<ChargeomaticBehaviour.QueenBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("FalseBoss"))
            {
                FinalBoss1Behaviour mono =
                    StartMonobehavior<FinalBoss1Behaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("TrueBoss"))
            {
                FinalBossBehaviour mono =
                    StartMonobehavior<FinalBossBehaviour>();

                mono.boss = bloon;
                mono.registration = registration;
            }

            if (bloonModel.id.Contains("NovaBoss"))
            {
                SuperNovaBehaviours mono =
                    StartMonobehavior<SuperNovaBehaviours>();

                mono.boss = bloon;
                mono.registration = registration;
            }
        }

        [RegisterTypeInIl2Cpp]
        public class DivisidonBehaviour : MonoBehaviour
        {
            public Bloon boss;
            public BossRegisteration registration;

            public DivisidonBehaviour() : base()
            {

            }

            public void Start()
            {
                BrotherMonkey.BrotherMonkey.boss = boss;
            }

            public void Update()
            {
                if (boss != null)
                {
                    var x = boss.Position.X;
                    var y = boss.Position.Y;
                    BrotherMonkey.BrotherMonkey.bossPos = new Vector2(x, y);

                    registration.fakeHealth = fakeHealth;
                    registration.fakeMaxHealth = fakeMaxHealth;

                    int Rody = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.Y;
                    int Rodx = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.X;

                    int zephyry = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.Y;
                    int zephyrx = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.X;

                    UnityEngine.Vector2 zephyrPos = new UnityEngine.Vector2(zephyrx, zephyry);

                    UnityEngine.Vector2 RodPos = new UnityEngine.Vector2(Rodx, Rody);

                    if (BrotherMonkey.BrotherMonkey.boss != null)
                    {
                        var bossX = BrotherMonkey.BrotherMonkey.boss.Position.X;
                        var bossY = BrotherMonkey.BrotherMonkey.boss.Position.Y;
                        var bossPos = BrotherMonkey.BrotherMonkey.boss.Position;
                        
                        if (UnityEngine.Vector2.Distance(RodPos, new UnityEngine.Vector2(bossX, bossY)) <= 75 && UnityEngine.Vector2.Distance(zephyrPos, new UnityEngine.Vector2(bossX, bossY)) <= 75)
                        {
                            if (totem == null)
                            {
                                if (boss.health < boss.bloonModel.maxHealth)
                                {
                                    fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                                }

                                boss.health = boss.bloonModel.maxHealth;

                                fakeHealth = Math.Max(0, fakeHealth);
                                bossPanel.healthBar = "healthBar";
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            if (totem == null)
                            {
                                bossPanel.healthBar = PHAYZEBAR;
                            }
                        }
                    }
                    
                    if (fakeHealth <= 0)
                    {
                        boss.Destroy();
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }
    }

    [RegisterTypeInIl2Cpp]
    public class ChargeomaticBehaviour : MonoBehaviour
    {
        public Bloon boss;
        public BossRegisteration registration;

        public ChargeomaticBehaviour() : base()
        {

        }

        public void Start()
        {
            BrotherMonkey.BrotherMonkey.boss = boss;
            Bosses.fakeHealth = 5000;
            Bosses.fakeMaxHealth = 5000;
            Bosses.SpawnHealthCharged();
        }

        public void Update()
        {
            if (boss != null)
            {
                var x = boss.Position.X;
                var y = boss.Position.Y;
                BrotherMonkey.BrotherMonkey.bossPos = new Vector2(x, y);

                registration.fakeHealth = Bosses.fakeHealth;
                registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                int Rody = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.Y;
                int Rodx = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.X;

                int zephyry = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.Y;
                int zephyrx = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.X;

                UnityEngine.Vector2 zephyrPos = new UnityEngine.Vector2(zephyrx, zephyry);

                UnityEngine.Vector2 RodPos = new UnityEngine.Vector2(Rodx, Rody);

                var bossX = BrotherMonkey.BrotherMonkey.boss.Position.X;
                var bossY = BrotherMonkey.BrotherMonkey.boss.Position.Y;
                var bossPos = BrotherMonkey.BrotherMonkey.boss.Position;

                if (UnityEngine.Vector2.Distance(RodPos, new UnityEngine.Vector2(bossX, bossY)) <= 75 &&
                    UnityEngine.Vector2.Distance(zephyrPos, new UnityEngine.Vector2(bossX, bossY)) <= 75)
                {
                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                    }

                    boss.health = boss.bloonModel.maxHealth;

                    Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);
                    bossPanel.healthBar = "healthBar";
                }
                else
                {
                    bossPanel.healthBar = PHAYZEBAR;
                }

                if (Bosses.fakeHealth <= 0)
                {
                    boss.Destroy();
                }


                if (Bosses.HealthCharger != null)
                {
                    if (Bosses.random.Next(5) == 0)
                    {
                        Bosses.fakeMaxHealth += 10;
                    }

                    if (Bosses.random.Next(10) == 0)
                    {
                        Bosses.fakeHealth += 10;
                    }
                }
            }
            else
            {
                this.Destroy();
            }
        }

        [RegisterTypeInIl2Cpp]
        public class CrystalBehaviour : MonoBehaviour
        {
            public Bloon boss;
            public BossRegisteration registration;

            public CrystalBehaviour() : base()
            {

            }

            public void Start()
            {
                BrotherMonkey.BrotherMonkey.boss = boss;
                Bosses.fakeHealth = 250000;
                Bosses.fakeMaxHealth = 250000;
            }

            public void Update()
            {
                if (boss != null && BrotherMonkey.BrotherMonkey.boss != null)
                {
                    var x = boss.Position.X;
                    var y = boss.Position.Y;
                    BrotherMonkey.BrotherMonkey.bossPos = new Vector2(x, y);

                    registration.fakeHealth = Bosses.fakeHealth;
                    registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                    int Rody = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.Y;
                    int Rodx = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.X;

                    int zephyry = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.Y;
                    int zephyrx = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.X;

                    UnityEngine.Vector2 zephyrPos = new UnityEngine.Vector2(zephyrx, zephyry);

                    UnityEngine.Vector2 RodPos = new UnityEngine.Vector2(Rodx, Rody);

                    var bossX = BrotherMonkey.BrotherMonkey.boss.Position.X;
                    var bossY = BrotherMonkey.BrotherMonkey.boss.Position.Y;
                    var bossPos = BrotherMonkey.BrotherMonkey.boss.Position;

                    if (UnityEngine.Vector2.Distance(RodPos, new UnityEngine.Vector2(bossX, bossY)) <= 75 && UnityEngine.Vector2.Distance(zephyrPos, new UnityEngine.Vector2(bossX, bossY)) <= 75)
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;

                        Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);
                        bossPanel.healthBar = "healthBar";
                    }
                    else
                    {
                        bossPanel.healthBar = PHAYZEBAR;
                    }

                    if (Bosses.fakeHealth <= 0)
                    {
                        boss.Destroy();
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }

        [RegisterTypeInIl2Cpp]
        public class QueenBehaviour : MonoBehaviour
        {
            public Bloon boss;
            public BossRegisteration registration;

            public QueenBehaviour() : base()
            {

            }

            public void Start()
            {
                BrotherMonkey.BrotherMonkey.boss = boss;
                Bosses.fakeHealth = 3500000;
                Bosses.fakeMaxHealth = 3500000;
            }

            public void Update()
            {
                if (boss != null && BrotherMonkey.BrotherMonkey.boss != null)
                {
                    var RandomBars = System.Random.Shared.Next(3);
                    if (System.Random.Shared.Next(20) == 0)
                    {
                        bossPanel.textBox.Text.SetText(System.Random.Shared.Next(0, 3500000) + " /" + System.Random.Shared.Next(0, 3500000));
                    }

                    if (System.Random.Shared.Next(20) == 0)
                    {
                        if (RandomBars == 0)
                        {
                            bossPanel.healthBar = PHAYZEBAR;
                        }
                        else if (RandomBars == 1)
                        {
                            bossPanel.healthBar = DREADBAR;
                        }
                        else if (RandomBars == 2)
                        {
                            bossPanel.healthBar = "healthBar";
                        }
                    }

                    var x = boss.Position.X;
                    var y = boss.Position.Y;
                    BrotherMonkey.BrotherMonkey.bossPos = new Vector2(x, y);

                    registration.fakeHealth = Bosses.fakeHealth;
                    registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                    int Rody = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.Y;
                    int Rodx = (int)BrotherMonkey.BrotherMonkey.rodrick.Position.X;

                    int zephyry = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.Y;
                    int zephyrx = (int)BrotherMonkey.BrotherMonkey.zephyr.Position.X;

                    UnityEngine.Vector2 zephyrPos = new UnityEngine.Vector2(zephyrx, zephyry);

                    UnityEngine.Vector2 RodPos = new UnityEngine.Vector2(Rodx, Rody);

                    var bossX = BrotherMonkey.BrotherMonkey.boss.Position.X;
                    var bossY = BrotherMonkey.BrotherMonkey.boss.Position.Y;
                    var bossPos = BrotherMonkey.BrotherMonkey.boss.Position;

                    if (UnityEngine.Vector2.Distance(RodPos, new UnityEngine.Vector2(bossX, bossY)) <= 75 && UnityEngine.Vector2.Distance(zephyrPos, new UnityEngine.Vector2(bossX, bossY)) <= 75 && Bosses.CustomBoss == null)
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;

                        Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);
                        //bossPanel.healthBar = "healthBar";
                    }
                    else
                    {
                        if (Bosses.CustomBoss == "Alive")
                        {
                            MelonLogger.Msg("Stopped Damaging Vanilla Bosses Are Alive");
                        }
                    }

                    if (Bosses.fakeHealth <= 0)
                    {
                        boss.Destroy();
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }



        [RegisterTypeInIl2Cpp]
        public class RedBadBehaviour : MonoBehaviour
        {
            public Bloon boss;
            public BossRegisteration registration;
            public float SpeedMultiplier = 1f;

            public RedBadBehaviour() : base()
            {

            }

            public void Start()
            {
                Bosses.fakeHealth = 100000;
                Bosses.fakeMaxHealth = 5000000;
                SpeedMultiplier = 1f;
                BrotherMonkey.BrotherMonkey.boss = boss;
            }

            public void Update()
            {
                if (boss != null)
                {
                    //registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                    if (System.Random.Shared.Next(0, 10) == 0)
                    {
                        Bosses.fakeHealth += 10;
                    }

                    registration.fakeHealth = Bosses.fakeHealth;
                    registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                    if (TimeManager.FastForwardActive == true)
                    {
                        SpeedMultiplier += 3 * 0.0001f;
                    }
                    else
                    {
                        SpeedMultiplier += 1 * 0.0001f;
                    }


                    boss.trackSpeedMultiplier = (float)SpeedMultiplier;

                    registration.usesExtraInfo = true;
                    registration.extraInfoText = "Battery: " + (SpeedMultiplier - 1) * 100 + "%";

                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                    }

                    boss.health = boss.bloonModel.maxHealth;

                    Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);

                    if (Bosses.fakeHealth <= 0)
                    {
                        boss.Destroy();
                        BrotherMonkey.BrotherMonkey.boss = null;
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }
    }

    [RegisterTypeInIl2Cpp]
    public class FinalBossBehaviour : MonoBehaviour
    {
        public Bloon boss;
        public BossRegisteration registration;
        public bool ForceStopMerge = false;

        public FinalBossBehaviour() : base()
        {

        }

        public void Start()
        {
            Bosses.fakeHealth = 10000000;
            Bosses.fakeMaxHealth = 10000000;
            Bosses.FinalBoss = boss;
            InGame.instance.SpawnBloons("FalseBoss", 1, 0);
        }

        public void Update()
        {
            if (boss != null)
            {
                Bosses.FianlBossPos = new UnityEngine.Vector2(boss.Position.X, boss.Position.Z);

                if (UnityEngine.Vector2.Distance(Bosses.FianlBossPos, Bosses.FianlBoss1Pos) <= 20 &&
                    ForceStopMerge == false)
                {
                    if (Bosses.canMerge)
                    {
                        Bosses.SellAll = true;
                        foreach (var tower in InGame.instance.GetTowers())
                        {
                            if (tower.towerModel.baseId != ModContent.TowerID<rodrick>())
                            {
                                if (tower.towerModel.baseId != ModContent.TowerID<zephyr>())
                                {
                                    tower.SellTower();
                                    InGame.instance.SetCash(0);
                                }
                            }
                        }

                        Bosses.canMerge = false;
                        ForceStopMerge = true;
                        Bosses.FinalBoss.Destroy();
                        Bosses.FinalBoss1.Destroy();
                        InGame.instance.SpawnBloons("NovaBoss", 1, 0);
                        this.Destroy();
                    }
                }

                registration.fakeHealth = Bosses.fakeHealth;
                registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                if (!Bosses.heal)
                {
                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth += (boss.bloonModel.maxHealth - boss.health);


                        if (Bosses.fakeHealth >= Bosses.fakeMaxHealth)
                        {
                            Bosses.fakeMaxHealth = Bosses.fakeHealth;
                            Bosses.ExtraFakeHealth += (boss.bloonModel.maxHealth - boss.health);
                        }
                    }
                }
                else
                {
                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                    }
                }

                boss.health = boss.bloonModel.maxHealth;

                Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);

                if (Bosses.fakeHealth <= 0)
                {
                    Bosses.FinalBoss.trackSpeedMultiplier = 25f;
                    Bosses.FinalBoss1.trackSpeedMultiplier = -25f;
                }
            }
            else
            {
                this.Destroy();
            }
        }
    }

    [RegisterTypeInIl2Cpp]
    public class FinalBoss1Behaviour : MonoBehaviour
    {
        public Bloon boss;
        public BossRegisteration registration;
        public bool EndTrack = false;

        public FinalBoss1Behaviour() : base()
        {

        }

        public void Start()
        {
            Bosses.fakeHealth = 10000000;
            Bosses.FinalBoss1 = boss;
            Bosses.fakeMaxHealth = 10000000;
        }

        public void Update()
        {
            if (boss != null)
            {
                if (Bosses.HealthCharger != null)
                {
                    if (Bosses.random.Next(10) == 0)
                    {
                        Bosses.fakeHealth += 1000;
                        if (Bosses.fakeHealth >= Bosses.fakeMaxHealth)
                        {
                            Bosses.fakeMaxHealth = Bosses.fakeHealth;
                        }
                    }
                }

                Bosses.FianlBoss1Pos = new UnityEngine.Vector2(boss.Position.X, boss.Position.Z);

                if (boss.PercThroughMap() > 0.99f)
                {
                    Bosses.canMerge = true;
                    boss.trackSpeedMultiplier = -1;
                    EndTrack = true;
                }
                else if (EndTrack == false)
                {
                    boss.trackSpeedMultiplier = 1500;
                }

                registration.fakeHealth = Bosses.fakeHealth;
                registration.fakeMaxHealth = Bosses.fakeMaxHealth;

                if (Bosses.heal)
                {
                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth += (boss.bloonModel.maxHealth - boss.health);


                        if (Bosses.fakeHealth >= Bosses.fakeMaxHealth)
                        {
                            Bosses.fakeMaxHealth = Bosses.fakeHealth;
                            Bosses.ExtraFakeHealth += (boss.bloonModel.maxHealth - boss.health);
                        }
                    }
                }
                else if (!Bosses.heal)
                {
                    if (boss.health < boss.bloonModel.maxHealth)
                    {
                        Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                    }
                }

                boss.health = boss.bloonModel.maxHealth;

                Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);
                bossPanel.healthBar = "healthBar";

                if (Bosses.fakeHealth <= 0)
                {
                    Bosses.FinalBoss.trackSpeedMultiplier = 25f;
                    Bosses.FinalBoss1.trackSpeedMultiplier = -25f;
                    this.Destroy();
                }
            }
            else
            {
                this.Destroy();
            }
        }
    }

    [RegisterTypeInIl2Cpp]
    
    public class SuperNovaBehaviours : MonoBehaviour
    {
        public Bloon boss;
        public BossRegisteration registration;
        public bool state1 = false;
        public bool state2 = false;
        public bool state3 = false;
        public bool state4 = false;

        public SuperNovaBehaviours() : base()
        {
        }

        public void Start()
        {
            Bosses.fakeHealth = 5000000;
            Bosses.fakeMaxHealth = 5000000;
            InGame.instance.SetHealth(1000);
            BrotherMonkey.BrotherMonkey.boss = boss;
            Heart.HeartUI.CreatePanel();
            Story.StoryMessage[] messages = [
                new("Alright brothers, this is it. This is the final clash. We have to defeat it!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("Ah?!?!?! Why does the Lives icon look weird?!", "Rodrick", Story.StoryPortrait.Bro1),
                new("What kind of dark witchcraft is this!?", "Zephyr", Story.StoryPortrait.Bro2),
                new("The reason the lives are black is because it’s slowly being withered away. To my calculations, precisely 1 life every second.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("And with exactly 3 minutes and 20 seconds to spare at this exact moment before the existence of the universe collapses on itself...", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("Everything you know and love, and don’t know and don’t love, heck, if the multiverse theory is true, every universe will cease to exist!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("YIKES!!! EEEEEK!!!", "Rodrick", Story.StoryPortrait.Bro1),
                new("Dang, you scared Rodrick so much that you made him forget he’s from Minnesota lol", "Zephyr", Story.StoryPortrait.Bro2),
                new("You two NEED to, and I cannot stress the word 'NEED' enough, destroy this Bloon!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("There are no other Monkeys and no money to fund more soldiers.", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey),
                new("You will decide the fate of the space-time continuum. Now GO! THERE'S NO TIME TO WASTE!! RAHHH!!!", "Doctor Monkey", Story.StoryPortrait.DoctorMonkey)
            ];

            Story.StoryUI.CreatePanel(messages);
        }

        public void Update()
        {
            if (boss != null)
            {
                if (boss.PercThroughMap() > 0.99f)
                {
                    boss.trackSpeedMultiplier = -1;
                }
                else if (boss.PercThroughMap() <= 0.1f)
                {
                    boss.trackSpeedMultiplier = 1;
                }

                boss.Rotation = boss.PercThroughMap() * 1000;
                boss.prevRot = boss.Rotation;
                
                registration.fakeHealth = Bosses.fakeHealth;
                registration.fakeMaxHealth = Bosses.fakeMaxHealth;
                
                if (boss.health < boss.bloonModel.maxHealth && Bosses.DamageActive)
                {
                    Bosses.fakeHealth -= (boss.bloonModel.maxHealth - boss.health);
                }

                boss.health = boss.bloonModel.maxHealth;

                Bosses.fakeHealth = Math.Max(0, Bosses.fakeHealth);
                bossPanel.healthBar = "healthBar";

                if (Bosses.fakeHealth <= Bosses.fakeMaxHealth * 0.8f && state1 == false)
                {
                    boss.TowerSetImmunity = TowerSet.AllMonkeyTowerSets;
                    state1 = true;
                    MelonLogger.Msg("Boss health 80%");
                    Bosses.bossState = 1;
                    InGame.instance.SpawnBloons("DdtFortified", 5, 25);
                }

                
                if (Bosses.fakeHealth <= Bosses.fakeMaxHealth * 0.6f && state2 == false)
                {
                    state2 = true;
                    MelonLogger.Msg("Boss health 60%");
                    Bosses.bossState = 2;
                    InGame.instance.SpawnBloons("BloonariusElite1", 1, 0);
                }
                
                if (Bosses.fakeHealth <= Bosses.fakeMaxHealth * 0.4f && state3 == false)
                {
                    state3 = true;
                    MelonLogger.Msg("Boss health 40%");
                    Bosses.HalvedWS = true;
                    Bosses.bossState = 3;
                }
                
                if (Bosses.fakeHealth <= Bosses.fakeMaxHealth * 0.2f && state4 == false)
                {
                    state4 = true;
                    MelonLogger.Msg("Boss health 20%");
                    Bosses.bossState = 4;
                    InGame.instance.SpawnBloons("BloonariusElite1", 1, 0);
                    InGame.instance.SpawnBloons("VortexElite1", 1, 0);
                    InGame.instance.SpawnBloons("BlastapopoulosElite1", 1, 0);
                    InGame.instance.SpawnBloons("LychElite1", 1, 0);
                    InGame.instance.SpawnBloons("PhayzeElite1", 1, 0);
                }
            }
            else
            {
                this.Destroy();
            }
        }
    }


    public class Totem : ModBloon
    {
        public override string BaseBloon => BloonType.sRed;

        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.maxHealth = 25;
            bloonModel.speed /= 2;
        }
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.OnSpawn))]
    public class BloonSpawn
    {
        private static readonly System.Random random = new System.Random();

        [HarmonyPostfix]
        public static void Postfix(Bloon __instance)
        {
            {
                MelonLogger.Msg(__instance.bloonModel.baseId);
                
                
                if (__instance.bloonModel.id.Contains("Totem"))
                {
                    if (random.Next(2) == 0)
                    {

                        MelonLogger.Msg("Totem Magic spawned");
                        __instance.TowerSetImmunity = TowerSet.Magic;
                        if (BrotherMonkey.BrotherMonkey.boss != null)
                        {
                            bossPanel.healthBar = DREADBAR;
                            Bosses.totem = __instance;
                        }
                    }
                    else
                    {

                        MelonLogger.Msg("Totem Military spawned");
                        __instance.TowerSetImmunity = TowerSet.Military;
                        if (BrotherMonkey.BrotherMonkey.boss != null)
                        {
                            bossPanel.healthBar = DREADBAR;
                            Bosses.totem = __instance;
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
    public class BloonPopped
    {
        [HarmonyPostfix]
        public static void Postfix(Bloon __instance, Tower tower)
        {
            {
                if (__instance.bloonModel.id.Contains("True"))
                {
                    BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1")
                        .GetBehavior<BuffBloonSpeedModel>();
                    buff.speedBoost = 1.2f;
                    var mutator = buff.Mutator;
                    Bosses.FinalBoss.AddMutator(mutator, 1);
                    Bosses.FinalBoss1.AddMutator(mutator, 1);
                }
                else if (__instance.bloonModel.id.Contains("False"))
                {
                    BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1")
                        .GetBehavior<BuffBloonSpeedModel>();
                    buff.speedBoost = 1.2f;
                    var mutator = buff.Mutator;
                    Bosses.FinalBoss.AddMutator(mutator, 1);
                    Bosses.FinalBoss1.AddMutator(mutator, 1);
                }
                
                if (__instance.bloonModel.id.Contains("Queen"))
                {
                    
                    //MelonLogger.Msg("Damaged Queen");
                    Dictionary<string, List<string>> towers = new Dictionary<string, List<string>>()
                    {
                        { "DartMonkey", new List<string> { "050", "150", "250", "051", "052", "555" } },
                        { "DartlingGunner", new List<string> { "300", "310", "320", "301", "302", "400", "410", "420", "401", "402", "500", "510", "520", "501", "502" } },
                        { "WizardMonkey", new List<string> { "000", "100", "200", "300", "400", "500", "110", "120", "101", "102", "210", "220", "201", "202", "310", "320", "301", "302", "410", "420", "401", "402", "510", "520", "501", "502", "010", "020", "001", "011", "021", "002", "012", "022", "003", "013", "023", "004", "014", "024", "005", "015", "025", "555" } },
                        { "SuperMonkey", new List<string> { "100", "110", "120", "101", "102", "200", "210", "220", "201", "202", "300", "310", "320", "301", "302", "400", "410", "420", "401", "402", "500", "510", "520", "501", "502", "103", "203", "104", "204", "105", "205", "040", "140", "240", "041", "042", "050", "150", "250", "051", "052" } },
                        { "DruidMonkey", new List<string> { "200", "210", "220", "201", "202", "230", "240", "250", "203", "204", "205", "300", "310", "320", "301", "302", "400", "410", "420", "401", "402", "500", "510", "520", "501", "502" } },
                        { "EngineerMonkey", new List<string> { "400", "410", "420", "401", "402", "500", "510", "520", "501", "502", "555" } }
                    };
                    
                    foreach (var towerPath in towers)
                    {
                        for (int i = 0; i < towerPath.Value.Count; i++)
                        {
                            if (towerPath.Value[i] == "555")
                            {
                                towerPath.Value[i] = "Paragon";
                            }
                        }
                    }
                    
                    foreach (var towerPath in towers)
                    {
                        foreach (var path in towerPath.Value)
                        {
                            string fullTowerPath = $"{towerPath.Key}-{path}";
                            if (tower.towerModel.name == fullTowerPath)
                            {
                                __instance.Damage(tower.towerModel.GetWeapon().projectile.GetDamageModel().damage * 5, null, false, false, false, tower);
                            }
                        }
                    }
                }

                if (__instance.health <= 1)
                {
                    if (__instance.bloonModel.id.Contains("Bloonarius"))
                    {
                        Bosses.CustomBoss = null;
                    }
                
                    if (__instance.bloonModel.id.Contains("Blastapopoulos"))
                    {
                        Bosses.CustomBoss = null;
                    }
                
                    if (__instance.bloonModel.id.Contains("Vortex"))
                    {
                        Bosses.CustomBoss = null;
                    }
                }
                
                if (__instance.bloonModel.baseId == ModContent.BloonID<Totem>())
                {
                    if (__instance.health <= 1)
                    {
                        if (BrotherMonkey.BrotherMonkey.boss != null)
                        {
                            bossPanel.healthBar = "healthBar";
                            Bosses.totem = null;
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(TimeTrigger), nameof(TimeTrigger.Trigger))]
    public class TimeTriggerPacth
    {
        [HarmonyPostfix]
        public static void Postfix(TimeTrigger __instance)
        {
            {
                if (__instance.timeTriggerModel.actionIds.Contains("Drain"))
                {
                    InGame.instance.AddHealth(-1);
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("ChargingCharger"))
                {
                    if (Bosses.HealthCharger == null)
                    {
                        Bosses.SpawnHealthCharged();
                    }
                }
                if (__instance.timeTriggerModel.actionIds.Contains("SpawnTotem"))
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<Totem>(), 1, 0);
                }

                if (__instance.timeTriggerModel.actionIds.Contains("ImmuneState"))
                {
                    if (Random.Shared.Next(5) == 0)
                    {
                        BrotherMonkey.BrotherMonkey.boss.TowerSetImmunity = TowerSet.Magic;
                    }
                    else if (Random.Shared.Next(3) == 0)
                    {
                        BrotherMonkey.BrotherMonkey.boss.TowerSetImmunity = TowerSet.Military;
                    }
                    else
                    {
                        BrotherMonkey.BrotherMonkey.boss.TowerSetImmunity = TowerSet.None;
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("SpawnCharger"))
                {
                    if (Bosses.HealthCharger == null)
                    {
                        Bosses.SpawnHealthCharged();
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("HealSpeed"))
                {
                    if (BrotherMonkey.BrotherMonkey.boss != null)
                    {
                        if (Bosses.fakeHealth >= Bosses.fakeMaxHealth)
                        {
                            Bosses.fakeMaxHealth += 5000;
                            Bosses.fakeHealth += 5000;
                        }
                        else
                        {
                            Bosses.fakeHealth += 5000;
                        }
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("SpawnBosses"))
                {
                    InGame.instance.SpawnBloons("Bloonarius2", 1, 0);
                    InGame.instance.SpawnBloons("Blastapopoulos1", 1, 50);
                    InGame.instance.SpawnBloons("Vortex1", 1, 100);
                    Bosses.CustomBoss = "Alive"; 
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("SellsTower"))
                {
                    List<Il2CppAssets.Scripts.Simulation.Towers.Tower> towers = InGame.instance.GetTowers();
                    
                    System.Random rand = new System.Random();

                    for (int i = 0; i < 1; i++)
                    {
                        int index = rand.Next(towers.Count);
                        Il2CppAssets.Scripts.Simulation.Towers.Tower towerToSell = towers[index];
                        towerToSell.SellTower();

                        towers.RemoveAt(index);
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("SpawnsDDT"))
                {
                    InGame.instance.SpawnBloons("Moab", Bosses.ExtraFakeHealth / 2, 50);
                }

                if (__instance.timeTriggerModel.actionIds.Contains("Swap"))
                {
                    if (Bosses.heal)
                    {
                        Bosses.heal = false;
                    }
                    else
                    {
                        Bosses.heal = true;
                    }
                }
                
                

                if (__instance.timeTriggerModel.actionIds.Contains("UpgradesMoabs"))
                {
                    foreach (var bloon in InGame.instance.GetBloons())
                    {
                        if (bloon.bloonModel.baseId == "Moab")
                        {
                            bloon.Destroy();
                            InGame.instance.SpawnBloons("Bfb", 1, 50);
                        }
                        else if (bloon.bloonModel.baseId == "Bfb")
                        {
                            bloon.Destroy();
                            InGame.instance.SpawnBloons("Zomg", 1, 100);
                        }
                        else if (bloon.bloonModel.baseId == "Zomg")
                        {
                            bloon.Destroy();
                            InGame.instance.SpawnBloons("Bad", 1, 100);
                        }
                    }
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("FinalMoveTowers"))
                {
                    List<Il2CppAssets.Scripts.Simulation.Towers.Tower> towers = InGame.instance.GetTowers();
                    
                    System.Random rand = new System.Random();

                    for (int i = 0; i < towers.Count; i++)
                    {
                        Il2CppAssets.Scripts.Simulation.Towers.Tower towerToMove = towers[i];
                        CalculateNewSpot(towerToMove);
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("MoveTower"))
                {
                    List<Il2CppAssets.Scripts.Simulation.Towers.Tower> towers = InGame.instance.GetTowers();
                    
                    System.Random rand = new System.Random();

                    for (int i = 0; i < 1; i++)
                    {
                        int index = rand.Next(towers.Count);
                        Il2CppAssets.Scripts.Simulation.Towers.Tower towerToMove = towers[index];
                        CalculateNewSpot(towerToMove);
                    }
                }

                if (__instance.timeTriggerModel.actionIds.Contains("NovaDDT"))
                {
                    if (Bosses.bossState != 0)
                    {
                        InGame.instance.SpawnBloons("DdtFortified", 5, 25);
                    }
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("Unstun"))
                {
                    if (Bosses.bossState != 0)
                    {
                        BrotherMonkey.BrotherMonkey.rodrick.IsStunned = false;
                        BrotherMonkey.BrotherMonkey.zephyr.IsStunned = false;
                    }
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("DamageActive"))
                {
                    if (Bosses.bossState >= 2)
                    {
                        BrotherMonkey.BrotherMonkey.boss.TowerSetImmunity = TowerSet.None;
                    }
                }
                
                if (__instance.timeTriggerModel.actionIds.Contains("SpawnsZomgs"))
                {
                    if (Bosses.bossState >= 3)
                    {
                        InGame.instance.SpawnBloons("Zomg", 5, 50);
                    }
                }
            }
        }
        
        public static int RandomInt(int min, int max)
        {
            System.Random rand = new();
            return rand.Next(min, max);
        }

        public static void CalculateNewSpot(Tower tower)
        {
            Vector2 oldPos = tower.Position.ToVector2();

            Vector2 newPos = new(RandomInt(-100, 100), RandomInt(-100, 100));

            tower.PositionTower(newPos);
            tower.Position.Z = 100;
        }
    }
}
