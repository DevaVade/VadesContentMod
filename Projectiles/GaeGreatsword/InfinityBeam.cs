using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public class InfinityBeam : ModProjectile
    {
        public const float MaxCharge = 90f;
        public const int StepLength = 256;
        public readonly int Steps = 2;

        private bool playSound = true;

        public bool IsCharged => Charge >= MaxCharge;

        public float Charge
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public float ColorTimer
        {
            get => projectile.localAI[1];
            set => projectile.localAI[1] = value;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.scale = 4f;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            projectile.GetGlobalProjectile<VadGlobalProjectile>().TimeFreezeImmune = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position = player.Center;
            projectile.timeLeft = 2;

            UpdatePlayer(player);
            ChargeBeam(player);

            if (IsCharged)
            {
                CastLights();

                if (playSound)
                {
                    Main.PlaySound(29, (int)projectile.position.X, (int)projectile.position.Y, 104);
                    playSound = false;
                }
            }

            ColorTimer++;
        }

        private void ChargeBeam(Player player)
        {
            if (!player.controlUseTile || player.altFunctionUse != 2 || player.HeldItem.type != ModContent.ItemType<Items.Weapons.GaeGreatsword>())
            {
                projectile.Kill();
                return;
            }

            if (Charge < MaxCharge)
                Charge++;
        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                if (IsCharged)
                {
                    Main.LocalPlayer.GetModPlayer<VadPlayer>().screenShake = 6;
                    projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;

                    float rotation = projectile.velocity.ToRotation();
                    float targetRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();

                    if (targetRotation > rotation + MathHelper.Pi)
                        targetRotation -= MathHelper.TwoPi;
                    else if (targetRotation < rotation - MathHelper.Pi)
                        targetRotation += MathHelper.TwoPi;

                    projectile.velocity = MathHelper.Lerp(rotation, targetRotation, 0.1f).ToRotationVector2();
                } else
                {
                    projectile.velocity = Vector2.Normalize((Main.MouseWorld - player.MountedCenter));
                }

                projectile.netUpdate = true;
            }

            projectile.rotation = projectile.velocity.ToRotation();

            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsCharged) return false;

            Player player = Main.player[projectile.owner];
            float point = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(), 
                player.Center, player.Center + projectile.velocity * Steps * StepLength * projectile.scale,
                240f * projectile.scale,
                ref point);
        }

        private void CastLights()
        {
            DelegateMethods.v3_1 = projectile.GetAlpha(Color.White).ToVector3();
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * Steps * StepLength, 128, DelegateMethods.CastLight);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * Steps * StepLength, 128, DelegateMethods.CutTiles);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (IsCharged)
            {
                Texture2D texture = Main.projectileTexture[projectile.type];

                Color color = projectile.GetAlpha(lightColor);
                Vector2 origin = new Vector2(0, texture.Height / 2);

                for (int i = 0; i < Steps; i++)
                {
                    spriteBatch.Draw(
                        texture,
                        projectile.position + (projectile.velocity * StepLength * projectile.scale * i) - Main.screenPosition,
                        null,
                        color,
                        projectile.rotation,
                        origin,
                        projectile.scale,
                        SpriteEffects.None,
                        0f);
                }
            }

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(Color.Red, Color.Yellow, ((float)Math.Sin(ColorTimer * 0.05f) + 1f) / 2f);

        public override bool ShouldUpdatePosition() => false;
    }
}
