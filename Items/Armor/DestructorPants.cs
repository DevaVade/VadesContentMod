using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DestructorPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leggings Of Dimensions");
            Tooltip.SetDefault("90% increased damage and 70% increased critical strike chance, 90% increased movement and 90% increased melee speed.\nalso increases armor penetration to 1300.");
            //add edgy and colored text yes pls
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = Item.sellPrice(gold: 69);
            item.defense = 50000;
            item.rare = ItemRarityID.Purple;
            item.expert = true;
        }

        public override void UpdateEquip(Player player)
        {
            const float damageUp = 0.9f;
            const int critUp = 70;
            player.meleeDamage += damageUp;
            player.rangedDamage += damageUp;
            player.magicDamage += damageUp;
            player.minionDamage += damageUp;
            player.meleeCrit += critUp;
            player.rangedCrit += critUp;
            player.magicCrit += critUp;

            player.moveSpeed += 0.9f;
            player.meleeSpeed += 0.9f;

            player.armorPenetration += 1300;
        }
    }
}
