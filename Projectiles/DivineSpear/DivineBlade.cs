using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.DivineSpear
{
    public class DivineBlade : ModProjectile
    {
        private readonly float RangeSQ = (float)Math.Pow(1200, 2);
        private readonly float MaxVelocitySQ = (float)Math.Pow(40, 2);
        private readonly int MaxTimeLeft = 300;

        private float ColorTimer = 0f;
        private float previousRotation = 0f;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.scale = 1.5f;
            projectile.timeLeft = MaxTimeLeft;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public bool HomeIn => projectile.ai[1] != 0f;

        public override void AI()
        {
            if (projectile.velocity.LengthSquared() < MaxVelocitySQ && (projectile.timeLeft > MaxTimeLeft - 15 || !HomeIn))
            {
                projectile.velocity *= 1.05f;
            } else if (HomeIn)
            {
                ChaseNPC();
            }

            UpdateVisuals();
            SpawnDusts();
        }

        private void UpdateVisuals()
        {
            projectile.alpha = (int)MathHelper.Max(projectile.alpha - 30, 0);

            previousRotation = projectile.rotation;
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver4 + MathHelper.PiOver2;

            ColorTimer++;
        }

        private void SpawnDusts()
        {

            Vector2 dustPosition = projectile.position - projectile.velocity * 4f;
            for (int d = 0; d < 1; d++)
            {
                Dust dust = Dust.NewDustDirect(dustPosition, projectile.width, projectile.height, DustID.LifeDrain, Scale: 1f);
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        private void ChaseNPC()
        {
            if (CurrentTarget == -1)
                CurrentTarget = TargetNPC();

            if (CurrentTarget != -1)
            {
                NPC target = Main.npc[CurrentTarget];

                if (target.active)
                {
                    float speed = 50f;
                    float inertia = 25f;

                    Vector2 direction = Vector2.Normalize(target.Center - projectile.Center);
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction * speed) / inertia;
                }
                else
                {
                    CurrentTarget = -1;
                }
            } else
            {
                projectile.velocity *= 1.06f;
            }
        }

        private int TargetNPC()
        {
            int currentNPC = -1;
            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.CanBeChasedBy()) continue;

                float distanceSQ = Vector2.DistanceSquared(npc.Center, projectile.Center);

                bool inSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                bool closest = currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ;
                bool inRange = distanceSQ <= RangeSQ;

                if (inSight && closest && inRange)
                {
                    currentDistanceSQ = distanceSQ;
                    currentNPC = i;
                }
            }

            return currentNPC;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, projectile.Center);

            for (int d = 0; d < 10; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.LifeDrain, Scale: 3f);
                dust.velocity *= 5f;
                dust.noGravity = true;
                dust.noLight = false;
            }

            for (int d = 0; d < 15; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RedTorch, Scale: 5f);
                dust.velocity *= 8f;
                dust.noGravity = true;
                dust.noLight = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(texture.Width - projectile.width / 2, projectile.height / 2);

            // Afterimages
            Texture2D trailTexture = mod.GetTexture("Projectiles/DivineSpear/DivineBladeTrail");
            int trails = 6;

            for (int i = 0; i < trails; i++)
            {
                int reverseIndex = trails - i;
                float rotationDiff = projectile.rotation - previousRotation;

                Vector2 position = projectile.Center - projectile.velocity * reverseIndex * 1f;
                Color trailColor = GetColor(ColorTimer - reverseIndex);

                spriteBatch.Draw(
                    trailTexture,
                    position - Main.screenPosition,
                    null,
                    trailColor * (0.2f + i * 0.1f) * projectile.Opacity,
                    projectile.rotation - rotationDiff * reverseIndex,
                    origin,
                    projectile.scale * 0.8f,
                    SpriteEffects.None,
                    0f);
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                GetColor(ColorTimer) * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        private Color GetColor(float timer)
        {
            float lerp = 0.7f + (float)Math.Sin(timer * 0.3f) * 0.2f;
            return Color.Lerp(Color.White, Color.Red, lerp);
        }
    }
}
