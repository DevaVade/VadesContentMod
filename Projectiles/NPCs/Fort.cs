using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.NPCs
{
    public class Fort : ModProjectile
    {
        private readonly float projectileAcceleration = 16f;
        private readonly float topSpeed = 32f;
        private float distance;

        public float MoveTimer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public int CurrentTarget
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("fard");
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 180;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void AI()
        {
            projectile.velocity *= 1.04f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (++MoveTimer > 20f)
            {
                if (CurrentTarget == -1)
                {
                    float currentDistance = -1;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        distance = Vector2.Distance(npc.Center, projectile.Center);

                        if ((currentDistance == -1 || distance < currentDistance) && npc.active && npc.CanBeChasedBy())
                        {
                            CurrentTarget = i;
                            currentDistance = distance;
                        }
                    }
                }

                if (CurrentTarget != -1)
                {
                    NPC target = Main.npc[CurrentTarget];

                    projectile.velocity += Vector2.Normalize(target.Center - projectile.Center) * projectileAcceleration;

                    if (projectile.velocity.Length() > topSpeed)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * topSpeed;
                    }

                    if (!target.active)
                        CurrentTarget = -1;
                }
            }

            Lighting.AddLight(projectile.Center, 1f, 0.1f, 0.1f);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(214, 94, 106, projectile.alpha);

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, projectile.height / 2);

            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / projectile.oldPos.Length);


                spriteBatch.Draw(
                    texture,
                    drawPos - Main.screenPosition,
                    new Rectangle(0,
                    projectile.frame * texture.Height / Main.projFrames[projectile.type],
                    texture.Width,
                    texture.Height / Main.projFrames[projectile.type]),
                    color,
                    projectile.rotation,
                    drawOrigin,
                    projectile.scale,
                    SpriteEffects.None,
                    0f);
            }

            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Fard"), projectile.Center);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Boom"), projectile.Center);
        }
    }
}
