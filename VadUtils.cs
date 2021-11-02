using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;

namespace VadesContentMod
{
    public static class VadUtils
    {
        public static void ButcherNPCs(Rectangle? hitbox = null)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || (hitbox.HasValue && !npc.Hitbox.Intersects(hitbox.Value))) continue;

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
        }
    }
}
