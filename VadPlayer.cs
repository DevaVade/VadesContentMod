using VadesContentMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace VadesContentMod
{
    public partial class VadPlayer : ModPlayer
    {
        public bool destructorSet;

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

        public override void UpdateDead() => ResetEffects();

        public override void PostUpdateMiscEffects()
        {
            if (godCurse)
            {
                player.statDefense = 0;
                player.endurance = 0;
                player.ghost = true;
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
                player.lifeRegen -= 1700;

                player.lifeRegenTime = 0;

                if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                player.lifeRegenCount -= 60000;
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

            if (this.godlikePower)
            {
                if (ModLoader.GetMod("CalamityMod") != null)
                {
                    if (player.lifeRegen > 20)
                    {
                        int num = player.lifeRegen - 20;
                        player.lifeRegen = 20 + num * 2;
                    }
                    if (player.endurance > 0.3f)
                    {
                        float num2 = player.endurance - 0.3f;
                        player.endurance = 0.3f + num2 * 4f;
                    }
                }
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
            if (oneShot)
            {
                target.life = 1;
                target.StrikeNPC(1, 0f, 1);
            } else if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 1000);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (oneShot)
            {
                target.life = 1;
                target.StrikeNPC(1, 0f, 1);
            } else if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 1000);
            }
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (oneShot)
            {
                int hitDirection = player.position.X > target.position.X ? 1 : -1;
                target.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 999999, hitDirection, true);
            } else if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 2);
            }
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if (oneShot)
            {
                int hitDirection = proj.position.X > target.position.X ? 1 : -1;
                target.KillMe(PlayerDeathReason.ByPlayer(player.whoAmI), 999999, hitDirection, true);
            } else if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 2);
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
                    int id = Dust.NewDust(player.Center, 5, 5, DustID.LifeDrain);
                    Main.dust[id].velocity *= 2;
                }

                for (int d = 0; d < 10; d++)
                {
                    Dust.NewDust(player.Center, 5, 5, DustID.TopazBolt);
                }
            }
        }

        public void WingStats()
        {
            player.wingTimeMax = 999999;
            player.wingTime = player.wingTimeMax;
            player.ignoreWater = true;
        }

        public void WingStats2()
        {
            player.wingTimeMax = 999999;
            player.wingTime = player.wingTimeMax;
            player.ignoreWater = true;

            if (player.controlDown && player.controlJump && !player.mount.Active)
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
    }
}