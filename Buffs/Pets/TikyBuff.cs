using VadesContentMod.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs.Pets
{
    public class TikyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tiky");
			Description.SetDefault("Just do what comes natural.");

			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<VadPlayer>().tiky = true;

			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<TikyPetProj>()] <= 0)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<TikyPetProj>(), 0, 0f, player.whoAmI);
			}
		}
	}
}


