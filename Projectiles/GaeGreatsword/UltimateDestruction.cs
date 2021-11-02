using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public class UltimateDestruction : ModProjectile
    {
        private readonly int FrameRate = 3;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = Main.projFrames[projectile.type] * FrameRate;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<VadGlobalProjectile>().TimeFreezeImmune = true;
        }

        public override void AI()
        {
            if (projectile.frame < Main.projFrames[projectile.type] - 1 && ++projectile.frameCounter >= FrameRate)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }

            VadUtils.ButcherNPCs(projectile.Hitbox);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int framesOnFirstColumn = 8;
            Texture2D texture = Main.projectileTexture[projectile.type];

            int frameWidth = texture.Width / 2;
            int frameHeight = texture.Height / framesOnFirstColumn;

            Rectangle sourceRectangle = new Rectangle(
                (projectile.frame / framesOnFirstColumn) * frameWidth,
                (projectile.frame % framesOnFirstColumn) * frameHeight,
                frameWidth,
                frameHeight);
            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(frameWidth, frameHeight) / 2;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                sourceRectangle,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
