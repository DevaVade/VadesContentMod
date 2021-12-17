using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Placeable
{
    public class DevTeam : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dev Team");
            Tooltip.SetDefault("Deva Vade#4493");
        }

        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.Purple;
            item.value = Item.buyPrice(0, 20, 0, 0);
            item.createTile = ModContent.TileType<Tiles.DevTeam>();
        }
    }
}
