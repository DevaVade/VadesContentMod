using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public class LifeStar : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.FallingStar;

        private readonly float RangeSQ = (float)Math.Pow(1000f, 2);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 24;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.netImportant = true;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<VadGlobalProjectile>().TimeFreezeImmune = true;
        }

        public float Rotation
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value % MathHelper.TwoPi;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public float ShootTimer
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (
                !player.dead
                && player.HeldItem.type == ModContent.ItemType<Items.Weapons.GaeGreatsword>()
                && player.ownedProjectileCounts[projectile.type] >= 6
                )
                projectile.timeLeft = 2;

            UpdatePosition(player);
            UpdateAttack(player);
            BlockProjectiles();

            Lighting.AddLight(projectile.Center, Color.Red.ToVector3());
        }

        private void UpdateAttack(Player player)
        {
            if (ShootTimer > 0) ShootTimer--;

            if (CurrentTarget == -1) FindTarget(player);

            if (CurrentTarget != -1)
            {
                NPC target = Main.npc[CurrentTarget];

                if (!target.active)
                {
                    CurrentTarget = -1;
                }
                else
                {
                    if (ShootTimer <= 0f)
                    {
                        ShootTimer = 20f;

                        if (Main.myPlayer == projectile.owner)
                        {
                            Projectile.NewProjectile(
                                projectile.Center,
                                Vector2.Normalize(target.Center - projectile.Center) * 20f,
                                ProjectileID.Bullet,
                                projectile.damage,
                                projectile.knockBack,
                                projectile.owner);
                        }
                    }
                }
            }
        }

        private void UpdatePosition(Player player)
        {
            float rotationSpeed = MathHelper.TwoPi / 60f;
            Rotation += rotationSpeed;
            projectile.rotation -= rotationSpeed;

            projectile.Center = player.Center + Rotation.ToRotationVector2() * 90f;
        }

        private void BlockProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.hostile && proj.Hitbox.Intersects(projectile.Hitbox))
                {
                    proj.Kill();
                }
            }
        }

        private void FindTarget(Player player)
        {
            float currentDistanceSQ = -1f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.CanBeChasedBy()) continue;

                float distanceSQ = Vector2.DistanceSquared(player.Center, npc.Center);

                if (distanceSQ <= RangeSQ && (currentDistanceSQ == -1f || distanceSQ < currentDistanceSQ))
                {
                    CurrentTarget = i;
                    currentDistanceSQ = distanceSQ;
                }
            }
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            Vector2 origin = texture.Size() / 2;
            Color color = projectile.GetAlpha(lightColor);

            int trails = ProjectileID.Sets.TrailCacheLength[projectile.type];
            for (int i = 0; i < trails; i++)
            {
                spriteBatch.Draw(
                    texture,
                    projectile.oldPos[i] + projectile.Size / 2f - Main.screenPosition,
                    null,
                    color * 0.5f * (1f - ((float)i / trails)),
                    projectile.oldRot[i],
                    origin,
                    projectile.scale,
                    SpriteEffects.None,
                    0f);
            }

            spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                null,
                color,
                projectile.rotation,
                origin,
                projectile.scale,
                SpriteEffects.None,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Main.hslToRgb(Rotation / MathHelper.TwoPi, 1f, 0.6f);

        public override void SendExtraAI(BinaryWriter writer) => writer.Write(ShootTimer);

        public override void ReceiveExtraAI(BinaryReader reader) => ShootTimer = reader.ReadSingle();
    }
}
