using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class TimeFrozen : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Slow;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.VadGlobalNPC>().TimeFrozen = true;
        }
    }
}
