using VadesContentMod.Helpers;
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
        public override string Texture => "Terraria/Projectile_" + ProjectileID.None;

        public const float MaxCharge = 90f;
        public const int StepLength = 256;
        public readonly int Steps = 20;

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
            projectile.width = 128;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
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
            if (!IsCharged) return false;

            Color BeamColor = projectile.GetAlpha(lightColor);

            Texture2D texture = mod.GetTexture("Textures/Circle");
            float extraScale = 1.1f + (float)Math.Sin(Main.GlobalTime * 2) * 0.1f;
            float scale = projectile.scale * 2 * extraScale;
            
            for (int i = 0; i < 10; i++)
            {
                Main.spriteBatch.Draw(
                    texture,
                    projectile.position - Main.screenPosition,
                    null,
                    BeamColor * 0.1f * (10f - i),
                    0f,
                    texture.Size() / 2,
                    scale * (i / 10f),
                    SpriteEffects.None,
                    0f);
            }

            Vector2 start = projectile.position;
            Vector2 end = start + projectile.velocity * Steps * StepLength * projectile.scale;
            float width = projectile.width * projectile.scale * 0.7f;

            Vector2 normal = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;
            float timeOffset = (-Main.GlobalTime * 6f) % 1;

            PrimitivePacket packet;

            // Flame
            for (int step = 0; step < Steps; step++)
            {
                Vector2 flameStart = start + projectile.velocity * StepLength * step;
                Vector2 flameEnd = flameStart + projectile.velocity * StepLength;

                packet = new PrimitivePacket(mod.GetTexture("Textures/Flames"), "Texture");

                packet.Add(flameStart + normal * extraScale, BeamColor * 0.4f, new Vector2(timeOffset, 0f));
                packet.Add(flameStart - normal * extraScale, BeamColor * 0.4f, new Vector2(timeOffset, 1f));
                packet.Add(flameEnd + normal * extraScale, BeamColor * 0.4f, new Vector2(1 + timeOffset, 0f));

                packet.Add(flameStart - normal * extraScale, BeamColor * 0.4f, new Vector2(timeOffset, 1f));
                packet.Add(flameEnd - normal * extraScale, BeamColor * 0.4f, new Vector2(1 + timeOffset, 1f));
                packet.Add(flameEnd + normal * extraScale, BeamColor * 0.4f, new Vector2(1 + timeOffset, 0f));

                packet.Send(spriteBatch);
            }

            // Main laser
            normal *= 1f;
            packet = new PrimitivePacket(mod.GetTexture("Textures/Trail1"), "Texture");

            packet.Add(start + normal * 2 * extraScale, BeamColor, Vector2.Zero);
            packet.Add(start - normal * 2 * extraScale, BeamColor, Vector2.UnitY);
            packet.Add(end + normal * 2 * extraScale, BeamColor, Vector2.UnitX);

            packet.Add(start - normal * 2 * extraScale, BeamColor, Vector2.UnitY);
            packet.Add(end - normal * 2 * extraScale, BeamColor, Vector2.One);
            packet.Add(end + normal * 2 * extraScale, BeamColor, Vector2.UnitX);

            packet.Send(spriteBatch);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Main.hslToRgb(Main.GlobalTime * 0.4f % 1f, 1f, 0.6f);

        public override bool ShouldUpdatePosition() => false;
    }
}
