using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class EntroBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropion Breastplate");
            Tooltip.SetDefault(
                "10% increased melee critical strike chance\n" +
                "20% increased melee speed\n" +
                "10% increased damage reduction");
        }

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 32;
            item.value = Item.sellPrice(platinum: 1, gold: 20);
            item.rare = ItemRarityID.Purple;
            item.defense = 130;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.1f;
            player.meleeCrit += 10;
            player.meleeSpeed += 0.2f;
        }
    }
}
