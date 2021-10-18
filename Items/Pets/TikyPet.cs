using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Pets
{
	public class TikyPet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Tricky Plushie");
			base.Tooltip.SetDefault("Summons a Smol tiky to provide light\nDont put it in a jar.");
		}

		public override void SetDefaults()
		{
			base.item.damage = 0;
			base.item.useStyle = 1;
			base.item.shoot = base.mod.ProjectileType("TikyPetProj");
			base.item.width = 32;
			base.item.height = 32;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
			base.item.rare = 16;
			base.item.noMelee = true;
			base.item.value = Item.sellPrice(69, 0, 0, 0);
			base.item.buffType = base.mod.BuffType("TikyBuff");
			base.item.UseSound = SoundID.Item2;
		}


		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(base.item.buffType, 15, true);
			}
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine tooltipLine in list)
			{
				if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
				{
					tooltipLine.overrideColor = new Color?(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
				}
			}
		}
	}
}
