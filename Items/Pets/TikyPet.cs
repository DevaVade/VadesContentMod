using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Pets
{
	public class TikyPet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tricky Plushie");
			Tooltip.SetDefault("Summons a Smol tiky to provide light\nDont put it in a jar.");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = ModContent.ProjectileType<Projectiles.Pets.TikyPetProj>();
			item.width = 32;
			item.height = 32;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 16;
			item.noMelee = true;
			item.value = Item.sellPrice(platinum: 69);
			item.buffType = ModContent.BuffType<Buffs.Pets.TikyBuff>();
			item.UseSound = SoundID.Item2;
		}


		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 15, true);
			}
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			foreach (TooltipLine tooltipLine in list)
			{
				if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
				{
					tooltipLine.overrideColor = Main.DiscoColor;
				}
			}
		}
	}
}
