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
            Item.width = 46;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.createTile = ModContent.TileType<Tiles.DevTeam>();
        }
    }
}
