using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.Pets
{
	public class TikyPetProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Smol tiky");
			Main.projFrames[base.projectile.type] = 5;
			Main.projPet[base.projectile.type] = true;
			ProjectileID.Sets.LightPet[base.projectile.type] = true;
		}

		public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 50;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = 26;
            aiType = ProjectileID.BabyHornet;
            projectile.netImportant = true;
            projectile.friendly = true;
        }

        public override bool PreAI()
        {
            Main.player[projectile.owner].hornet = false;
            return true;
        }

		public override void AI()
		{
			Lighting.AddLight(base.projectile.Center, (float)Main.DiscoR / 110f, (float)Main.DiscoG / 110f, (float)Main.DiscoB / 110f);
			Player player = Main.player[base.projectile.owner];
			VadPlayer modPlayer = player.GetModPlayer<VadPlayer>();
			if (player.dead)
			{
				modPlayer.tiky = false;
			}
			if (modPlayer.tiky)
			{
				base.projectile.timeLeft = 2;
			}
			if (Vector2.Distance(base.projectile.Center, player.Center) > 2500f)
			{
				base.projectile.Center = player.Center;
			}

			if (projectile.Distance(player.Center) > 3000)
                projectile.Center = player.Center;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y3 = num156 * projectile.frame; 
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects spriteEffects = projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, spriteEffects, 0f);
            return false;
        }
	}
}
