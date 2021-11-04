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
			DisplayName.SetDefault("Smol tiky");

			Main.projFrames[projectile.type] = 5;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
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
			Lighting.AddLight(projectile.Center, Main.DiscoColor.ToVector3() / 110f);

			Player player = Main.player[projectile.owner];
			VadPlayer modPlayer = player.GetModPlayer<VadPlayer>();

			if (player.dead)
				modPlayer.tiky = false;

			if (modPlayer.tiky)
				projectile.timeLeft = 2;

			if (Vector2.Distance(projectile.Center, player.Center) > 2500f)
			{
				projectile.Center = player.Center;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];

            Rectangle sourceRectangle = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            SpriteEffects spriteEffects = projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(
				texture, 
				projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), 
				sourceRectangle, 
				projectile.GetAlpha(lightColor), 
				projectile.rotation, 
				origin, 
				projectile.scale, 
				spriteEffects, 
				0f);

            return false;
        }
	}
}
