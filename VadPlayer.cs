using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Capture;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs;
using CalamityMod.World;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using VadesContentMod;

namespace VadesContentMod
{
    public partial class VadPlayer : ModPlayer
    {
        public bool godlikePower;
        public bool godCurse;
        public bool GodCurse2;
        public bool godGauntlet;
        public bool OmniSet;
        public bool MutantAttack;
        public int MutantCD;
        public int BlastCD;
        public bool Evil;
        public bool Xiao;
        public bool Tiky;
        public bool GODSet;
        public bool sus;

        public override void ResetEffects()
        {
            godlikePower = false;
            godCurse = false;
            GodCurse2 = false;
            godGauntlet = false;
            OmniSet = false;
            MutantAttack = false;
            Evil = false;
            Xiao = false;
            Tiky = false;
            sus = false;
            GODSet = false;
        }
        public override void UpdateDead()
        {
            godlikePower = false;
            godCurse = false;
            GodCurse2 = false;
            godGauntlet = false;
            OmniSet = false;
            MutantAttack = false;
            Evil = false;
            Xiao = false;
            Tiky = false;
            sus = false;
            GODSet = false;
        }
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

            if (GodCurse2)
            {
                if (player.lifeRegen > 0)
                    player.lifeRegen = 0;
                player.lifeRegen -= 1700;

                player.lifeRegenTime = 0;

                if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                player.lifeRegenCount -= 60000;
            }

            if (Evil)
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
                player.statLife = base.player.statLifeMax2;
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

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return !godlikePower;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return !godlikePower;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 1000);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 1000);
            }
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 2);
            }
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if (godGauntlet)
            {
                target.AddBuff(mod.BuffType("GodCurse2"), 2);
            }
        }

        public override void PreUpdateBuffs()
        {
            if (player != Main.LocalPlayer) return;

            for (int i = player.CountBuffs() - ModContent.GetInstance<modConfig>().ExtraPlayerBuff; i > 0; i--)
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
    }
}