using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DestructorBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armor Of Omnipotence");
            Tooltip.SetDefault("160% increased damage and 100% increased critical strike chance, Increases max life by 20000 and mana by 300.\nIncreases damage reduction by 420%, Godlike life regen, also increases armor penetration to 1300.");
            //add edgy and colored text yes pls
        }

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 32;
            item.value = Item.sellPrice(gold: 69);
            item.defense = 50000;
            item.rare = 11;
            item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            const float damageUp = 1.6f;
            const int critUp = 100;
            player.meleeDamage += damageUp;
            player.rangedDamage += damageUp;
            player.magicDamage += damageUp;
            player.minionDamage += damageUp;
            player.meleeCrit += critUp;
            player.rangedCrit += critUp;
            player.magicCrit += critUp;

            player.statLifeMax2 += 20000;
            player.statManaMax2 += 300;

            player.endurance += 4.2f;

            player.lifeRegen += 999;
            player.lifeRegenCount += 999;
            player.lifeRegenTime += 999;

            player.armorPenetration += 1300;
        }


    }
}
