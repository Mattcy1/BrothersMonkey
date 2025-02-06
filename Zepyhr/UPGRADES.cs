using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles.Behaviors;
using UnityEngine;

namespace BrotherMonkey.Zepyhr.TopPath;

public class UPGRADES
{
    public class PowerfulMagic : ModUpgrade<zephyr>
    {
        public override int Path => Middle;

        public override int Tier => 1;

        public override string Icon => "ZephyrI" + Tier;
        
        public override string Portrait => "ZephyrP" + Tier;

        public override int Cost => 700;

        public override string Description => "Zephyr’s magic gains lots of popping power, more pierce, damage, speed and range. Stuns Bloons for a short time upon impact.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().weapons[0].projectile.pierce += 3;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
            towerModel.GetAttackModel().range += 10;
            towerModel.range += 10;
            towerModel.GetAttackModel().weapons[0].rate -= 0.1f;
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("EngineerMonkey-002").GetAttackModel().weapons[0].projectile.GetDescendant<SlowOnPopModel>().Duplicate());
        }
    }
    
    public class SmartMagic : ModUpgrade<zephyr>
    {
        public override int Path => Middle;

        public override int Tier => 2;

        public override string Icon => "ZephyrI" + Tier;
        
        public override string Portrait => "ZephyrP" + Tier;
        
        public override int Cost => 900;

        public override string Description => "Add a new projectile that out their targets. Can see Camo bloons and is also able to pop purples.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().Duplicate());
            
            RetargetOnContactModel bounce = new RetargetOnContactModel("Retarget", 9999, 20, 9999, "Close", 0, true);
            towerModel.GetAttackModel().weapons[1].projectile.AddBehavior(bounce);

            towerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel().weapons[1].projectile.AddBehavior(Game.instance.model.GetTowerFromId("EngineerMonkey-002").GetAttackModel().weapons[0].projectile.GetDescendant<SlowOnPopModel>().Duplicate());
            
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
    }
    
    
    public class BloonHex  : ModUpgrade<zephyr>
    {
        public override int Path => Middle;

        public override int Tier => 3;
        
        public override string Icon => "ZephyrI" + Tier;
        
        public override string Portrait => "ZephyrP" + Tier;

        public override int Cost => 2700;

        public override string Description => "Magic casts a spell on the Bloons it touches that never goes away; withering away 1 layer per 0.5 seconds. This hex also slows the Bloons permanently by a little bit.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var Dot = Game.instance.model.GetTowerFromId("Alchemist").GetDescendant<AddBehaviorToBloonModel>()
                .Duplicate();

            Dot.GetBehavior<DamageOverTimeModel>().interval = 0.5f;
            Dot.mutationId = "BloonHexModded";
            Dot.overlayType = "EziliVoodoo";
            Dot.lifespan = 999f;
            Dot.lifespanFrames = 999;

            towerModel.GetWeapon().projectile.AddBehavior(Dot);
            towerModel.GetWeapon(1).projectile.AddBehavior(Dot);
            towerModel.GetWeapon().projectile.GetBehavior<AddBehaviorToBloonModel>().GetDescendant<DamageOverTimeModel>().damage = 5; 
            towerModel.GetWeapon(1).projectile.GetBehavior<AddBehaviorToBloonModel>().GetDescendant<DamageOverTimeModel>().damage = 5; 
            towerModel.ApplyDisplay<Display1>();
        }
    }
    
    public class EtherealStorm : ModUpgrade<zephyr>
    {
        public override int Path => Middle;

        public override int Tier => 4;
        
        public override string Icon => "ZephyrI" + Tier;
        
        public override string Portrait => "ZephyrP" + Tier;

        public override int Cost => 7500;

        public override string Description => "Now shoots 2 more magic bolts that curve around back to the tower. Activated ability: triples attack speed, triples damage, triples pierce and doubles range for 15 seconds.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MortarMonkey-040").GetAbility().Duplicate());
            towerModel.GetAbility().name = "Magic Go brrrr";
            towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage = 3;
            towerModel.GetAbility().GetBehavior<TurboModel>().multiplier = 0.125f;
            towerModel.GetAbility().GetBehavior<TurboModel>().Lifespan = 15;
            towerModel.GetAbility().Cooldown = 30;
            
            
            towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("BoomerangMonkey").GetWeapon().Duplicate());
            var rang = towerModel.GetAttackModel().weapons[2];
            towerModel.GetWeapon(2).emission = new ArcEmissionModel("ArcEmissionModel", 3, 0, 45, null, false, false);
            rang.projectile.GetDamageModel().damage += 10;
            rang.projectile.pierce += 10;
            rang.rate /= 2;
            towerModel.ApplyDisplay<Display2>();
        }
    }
    
    public class TimeStorm : ModUpgrade<zephyr>
    {
        public override int Path => Middle;

        public override int Tier => 5;
        
        public override string Icon => "ZephyrI" + Tier;
        
        public override string Portrait => "ZephyrP" + Tier;

        public override int Cost => 170000;

        public override string Description => "Electrical currents send Bloons back in time and deal massive damage.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage = 300;
            towerModel.GetAbility().GetBehavior<TurboModel>().multiplier = 0.150f;
            towerModel.GetAbility().GetBehavior<TurboModel>().Lifespan = 15;
            towerModel.GetAbility().Cooldown = 30;
            towerModel.GetAbility().AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-040").GetAbility().GetDescendant<ActivateAttackModel>().Duplicate());
            towerModel.GetAbility().GetDescendant<ActivateAttackModel>().attacks[0].RemoveWeapon(towerModel.GetAbility().GetDescendant<ActivateAttackModel>().attacks[0].weapons[0]);
            towerModel.GetAbility().GetDescendant<ActivateAttackModel>().attacks[0].AddWeapon(Game.instance.model.GetTowerFromId("Druid-400").GetAttackModel().weapons[1].Duplicate());
            
            towerModel.GetWeapon().projectile.GetBehavior<AddBehaviorToBloonModel>().GetDescendant<DamageOverTimeModel>().damage = 100; 
            towerModel.GetWeapon(1).projectile.GetBehavior<AddBehaviorToBloonModel>().GetDescendant<DamageOverTimeModel>().damage = 100;
            
            var rang = towerModel.GetAttackModel().weapons[2];
            towerModel.GetWeapon(2).emission = new ArcEmissionModel("ArcEmissionModel", 7, 0, 45, null, false, false);
            rang.projectile.GetDamageModel().damage += 25;
            towerModel.GetWeapon().projectile.GetDamageModel().damage += 25;
            towerModel.GetWeapon(1).projectile.GetDamageModel().damage += 25;

            towerModel.GetWeapon().projectile.RemoveBehavior<SlowOnPopModel>();
            towerModel.GetWeapon(1).projectile.RemoveBehavior<SlowOnPopModel>();
            towerModel.ApplyDisplay<Display3>();
        }
    }
}

public class Display1 : ModDisplay
{
    public override string BaseDisplay => Game.instance.model.GetTowerFromId("WizardMonkey-003").display.guidRef;
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        SetMeshTexture(node, "ZephyrD3");
        SetMeshTexture(node, "ZephyrD3", 1);
        SetMeshOutlineColor(node, Color.blue);
    }
}

public class Display2 : ModDisplay
{
    public override string BaseDisplay => Game.instance.model.GetTowerFromId("WizardMonkey-040").display.guidRef;
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        SetMeshTexture(node, "ZephyrD4");
        SetMeshTexture(node, "ZephyrD4", 1);
        SetMeshOutlineColor(node, Color.blue);
    }
}

public class Display3 : ModDisplay
{
    public override string BaseDisplay => Game.instance.model.GetTowerFromId("Druid-500").display.guidRef;
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        SetMeshTexture(node, "ZephyrD5");
        SetMeshTexture(node, "ZephyrD5", 1);
        SetMeshOutlineColor(node, Color.blue);
    }
}
