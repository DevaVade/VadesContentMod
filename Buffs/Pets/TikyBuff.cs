using System;
using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs.Pets
{
	public class TikyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			base.DisplayName.SetDefault("Tiky");
			base.Description.SetDefault("Just do what comes natural.");
			Main.buffNoTimeDisplay[base.Type] = true;
			Main.lightPet[base.Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<VadPlayer>().tiky = true;
			if (player.ownedProjectileCounts[base.mod.ProjectileType("TikyPetProj")] <= 0 && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, base.mod.ProjectileType("TikyPetProj"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}


