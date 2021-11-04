using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles
{
    public class EntropionBladeSlash : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 17;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 1;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = -1;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.scale = 2;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position = player.Center;
            projectile.timeLeft = 2;

            UpdateAnimation();
            UpdatePlayer(player);
            CastLights(player);
            SpawnDusts(player);

            if (!player.channel)
            {
                projectile.Kill();
            }

        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 directionToMouse = Vector2.Normalize(Main.MouseWorld - player.Center);

                projectile.velocity = directionToMouse;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
        }

        private void UpdateAnimation()
        {
            if (++projectile.frameCounter > 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }

        private void CastLights(Player player)
        {
            DelegateMethods.v3_1 = new Vector3(3f, 0f, 0f);
            Utils.PlotTileLine(
                player.Center,
                player.Center + projectile.velocity * 280f * projectile.scale,
                80f * projectile.scale,
                DelegateMethods.CastLight);
        }

        private void SpawnDusts(Player player)
        {
            Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);
            float spread = MathHelper.PiOver4 * 0.8f;

            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(player.Center + direction * 10f, 5, 5, DustID.LifeDrain, Scale: 3f);
                dust.velocity = direction.RotatedBy(Main.rand.NextFloat(-spread, spread)) * 8f * projectile.scale;
                dust.fadeIn = 0f;
            }

            if (Main.rand.NextBool(8))
            {
                Dust dust = Dust.NewDustDirect(player.Center + direction * 10f, 5, 5, DustID.TopazBolt, Scale: 1.5f);
                dust.velocity = direction.RotatedBy(Main.rand.NextFloat(-spread * 1.5f * projectile.direction)) * 6f * projectile.scale;
                dust.noGravity = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            int frameCount = Main.projFrames[projectile.type];
            int framesOnFirstColumn = (int)Math.Ceiling(frameCount / 2f);

            int frameWidth = texture.Width / 2;
            int frameHeight = texture.Height / framesOnFirstColumn;

            Color color = GetAlpha(lightColor) ?? lightColor;
            Vector2 origin = new Vector2(110f, frameHeight / 2);

            Rectangle sourceRectangle = new Rectangle(
                projectile.frame >= framesOnFirstColumn ? frameWidth : 0,
                (projectile.frame % framesOnFirstColumn) * frameHeight,
                frameWidth,
                frameHeight);

            spriteBatch.Draw(
                texture,
                projectile.position - Main.screenPosition,
                sourceRectangle,
                color * projectile.Opacity,
                projectile.velocity.ToRotation(),
                origin,
                projectile.scale,
                projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0f
                );

            return false;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;

            Utils.PlotTileLine(
                projectile.Center,
                projectile.Center + projectile.velocity * 360f,
                300f,
                DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            float point = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.Left(),
                targetHitbox.Size(),
                player.Center,
                player.Center + projectile.velocity * 360f * projectile.scale,
                300f * projectile.scale,
                ref point);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Item14, target.Center);

            for (int d = 0; d < 5; d++)
            {
                Dust dust = Dust.NewDustDirect(target.Center, 5, 5, DustID.LifeDrain, Scale: 2f);
                dust.fadeIn *= 0.3f;
            }

            for (int d = 0; d < 5; d++)
            {
                Dust dust = Dust.NewDustDirect(target.Center, 5, 5, DustID.TopazBolt);
                dust.velocity *= 2f;
                dust.noGravity = false;
            }

            if (Main.rand.NextBool(3))
            {
                Gore gore = Gore.NewGoreDirect(
                    target.Center,
                    Main.rand.NextVector2Circular(2f, 2f),
                    Main.rand.Next(GoreID.ChimneySmoke1, GoreID.ChimneySmoke2 + 1));
                gore.timeLeft = (int)(gore.timeLeft * 0.7f);
            }

            int godCurse = ModContent.BuffType<Buffs.GodCurse>();

            target.buffImmune[godCurse] = false;
            target.AddBuff(godCurse, 2);

            if (Main.myPlayer == projectile.owner)
            {
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
                        ModContent.ProjectileType<DivineSpear.DivineBlade>(),
                        projectile.damage / 2,
                        projectile.knockBack,
                        projectile.owner);
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.3f);
    }
}
