using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.NPCs
{
    public partial class VadGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool TimeFrozen = false;
        public bool firstTick = true;
        public bool godlikePower;
        public bool godCurse;
        public bool godCurse2;

        public override void ResetEffects(NPC npc)
        {
            TimeFrozen = false;
            godlikePower = false;
            godCurse = false;
            godCurse2 = false;
        }

        public override bool PreAI(NPC npc)
        {
            bool doAI = true;
            if (TimeFrozen && !firstTick)
            {
                npc.position = npc.oldPosition;
                npc.frameCounter = 0;
                doAI = false;
            }

            if (firstTick)
                firstTick = false;

            return doAI;
        }

        public override void SetDefaults(NPC npc)
        {
            int extraNpcBuff = ModContent.GetInstance<VadConfig>().ExtraNpcBuff;

            npc.buffType = new int[extraNpcBuff];
            npc.buffTime = new int[extraNpcBuff];
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (godCurse)
            {
                if (Main.rand.NextBool(7))
                {
                    Dust dust = Dust.NewDustDirect(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.AmethystBolt, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Main.DiscoColor, 3.5f);
                    dust.noGravity = true;
                    dust.velocity *= 1.2f;
                    dust.velocity.Y -= 0.15f;

                    if (Main.rand.NextBool(4))
                    {
                        dust.noGravity = false;
                        dust.scale *= 0.5f;
                    }
                }

                Lighting.AddLight(npc.position, 0.15f, 0.03f, 0.09f);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (godCurse)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 1000;
                if (damage < 20000000)
                {
                    damage = 20000000;
                }
            }

            if (godCurse2)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 777;
                if (damage < 4500)
                {
                    damage = 4500;
                }
            }

            if (TimeFrozen && npc.life == 1)
            {
                if (npc.lifeRegen < 0)
                    npc.lifeRegen = 0;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (TimeFrozen)
            {
                npc.life = 1;
                return false;
            }

            return base.CheckDead(npc);
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot) => !TimeFrozen;

        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (TimeFrozen && npc.life == 1)
                return false;

            return base.CanBeHitByItem(npc, player, item);
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (TimeFrozen && npc.life == 1)
                return false;

            return base.CanBeHitByProjectile(npc, projectile);
        }
    }
}