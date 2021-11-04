using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.DivineSpear
{
    public class DivineSpearProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divine Spear");
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 48;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.scale = 1.5f;
            projectile.ownerHitCheck = true;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
        }

        public float MovementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float ShootTimer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.direction = projectile.spriteDirection = player.direction;
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation;

            if (!player.frozen)
            {
                if (MovementFactor == 0f)
                {
                    MovementFactor = 3f;
                    projectile.netUpdate = true;
                }

                if (player.itemAnimation < player.itemAnimationMax / 2)
                    MovementFactor -= 0.5f;
                else
                    MovementFactor += 0.5f;

                if (player.itemAnimation < player.itemAnimationMax - 5 && ++ShootTimer > 5f)
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        float spread = MathHelper.PiOver4 * 0.2f;
                        Projectile.NewProjectile(
                            projectile.Center,
                            projectile.velocity.RotatedBy(Main.rand.NextFloat(-spread, spread)) * 0.4f,
                            ModContent.ProjectileType<DivineBlade>(),
                            projectile.damage / 2,
                            projectile.knockBack,
                            projectile.owner,
                            -1f, 1f); // Homing blade
                    }

                    ShootTimer = 0f;
                }
            }

            if (player.itemAnimation == 0)
            {
                projectile.Kill();
            }

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.LifeDrain, Scale: 2f);
                dust.noGravity = false;
                dust.noLight = false;
                dust.velocity = projectile.velocity * dust.velocity.Length() * 0.1f;
            }

            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.position.X = ownerMountedCenter.X - projectile.width / 2f;
            projectile.position.Y = ownerMountedCenter.Y - projectile.height / 2f;

            projectile.position += projectile.velocity * MovementFactor;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation += MathHelper.PiOver2;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.NPCHit53, projectile.Center);

            for (int d = 0; d < 2; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RedTorch, Scale: 1.2f);
                dust.noGravity = false;
                dust.noLight = false;
                dust.velocity *= 2f;
            }

            for (int d = 0; d < 2; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, Scale: 1.5f);
                dust.velocity *= 2f;
            }

            if (Main.myPlayer != projectile.owner) return;

            for (int i = 0; i < 3; i++)
            {
                Vector2 position;

                if (Main.rand.NextBool())
                {
                    // Top/bottom
                    position = new Vector2(Main.rand.NextFloat(-Main.screenWidth / 2, Main.screenWidth / 2), Main.screenHeight + projectile.height);
                    if (Main.rand.NextBool()) position.Y *= -1f;
                }
                else
                {
                    // Right/left
                    position = new Vector2(Main.screenWidth + projectile.width, Main.rand.NextFloat(-Main.screenHeight / 2, Main.screenHeight / 2));
                    if (Main.rand.NextBool()) position.X *= -1f;
                }

                position += Main.player[projectile.owner].position;

                Vector2 direction = Vector2.Normalize(target.Center - position);
                float speed = 2f + i * 3f;

                Projectile.NewProjectile(
                    position,
                    direction * speed,
                    ModContent.ProjectileType<DivineBlade>(),
                    projectile.damage / 2,
                    projectile.knockBack,
                    projectile.owner);
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = Vector2.One * 10f;

            if (projectile.spriteDirection == -1)
            {
                origin.Y = texture.Height - origin.Y;
            }

            Color color = GetAlpha(lightColor) ?? lightColor;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0f);

            return false;
        }
    }
}
