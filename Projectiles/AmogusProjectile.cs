using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles
{
    public class AmogusProjectile : ModProjectile
    {
        private readonly float DistanceSQ = (float)Math.Pow(1000, 2);

        private float spriteScale = 0f;

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 40;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.TwoPi / 15f;
            spriteScale = MathHelper.Min(spriteScale + 0.1f, 1f);

            if (CurrentTarget == -1) TargetNPC();

            NPC targetNPC = Main.npc[CurrentTarget];

            if (targetNPC.CanBeChasedBy(this))
            {
                float speed = 20f;
                float inertia = 30f;
                Vector2 velocity = Vector2.Normalize(targetNPC.Center - projectile.Center) * speed;
                projectile.velocity = (projectile.velocity * (inertia - 1) + velocity) / inertia;
            } else
            {
                CurrentTarget = -1;
            }
        }

        private int TargetNPC()
        {
            float currentDistance = -1f;
            int currentNPC = -1;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(this)) continue;

                float diff = (npc.Center - projectile.Center).LengthSquared();
                if (
                    diff <= DistanceSQ && (currentDistance == -1 || diff < currentDistance) &&
                    Collision.CanHitLine(
                        projectile.position, projectile.width, projectile.height,
                        npc.position, npc.width, npc.height)
                    )
                {
                    currentDistance = diff;
                    currentNPC = i;
                }
            }

            if (currentNPC != -1)
            {
                CurrentTarget = currentNPC;
            }

            return currentNPC;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.Center);

            for (int d = 0; d < 20; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.WhiteTorch, Scale: 2.5f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = GetAlpha(Color.White) ?? Color.White;

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale * spriteScale,
                SpriteEffects.None,
                0f);

            return false;
        }
    }
}
