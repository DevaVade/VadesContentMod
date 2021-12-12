using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace VadesContentMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class EntroWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropion Wings");
        }

        public override void SetDefaults()
        {
            item.width = 154;
            item.height = 74;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(platinum: 1, gold: 20);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 220;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.95f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 4.5f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 10f;
            acceleration *= 2.8f;
        }
    }
}
