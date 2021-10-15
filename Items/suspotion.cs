using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace VadesContentMod.Items
{
    public class suspotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sussy potion");
            Tooltip.SetDefault("Makes you unfunny sussy");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 30;
            item.rare = ItemRarityID.Purple;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;
            item.buffType = ModContent.BuffType<Buffs.Sugoma>();
            item.buffTime = 25200;
            item.UseSound = SoundID.Item3;
            item.value = Item.sellPrice(gold: 5);
        }
    }
}