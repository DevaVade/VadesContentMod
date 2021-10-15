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
            projectile.ownerHitCheck = true;
            projectile.penetrate = -1;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
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
            Utils.PlotTileLine(player.Center, player.Center + projectile.velocity * 280f, 80f, DelegateMethods.CastLight);
        }

        private void SpawnDusts(Player player)
        {
            Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);
            float spread = MathHelper.PiOver4 * 0.8f;

            if (Main.rand.NextBool(10))
            {
                int id = Dust.NewDust(player.Center + direction * 10f, 5, 5, DustID.LifeDrain, 0, 0, 0, default, 3f);
                Main.dust[id].velocity = direction.RotatedBy(Main.rand.NextFloat(-spread, spread)) * 8f;
                Main.dust[id].fadeIn = 0f;
            }

            if (Main.rand.NextBool(8))
            {
                int id = Dust.NewDust(player.Center + direction * 10f, 5, 5, DustID.TopazBolt, 0, 0, 0, default, 1.5f);
                Main.dust[id].velocity = direction.RotatedBy(Main.rand.NextFloat(-spread * 1.5f * projectile.direction)) * 6f;
                Main.dust[id].noGravity = false;
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
                player.Center + projectile.velocity * 360f,
                300f,
                ref point);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Item14, target.Center);

            for (int d = 0; d < 5; d++)
            {
                int id = Dust.NewDust(target.Center, 5, 5, DustID.LifeDrain, 0, 0, 0, default, 2f);
                Main.dust[id].fadeIn *= 0.3f;
            }

            for (int d = 0; d < 5; d++)
            {
                int id = Dust.NewDust(target.Center, 5, 5, DustID.TopazBolt);
                Main.dust[id].velocity *= 2f;
                Main.dust[id].noGravity = false;
            }

            if (Main.rand.NextBool(3)) { 
                int id = Gore.NewGore(
                    target.Center, 
                    Main.rand.NextVector2Circular(2f, 2f), 
                    Main.rand.Next(GoreID.ChimneySmoke1, GoreID.ChimneySmoke2 + 1));
                Main.gore[id].timeLeft = (int)(Main.gore[id].timeLeft * 0.7f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.3f);
        }
    }
}
