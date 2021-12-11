using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using VadesContentMod.Buffs;
using VadesContentMod.Projectiles;

namespace VadesContentMod
{
    public class VadPlayer : ModPlayer
    {
        private int MaxFreeze = -1;
        private float oldMusicFade = 0f;

        public bool FreezeTime = false;
        public int freezeLength = 180;

        public float entroDamageBonus = 0;
        public int entroDamageTimer = 0;

        public int screenShake = 0;

        public int shockwaveTime = -1;

        public bool destructorSet;
        public bool entroSet;

        public bool reviveCooldown;
        public bool reviveBuff;
        public bool oneShotCooldown;
        public bool oneShot;
        public bool godlikePower;
        public bool godCurse;
        public bool godCurse2;
        public bool godGauntlet;
        public bool omniSet;
        public bool mutantAttack;
        public int mutantCD;
        public int blastCD;
        public bool evil;
        public bool xiao;
        public bool tiky;
        public bool godSet;
        public bool sus;

        public override void ResetEffects()
        {
            reviveBuff = false;
            reviveCooldown = false;
            destructorSet = false;
            entroSet = false;
            oneShotCooldown = false;
            oneShot = false;
            godlikePower = false;
            godCurse = false;
            godCurse2 = false;
            godGauntlet = false;
            omniSet = false;
            mutantAttack = false;
            evil = false;
            xiao = false;
            tiky = false;
            sus = false;
            godSet = false;
        }

        public override void UpdateDead()
        {
            ResetEffects();

            FreezeTime = false;
            freezeLength = 0;
        }

        public override void PostUpdateMiscEffects()
        {
            UpdateEntroDamage();

            if (screenShake > 0) screenShake--;

            if (godCurse)
            {
                player.statDefense = 0;
                player.endurance = 0;
                player.ghost = true;
            }

            if (FreezeTime && freezeLength > 0)
            {
                if (MaxFreeze < 0)
                    MaxFreeze = freezeLength;

                // Shader
                if (Main.netMode != NetmodeID.Server)
                {
                    if (!Filters.Scene["VadesContentMod:Grayscale"].IsActive())
                    {
                        Filters.Scene.Activate("VadesContentMod:Grayscale");
                    }

                    float progress = MathHelper.Max(freezeLength - 120f, 0f) / MaxFreeze * 2f;
                    if (progress > 1f) progress = 1f;

                    Filters.Scene["VadesContentMod:Grayscale"].GetShader().UseProgress(progress);
                }

                // Stop music
                if (!Main.dedServ)
                {
                    if (freezeLength < 5)
                    {
                        if (oldMusicFade > 0)
                        {
                            Main.musicFade[Main.curMusic] = oldMusicFade;
                            oldMusicFade = 0;
                        }
                    }
                    else
                    {
                        if (oldMusicFade == 0)
                        {
                            oldMusicFade = Main.musicFade[Main.curMusic];
                        }
                        else
                        {
                            for (int i = 0; i < Main.musicFade.Length; i++)
                            {
                                Main.musicFade[i] = 0f;
                            }
                        }
                    }
                }

                // Freeze NPCs
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.HasBuff(ModContent.BuffType<TimeFrozen>()))
                    {

                        npc.AddBuff(ModContent.BuffType<TimeFrozen>(), freezeLength);
                    }
                }

                // Freeze projectiles
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && !(p.minion && !ProjectileID.Sets.MinionShot[p.type]))
                    {
                        var globalProj = p.GetGlobalProjectile<VadGlobalProjectile>();

                        if (!globalProj.TimeFreezeImmune && globalProj.TimeFrozen == 0)
                            globalProj.TimeFrozen = freezeLength;
                    }
                }

                freezeLength--;

                if (freezeLength <= 60 && Main.netMode != NetmodeID.Server && Filters.Scene["VadesContentMod:Grayscale"].IsActive())
                {
                    Filters.Scene.Deactivate("VadesContentMod:Grayscale");
                }

                if (freezeLength <= 0)
                {
                    FreezeTime = false;
                    freezeLength = 180;
                    MaxFreeze = -1;

                    for (int i = 0; i < Main.maxNPCHitSounds; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.dontTakeDamage && npc.life == 1 && npc.lifeMax > 1)
                            npc.StrikeNPC(9999, 0f, 0);
                    }
                }
            }

            if (Main.netMode != NetmodeID.Server)
            {
                if (shockwaveTime >= 0)
                {
                    if (!Filters.Scene["VadesContentMod:Shockwave"].IsActive())
                    {
                        Filters.Scene.Activate("VadesContentMod:Shockwave", player.Center).GetShader().UseColor(3f, 8f, 25f);
                    }

                    float progress = shockwaveTime / 30f;
                    float opacity = 100f - (progress * 80f);
                    Filters.Scene["VadesContentMod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(opacity).UseTargetPosition(player.Center);


                    if (shockwaveTime++ >= 120)
                        shockwaveTime = -1;
                }
                else if (Filters.Scene["VadesContentMod:Shockwave"].IsActive())
                {
                    Filters.Scene.Deactivate("VadesContentMod:Shockwave");
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (godCurse)
            {
                if (player.lifeRegen > 0)
                    player.lifeRegen = 0;
                player.lifeRegen -= 1700;

                player.lifeRegenTime = 0;

                if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                player.lifeRegenCount -= 6000000;
            }

            if (godCurse2)
            {
                if (player.lifeRegen > 0)
                    player.lifeRegen = 0;
                player.lifeRegen -= 666;

                player.lifeRegenTime = 0;

                if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                player.lifeRegenCount -= 666;
            }

            if (evil)
            {
                if (player.lifeRegen > 0)
                    player.lifeRegen = 0;

                player.lifeRegenTime = 0;

                if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                player.lifeRegenCount -= 4;
            }
        }

        public override void PreUpdate()
        {
            if (godlikePower)
            {
                player.statLife = player.statLifeMax2;
                if (player.statLifeMax < 100)
                {
                    player.statLifeMax = 100;
                }
                if (player.statLifeMax2 < 100)
                {
                    player.statLifeMax2 = 100;
                }

                player.statLife = player.statLifeMax2;
                player.ghost = false;
                player.dead = false;
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) => !godlikePower && !reviveBuff;

        public override bool CanBeHitByProjectile(Projectile proj) => !godlikePower && !reviveBuff;

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (entroSet && item.melee) AddEntroDamage();

            if (oneShot)
            {
                target.life = 1;
                target.StrikeNPC(1, 0f, 1);
            }
            else if (godGauntlet)
            {
                // TODO
                //target.AddBuff(ModContent.BuffType<GodCurse2>(), 1000);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (entroSet && proj.melee) AddEntroDamage();

            if (oneShot)
            {
                target.life = 1;
                target.StrikeNPC(1, 0f, 1);
            }
            else if (godGauntlet)
            {
                // TODO
                //target.AddBuff(ModCOntent.BuffType<GodCurse2>(), 1000);
            }
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (oneShot)
            {
                int hitDirection = player.position.X > target.position.X ? 1 : -1;
                target.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 999999, hitDirection, true);
            }
            else if (godGauntlet)
            {
                // TODO
                // target.AddBuff(ModContent.BuffType<GodCurse2>(), 2);
            }
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if (oneShot)
            {
                int hitDirection = proj.position.X > target.position.X ? 1 : -1;
                target.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 999999, hitDirection, true);
            }
            else if (godGauntlet)
            {
                // TODO
                //target.AddBuff(ModContent.BuffType<GodCurse2>(), 2);
            }
        }

        public override void PreUpdateBuffs()
        {
            if (player != Main.LocalPlayer) return;

            for (int i = player.CountBuffs() - ModContent.GetInstance<VadConfig>().ExtraPlayerBuff; i > 0; i--)
            {
                int num = -1;
                for (int j = 0; j < Player.MaxBuffs; j++)
                {
                    if (!Main.debuff[player.buffType[j]] && player.buffTime[j] > 0)
                    {
                        num = j;
                    }
                }

                if (num == -1) return;

                player.DelBuff(num);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!oneShotCooldown && destructorSet && VadesContentMod.OneShothotKey.JustPressed)
            {
                oneShot = oneShotCooldown = true;
                player.AddBuff(ModContent.BuffType<OneShot>(), 1900);
                player.AddBuff(ModContent.BuffType<OneShotCooldown>(), 7200);

                Main.PlaySound(SoundID.NPCDeath56);

                for (int d = 0; d < 30; d++)
                {
                    Dust dust = Dust.NewDustDirect(player.Center, 5, 5, DustID.LifeDrain);
                    dust.velocity *= 2;
                }

                for (int d = 0; d < 10; d++)
                {
                    Dust.NewDust(player.Center, 5, 5, DustID.TopazBolt);
                }
            }
        }

        public void WingStats2()
        {
            if (destructorSet && player.controlDown && player.controlJump && !player.mount.Active)
            {
                player.position.Y -= player.velocity.Y;
                if (player.velocity.Y > 0.1f)
                    player.velocity.Y = 0.1f;
                else if (player.velocity.Y < -0.1f)
                    player.velocity.Y = -0.1f;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (!reviveCooldown && destructorSet)
            {
                Main.PlaySound(SoundID.NPCDeath56);

                int heal = player.statLifeMax2;

                player.statLife = (int)MathHelper.Min(player.statLife + heal, player.statLifeMax2);
                player.HealEffect(heal);

                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }

                player.AddBuff(ModContent.BuffType<ReviveCooldown>(), 2400, true);
                player.AddBuff(ModContent.BuffType<ReviveBuff>(), 600, true);

                return false;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void ModifyScreenPosition()
        {
            if (screenShake > 0)
            {
                float shakeValue = screenShake;
                Main.screenPosition.X += Main.rand.NextFloat(-shakeValue, shakeValue);
                Main.screenPosition.Y += Main.rand.NextFloat(-shakeValue, shakeValue);
            }
        }

        private void AddEntroDamage()
        {
            entroDamageBonus = MathHelper.Min(entroDamageBonus + 0.02f, 0.6f);
            entroDamageTimer = 120;
        }

        private void UpdateEntroDamage()
        {
            if (entroSet)
            {
                if (--entroDamageTimer <= 0)
                {
                    entroDamageTimer = 0;
                    entroDamageBonus = MathHelper.Max(entroDamageBonus - 0.001f, 0f);
                }
            } else
            {
                entroDamageTimer = 0;
                entroDamageBonus = 0f;
            }
        }
    }
}