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
            projectile.width = 256;
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

            // texture used for flash
            Texture2D texture = mod.GetTexture("Textures/Circle");
            // make the beam slightly change scale with time
            float mult = (1.1f + (float)Math.Sin(Main.GlobalTime * 2) * 0.1f);
            // base scale for the flash so it actually connects with beam
            float scale = projectile.scale * 2 * mult;
            // draw flash
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

            PrimitivePacket packet = new PrimitivePacket() { Pass = "Texture" };

            Vector2 start = projectile.position;
            Vector2 end = projectile.position + projectile.velocity * Steps * StepLength * projectile.scale;
            float width = projectile.width * projectile.scale;

            // offset so i can make the triangles
            Vector2 offset = (start - end).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * width;
            PrimitivePacket.SetTexture(0, mod.GetTexture("Textures/Flames"));
            float off = -Main.GlobalTime % 1;

            // draw the flame part of the beam
            packet.Add(start + offset * mult, BeamColor * 0.4f, new Vector2(0 + off, 0));
            packet.Add(start - offset * mult, BeamColor * 0.4f, new Vector2(0 + off, 1));
            packet.Add(end + offset * mult, BeamColor * 0.4f, new Vector2(1 + off, 0));

            packet.Add(start - offset * mult, BeamColor * 0.4f, new Vector2(0 + off, 1));
            packet.Add(end - offset * mult, BeamColor * 0.4f, new Vector2(1 + off, 1));
            packet.Add(end + offset * mult, BeamColor * 0.4f, new Vector2(1 + off, 0));
            packet.Send();

            PrimitivePacket packet2 = new PrimitivePacket() { Pass = "Texture" };

            PrimitivePacket.SetTexture(0, mod.GetTexture("Textures/Trail1"));

            // draw the main part of the beam
            packet2.Add(start + offset * 2 * mult, BeamColor, Vector2.Zero);
            packet2.Add(start - offset * 2 * mult, BeamColor, Vector2.UnitY);
            packet2.Add(end + offset * 2 * mult, BeamColor, Vector2.UnitX);

            packet2.Add(start - offset * 2 * mult, BeamColor, Vector2.UnitY);
            packet2.Add(end - offset * 2 * mult, BeamColor, Vector2.One);
            packet2.Add(end + offset * 2 * mult, BeamColor, Vector2.UnitX);
            packet2.Send();

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(Color.Red, Color.Yellow, ((float)Math.Sin(ColorTimer * 0.05f) + 1f) / 2f);

        public override bool ShouldUpdatePosition() => false;
    }
}
