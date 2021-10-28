using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.GaeGreatsword
{
    public class InfinitySlash : ModProjectile
    {
        public readonly float MaxCharge = 30f;
        public bool IsCharged => Charge >= MaxCharge;

        private bool playSound = true;

        public bool Released
        {
            get => projectile.ai[0] != 0f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }

        public float Charge
        {
            get => projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 11;
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ownerHitCheck = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.position = player.Center + projectile.velocity * 5f;
            projectile.rotation = projectile.velocity.ToRotation();


            if (Released)
                UpdateAnimation();
            else
            {
                projectile.timeLeft = 2;

                UpdatePlayer(player);
                ChargeSlash(player);
            }
        }

        private void ChargeSlash(Player player)
        {
            if (!player.channel)
            {
                if (IsCharged)
                {
                    Released = true;
                    projectile.timeLeft = 30;
                    player.itemTime = player.itemAnimation = 30;

                    if (projectile.owner == Main.myPlayer)
                    {
                        Main.PlaySound(SoundID.Item71);
                    }
                }
                else
                {
                    projectile.Kill();
                }

                return;
            }

            if (!IsCharged)
            {
                Charge++;
            }
            else
            {
                if (playSound && projectile.owner == Main.myPlayer)
                {
                    Main.PlaySound(SoundID.Item14);
                    playSound = false;
                }
            }
        }

        public override bool PreKill(int timeLeft)
        {
            if (Released)
            {
                // Butcher all NPCs
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.active || npc.friendly) continue;

                    npc.dontTakeDamage = false;

                    int damage = 999999;
                    npc.StrikeNPC(damage, 0f, 0, true);
                    npc.life = 1;
                    npc.lifeMax = 1;
                    npc.StrikeNPC(damage, 0f, 0, true);
                    npc.lifeRegen -= damage;

                    if (npc.life > 0)
                    {
                        npc.NPCLoot();
                    }

                    if (npc.type == NPCID.TargetDummy)
                    {
                        npc.active = false;
                        Main.PlaySound(npc.DeathSound, npc.position);
                        npc.NPCLoot();
                        WorldGen.KillTile((int)npc.position.X / 16, (int)npc.position.Y / 16);
                    }

                    npc.life = 0;
                    npc.active = false;
                    Main.npc[npc.whoAmI] = new NPC();
                }

                var modPlayer = Main.player[projectile.owner].GetModPlayer<VadPlayer>();
                if (!modPlayer.FreezeTime)
                {
                    modPlayer.FreezeTime = true;
                    modPlayer.freezeLength = 300;

                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Flashbang"));
                }
            }

            return base.PreKill(timeLeft);
        }

        private void UpdateAnimation()
        {
            if (projectile.frame < Main.projFrames[projectile.type] - 1)
            {
                projectile.frameCounter++;

                if (projectile.frameCounter > 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
            }
        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer && !Released)
            {
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.Center);
                projectile.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                projectile.netUpdate = true;
            }

            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
        }

        public override bool CanDamage() => Released;

        public override bool ShouldUpdatePosition() => false;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];

            int framesOnFirstColumn = 7;

            int frameWidth = texture.Width / 2;
            int frameHeight = texture.Height / framesOnFirstColumn;

            Rectangle sourceRect = new Rectangle(
                projectile.frame / framesOnFirstColumn,
                projectile.frame * frameHeight,
                frameWidth,
                frameHeight);

            Vector2 origin = new Vector2(frameWidth, frameHeight) / 2;
            origin.X += 20f;
            origin.Y -= 30f * projectile.direction;

            Color color = GetAlpha(lightColor) ?? lightColor;
            var spriteEffects = projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;

            spriteBatch.Draw(
                texture,
                projectile.position - Main.screenPosition,
                sourceRect,
                color * projectile.Opacity,
                projectile.rotation,
                origin,
                projectile.scale,
                spriteEffects,
                0f);

            return false;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
