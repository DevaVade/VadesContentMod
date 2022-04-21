using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace VadesContentMod.Projectiles
{
    public class KitchenBullet : ModProjectile
    {
        private float spriteScale = 0f;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;

                Projectile.velocity *= 0.5f;

                float maxSpeed = 10f;
                float speed = Projectile.velocity.Length();
                if (speed > maxSpeed)
                    Projectile.velocity *= maxSpeed / speed;
            }

            
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, Scale: 1.5f);

                Vector2 direction = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2);
                if (Main.rand.NextBool())
                    direction *= -1f;

                dust.velocity = direction * dust.velocity.Length() * 1.6f;
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(12)){
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AmberBolt, Scale: 1f);
                dust.velocity = Vector2.Zero;
            }

            spriteScale = MathHelper.Min(spriteScale + 0.3f, 1f);
            Projectile.rotation = Projectile.velocity.ToRotation();

            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.6f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 2;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            int size = 16;
            Rectangle hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.dontTakeDamage && npc.Hitbox.Intersects(hitbox))
                {
                    int hitDirection = Math.Sign(npc.Center.X - Projectile.Center.X);
                    npc.StrikeNPC(Projectile.damage / 2, Projectile.knockBack / 2, hitDirection);
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
                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.AmberBolt, Scale: 1.2f);
                dust.velocity *= 1.5f;
            }
        }

        public override bool PreDraw(ref Color color)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            color = Projectile.GetAlpha(color);
            Vector2 origin = new Vector2(texture.Width - 12, texture.Height / 2);
            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale * spriteScale, spriteEffects, 0);

            return false;
        }
    }
}
