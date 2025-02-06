using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles.Behaviors;
using UnityEngine;

namespace BrotherMonkey.Rodrick.TopPath;

public class UPGRADES
{
    public class HeavyDarts : ModUpgrade<rodrick>
    {
        public override string Icon => "RodrickI1";
        
        public override string Portrait => "RodrickP1";

        public override int Path => Middle;

        public override int Tier => 1;

        public override int Cost => 700;

        public override string Description => "Darts gain more popping power.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += 3;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }
    }

    public class Crusher : ModUpgrade<rodrick>
    {
        public override string Icon => "RodrickI2";
        
        public override string Portrait => "RodrickP2";
        
        public override int Path => Middle;

        public override int Tier => 2;

        public override int Cost => 900;

        public override string Description =>
            "Darts gain increased ceramic, fortified, MOAB and Boss damage, and increased damage to all.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(
                new DamageModifierForTagModel("DamageModifierForTagModel_Moab", "Moab", 2, 0, false, true));
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(
                new DamageModifierForTagModel("DamageModifierForTagModel_Fortified", "Fortified", 2, 0, false, true));
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(
                new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 2, 0, false, true));
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(
                new DamageModifierForTagModel("DamageModifierForTagModel_Boss", "Boss", 2, 0, false, true));
        }
    }

    public class Gunpower : ModUpgrade<rodrick>
    {
        public override string Icon => "RodrickI3";
        
        public override string Portrait => "RodrickP3";
        
        public override int Path => Middle;

        public override int Tier => 3;

        public override int Cost => 2800;

        public override string DisplayName => "Gunpowder-packed Dartling Gun";

        public override string Description =>
            "Rodrick’s dartling gun is shoved with an overload of gunpowder. While it decreases accuracy and makes an explosion on Rodrick’s gun, it increases damage and gives Lead popping power.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetDescendants<RandomEmissionModel>().ForEach(e => e.angle = 10);

            foreach (var weapon in towerModel.GetWeapons())
            {
                weapon.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                weapon.projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-030").GetWeapon()
                    .projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().Duplicate());
            }
        }
    }

    public class BURN : ModUpgrade<rodrick>
    {
        public override string Icon => "RodrickI4";
        
        public override string Portrait => "RodrickP4";
        
        public override int Path => Middle;

        public override int Tier => 4;

        public override int Cost => 6000;

        public override string DisplayName => "Flamethrower-propelled Rockets";

        public override string Description =>
            "Rodrick upgrades his machine gun with flamethrower attachments, not only burning everything on fire, but also propelling rocket grenades that have massive popping power";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<RodrickT4>();
            towerModel.GetWeapon().projectile.GetDamageModel().damage += 2;
            var Dot = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>()
                .Duplicate();

            Dot.GetBehavior<DamageOverTimeModel>().interval = 0.7f;
            Dot.overlayType = "Fire";
            Dot.lifespan = 999f;
            Dot.lifespanFrames = 999;

            towerModel.GetWeapon().projectile.AddBehavior(Dot);
            towerModel.GetWeapon().projectile.UpdateCollisionPassList();
            towerModel.GetWeapon().projectile.GetDescendant<CreateProjectileOnExhaustPierceModel>().projectile
                .AddBehavior(Dot);
            towerModel.GetWeapon().projectile.GetDescendant<CreateProjectileOnExhaustPierceModel>().projectile
                .UpdateCollisionPassList();
            towerModel.GetWeapon().projectile.GetDescendant<AddBehaviorToBloonModel>()
                .GetDescendant<DamageOverTimeModel>().damage = 4;
            towerModel.GetWeapon().projectile.GetDescendant<CreateProjectileOnExhaustPierceModel>().projectile
                .GetDescendant<AddBehaviorToBloonModel>().GetDescendant<DamageOverTimeModel>().damage = 4;
        }
    }

    public class NUKES : ModUpgrade<rodrick>
    {
        public override string Icon => "RodrickP5";
        
        public override string Portrait => "RodrickP5";
        
        public override int Path => Middle;

        public override int Tier => 5;

        public override int Cost => 80000;

        public override string DisplayName => "Napalm-infused Nukes";

        public override string Description =>
            "The powerful incendiary mixture infused into nuclear missiles. What could go wrong?";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<RodrickT5>();
            towerModel.GetWeapon().projectile.GetDamageModel().damage += 500;
            towerModel.GetWeapon().projectile.pierce = 1f;

            towerModel.GetWeapon().projectile.GetDescendant<AddBehaviorToBloonModel>()
                .GetDescendant<DamageOverTimeModel>().damage = 50;
            towerModel.GetWeapon().projectile.RemoveBehavior<CreateProjectileOnExhaustPierceModel>();

            var boom = Game.instance.model.GetTower("BombShooter", 3).GetDescendant<CreateProjectileOnContactModel>()
                .projectile.Duplicate();
            var boomFx = Game.instance.model.GetTower("BombShooter", 3).GetDescendant<CreateEffectOnContactModel>()
                .effectModel.Duplicate();


            towerModel.GetWeapon().projectile.AddBehavior(new CreateEffectOnExhaustFractionModel(
                "CreateEffectOnExhaustFractionModel", boomFx, boomFx.lifespan,
                Il2CppAssets.Scripts.Models.Effects.Fullscreen.No, 1, 1, true));

            towerModel.GetWeapon().projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel(
                "CreateProjectileOnExhaustFractionModel", boom, new SingleEmissionModel("SingleEmissionModel", null), 1,
                1, false, true, false));


            towerModel.GetWeapon().projectile.GetDescendant<CreateProjectileOnExhaustFractionModel>().projectile
                .GetDamageModel().damage += 500;
            towerModel.GetWeapon().projectile.GetDescendant<CreateProjectileOnExhaustFractionModel>().projectile
                .AddBehavior(towerModel.GetWeapon().projectile.GetDescendant<DamageOverTimeModel>());
            towerModel.GetWeapon().projectile.scale *= 5f;
            towerModel.GetWeapon().projectile.radius *= 5f;
            towerModel.GetWeapon().projectile.GetDescendant<CreateEffectOnExhaustFractionModel>().effectModel.scale *=
                5f;

            towerModel.GetWeapon().projectile.id = "Stun";
            towerModel.GetWeapon().GetDescendant<CreateProjectileOnExhaustFractionModel>().projectile.id = "Stun";

            towerModel.GetWeapon().projectile.ApplyDisplay<MadProjectile>();
        }
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.ApplyDamageToBloon))]
    public class YouStuckLol
    {
        [HarmonyPostfix]
        public static void Postfix(Bloon __instance, Projectile projectile)
        {
            if (projectile != null && !__instance.bloonModel.baseId.Contains("Bad"))
            {
                if (projectile.projectileModel.id.Contains("Stun"))
                {
                    BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1")
                        .GetBehavior<BuffBloonSpeedModel>();
                    buff.speedBoost = 0.8f;
                    var mutator = buff.Mutator;
                    __instance.AddMutator(mutator, 999);
                }
            }
        }
    }
}

public class MadProjectile : ModDisplay
{
    public override string BaseDisplay => "17d97a491cfa0154095f42ec1c5dae2d";
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        SetMeshTexture(node, "500NukeProjectile");
    }
}

public class RodrickT4 : ModCustomDisplay
{
    public override string AssetBundleName => "bosses";
    public override string PrefabName => "rodrickt4";
    
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var renderer in node.GetMeshRenderers())
        {
            renderer.ApplyOutlineShader();
            renderer.SetOutlineColor(Color.red);
        }
    }
}

public class RodrickT5 : ModCustomDisplay
{
    public override string AssetBundleName => "bosses";
    public override string PrefabName => "RodrickT5";
    
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var renderer in node.GetMeshRenderers())
        {
            renderer.ApplyOutlineShader();
            renderer.SetOutlineColor(Color.red);
        }
    }
}
