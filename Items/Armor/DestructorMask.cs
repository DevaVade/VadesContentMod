using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DestructorMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helm Of Omniscience");
            Tooltip.SetDefault("90% increased damage and 50% increased critical strike chance, Increases max number of minions and sentries by 400.\n90% reduced mana usage, 25% chance not to consume ammo, also increases armor penetration to 1300.");
            //add edgy and colored text yes pls
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 26;
            item.value = Item.sellPrice(gold: 69);
            item.defense = 50000;
            item.rare = ItemRarityID.Purple;
            item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            const float damageUp = 0.9f;
            const int critUp = 50;
            player.meleeDamage += damageUp;
            player.rangedDamage += damageUp;
            player.magicDamage += damageUp;
            player.minionDamage += damageUp;
            player.meleeCrit += critUp;
            player.rangedCrit += critUp;
            player.magicCrit += critUp;

            player.maxMinions += 400;
            player.maxTurrets += 400;

            player.manaCost -= 0.90f;
            player.ammoCost75 = true;

            player.armorPenetration += 1300;

            player.wingTimeMax = 999999;
            player.wingTime = player.wingTimeMax;
            player.ignoreWater = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) =>
            body.type == ModContent.ItemType<DestructorBody>() && legs.type == ModContent.ItemType<DestructorPants>();

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<VadPlayer>().destructorSet = true;

            List<string> hotkeys = VadesContentMod.OneShothotKey.GetAssignedKeys();
            string hotkey = hotkeys.Count == 0 ? "[NONE]" : hotkeys[0];

            player.setBonus = "Press " + hotkey + " to instakill anything. This effect has a 2-minute cooldown";

            player.allDamage += 22.2f;
            const int critUp = 100;
            player.meleeCrit += critUp;
            player.rangedCrit += critUp;
            player.magicCrit += critUp;
            player.allDamage += 12f;
            player.thorns = 100f;

            player.armorPenetration += 15900;

            player.longInvince = true;
            player.endurance += 500f;
            player.lavaImmune = true;
            player.manaFlower = true;
            player.manaMagnet = true;
            player.magicCuffs = true;
            player.ignoreWater = true;
            player.pStone = true;
            player.findTreasure = true;
            player.noKnockback = true;

            player.lavaImmune = true;
            player.noFallDmg = true;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
            player.armorEffectDrawOutlines = true;
        }
    }
}
