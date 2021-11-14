using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public abstract class OuroborosSegment : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ouroboros");

            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 100;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<VadGlobalProjectile>().TimeFreezeImmune = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public void PortalDusts(int amount, int spawnSize = 64)
        {
            for (int d = 0; d < amount; d++)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - Vector2.One * (spawnSize / 2), spawnSize, spawnSize, DustID.FireworkFountain_Red, Scale: 3f);
                dust.velocity = Main.rand.NextVector2Circular(10f, 10f);
                dust.noGravity = true;
            }
        }

        public override bool? CanCutTiles() => false;

        public override bool MinionContactDamage() => true;
    }

    public class OuroborosHead : OuroborosSegment
    {
        public readonly float RangeSQ = (float)Math.Pow(1000, 2);

        public NPC Target => Main.npc[CurrentTarget];

        public int CurrentTarget
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public int Follower
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public Vector2 IdleOffset
        {
            get => new Vector2(projectile.localAI[0], projectile.localAI[1]);
            set
            {
                projectile.localAI[0] = value.X;
                projectile.localAI[1] = value.Y;
            }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            drawOriginOffsetY = -80;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            bool firstTick = projectile.GetGlobalProjectile<VadGlobalProjectile>().firstTick;

            if (firstTick)
            {
                IdleOffset = projectile.Center + projectile.velocity * 60f - player.Center;

                PortalDusts(600, 160);

                for (int d = 0; d < 40; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.RedTorch, Scale: 6f);
                    dust.velocity = Main.rand.NextVector2Circular(15f, 15f);
                    dust.noGravity = true;
                }

                projectile.netUpdate = true;
            }

            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<Buffs.Ouroboros>());

            if (player.HasBuff(ModContent.BuffType<Buffs.Ouroboros>()))
                projectile.timeLeft = 2;

            if (Main.myPlayer == projectile.owner && Follower == -1)
            {
                Follower = Projectile.NewProjectile(
                    projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<OuroborosBody>(),
                    projectile.damage,
                    projectile.knockBack,
                    projectile.owner,
                    projectile.whoAmI,
                    20); // Segments

                projectile.netUpdate = true;
            }

            float speed = 40f;
            Vector2 targetPosition;

            if (player.HasMinionAttackTargetNPC)
            {
                targetPosition = Main.npc[player.MinionAttackTargetNPC].Center;
            }
            else
            {
                if (CurrentTarget == -1)
                    FindTarget();

                if (CurrentTarget != -1)
                {
                    NPC target = Main.npc[CurrentTarget];
                    if (!target.active) CurrentTarget = -1;

                    targetPosition = target.Center;
                }
                else
                {
                    // Idle
                    speed *= 0.75f;

                    if (Main.myPlayer == projectile.owner && Vector2.DistanceSquared(player.Center + IdleOffset, projectile.Center) < Math.Pow(120f, 2))
                    {
                        IdleOffset = Main.rand.NextVector2Circular(1f, 0.6f) * Main.rand.NextFloat(900f, 1200f);
                        projectile.netUpdate = true;
                    }

                    targetPosition = player.Center + IdleOffset;
                }
            }

            Vector2 targetVelocity = Vector2.Normalize(targetPosition - projectile.Center) * speed;

            projectile.velocity = Vector2.Lerp(projectile.velocity, targetVelocity, 0.018f);

            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;
        }

        private void FindTarget()
        {
            CurrentTarget = -1;
            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(this)) continue;

                float distanceSQ = Vector2.DistanceSquared(projectile.Center, npc.Center);

                bool inRange = distanceSQ <= RangeSQ;
                bool closest = currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ;
                bool inSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);


                if (inRange && closest && inSight)
                {
                    CurrentTarget = i;
                    currentDistanceSQ = distanceSQ;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = new Vector2(texture.Width / 2, texture.Height - projectile.height / 2);
            Color color = projectile.GetAlpha(lightColor);

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.localAI[0]);
            writer.Write(projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            IdleOffset = new Vector2(x, y);
        }
    }

    public class OuroborosBody : OuroborosSegment
    {
        public int Following => (int)projectile.ai[0];

        public int DistanceFromTail => (int)projectile.ai[1];

        public int Follower
        {
            get => (int)projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public bool HasMoved
        {
            get => projectile.localAI[1] == 1f;
            set => projectile.localAI[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            projectile.timeLeft = 2;

            bool firstTick = projectile.GetGlobalProjectile<VadGlobalProjectile>().firstTick;

            if (Main.myPlayer == projectile.owner && DistanceFromTail >= 0 && Follower == 0)
            {
                Follower = Projectile.NewProjectile(
                    projectile.Center,
                    Vector2.Zero,
                    DistanceFromTail > 0 ? projectile.type : ModContent.ProjectileType<OuroborosTail>(),
                    projectile.damage,
                    projectile.knockBack,
                    projectile.owner,
                    projectile.whoAmI,
                    DistanceFromTail - 1);

                projectile.netUpdate = true;
            }

            if (Following >= 0 && Following < Main.maxProjectiles)
            {
                Projectile follow = Main.projectile[Following];

                if (!follow.active)
                {
                    projectile.Kill();
                }
                else
                {
                    // Follow next body part
                    Vector2 velocity = follow.Center - projectile.Center;
                    projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;

                    float len = velocity.Length();
                    float distance = projectile.width * 0.8f;

                    velocity *= (len - distance) / len;

                    projectile.velocity = Vector2.Zero;

                    if (len - distance > 0f)
                    {
                        if (!HasMoved)
                        {
                            PortalDusts(60);
                            HasMoved = true;
                        }

                        projectile.position += velocity;
                    }
                }
            }
        }
    }

    public class OuroborosTail : OuroborosBody { }
}
