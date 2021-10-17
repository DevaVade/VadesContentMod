using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class Wrings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Flight");
			Tooltip.SetDefault("Its transcendent owner has travelled countless universes and dimensions with this.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.accessory = true;
			item.value = 1000000;
			item.rare = ItemRarityID.Purple;
			ItemID.Sets.ItemNoGravity[item.type] = true;
			item.expert = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            VadPlayer VadPlayer = player.GetModPlayer<VadPlayer>();
            VadPlayer.WingStats2();
            player.runSlowdown = 2;
            player.moveSpeed += 0.5f;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            player.wingsLogic = 22;
            ascentWhenFalling = 2f;
            ascentWhenRising = 0.55f;
            maxCanAscendMultiplier = 3f;
            maxAscentMultiplier = 5f;
            constantAscend = 0.160f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 23f;
            acceleration *= 7f;
        }
    }
}
