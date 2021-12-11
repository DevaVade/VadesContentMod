using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class EntroPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropion Leggings");
            Tooltip.SetDefault(
                "10% increased melee critical strike chance\n" +
                "20% increased movement speed\n" +
                "10% increased damage reduction");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(platinum: 1);
            item.defense = 110;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.1f;
            player.meleeCrit += 10;
            player.moveSpeed += 0.2f;
        }
    }
}
