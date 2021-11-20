using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Weapons
{
	public class EntropionBlade : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Entropion Blade");
			Tooltip.SetDefault("'The grandfather of all blades'");

			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 10));
		}

		public override void SetDefaults() 
		{
			item.damage = 5500;
			item.melee = true;
			item.width = 180;
			item.height = 278;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noUseGraphic = true;
			item.channel = true;
			item.knockBack = 6;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Purple;
			item.shoot = ModContent.ProjectileType<Projectiles.EntropionBladeSlash>();
			item.shootSpeed = 1;
			item.UseSound = SoundID.Item1; 
			item.autoReuse = true;
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine tooltipLine in list)
			{
				if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
				{
					tooltipLine.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					break;
				}
			}
		}
	}
}