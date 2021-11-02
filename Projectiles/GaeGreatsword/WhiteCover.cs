using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public class WhiteCover : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        private bool init = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinity Slash");
        }

        public override void SetDefaults()
        {
            projectile.width = 2400;
            projectile.height = 1600;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.timeLeft = 180;
            projectile.GetGlobalProjectile<VadGlobalProjectile>().TimeFreezeImmune = true;
        }

        public float LifeTimer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (init)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Flashbang"));
                init = false;
            }

            LifeTimer++;
            if (LifeTimer < 10)
            {
                projectile.alpha = (int)MathHelper.Max(projectile.alpha - 80, 0);
            } else if(LifeTimer > 30)
            {
                projectile.alpha = (int)MathHelper.Min(projectile.alpha + 2, 255);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle destRect = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            Color color = GetAlpha(lightColor) ?? lightColor;

            spriteBatch.Draw(Main.magicPixel, destRect, color * projectile.Opacity);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool CanDamage() => false;
    }
}
