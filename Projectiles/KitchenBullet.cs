using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles
{
    public class KitchenBullet : ModProjectile
    {
        private float spriteScale = 0f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0f)
            {
                projectile.ai[0] = 1f;

                projectile.velocity *= 0.5f;

                float maxSpeed = 10f;
                float speed = projectile.velocity.Length();
                if (speed > maxSpeed)
                    projectile.velocity *= maxSpeed / speed;
            }

            
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.LifeDrain, Scale: 1.5f);

                Vector2 direction = Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.PiOver2);
                if (Main.rand.NextBool())
                    direction *= -1f;

                dust.velocity = direction * dust.velocity.Length() * 1.6f;
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(12)){
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.TopazBolt, Scale: 1f);
                dust.velocity = Vector2.Zero;
            }

            spriteScale = MathHelper.Min(spriteScale + 0.3f, 1f);
            projectile.rotation = projectile.velocity.ToRotation();

            Lighting.AddLight(projectile.Center, Color.Red.ToVector3() * 0.6f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 2;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);

            int size = 16;
            Rectangle hitbox = new Rectangle((int)projectile.Center.X - size / 2, (int)projectile.Center.Y - size / 2, size, size);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.dontTakeDamage && npc.Hitbox.Intersects(hitbox))
                {
                    int hitDirection = Math.Sign(npc.Center.X - projectile.Center.X);
                    npc.StrikeNPC(projectile.damage / 2, projectile.knockBack / 2, hitDirection);
                }
            }

            for (int d = 0; d < 8; d++)
            {
                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.LifeDrain, Scale: 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }

            for (int d = 0; d < 4; d++)
            {
                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.TopazBolt, Scale: 1.2f);
                dust.velocity *= 1.5f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Color color = projectile.GetAlpha(lightColor);
            Vector2 origin = new Vector2(texture.Width - 12, texture.Height / 2);
            SpriteEffects spriteEffects = projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale * spriteScale,
                spriteEffects,
                0f);

            return false;
        }
    }
}
