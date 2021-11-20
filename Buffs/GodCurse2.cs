using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class GodCurse2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("God Curse");
            Description.SetDefault("The laws of reality has judged you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            canBeCleared = false;
            longerExpertDebuff = true;
			Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VadPlayer>().godCurse2 = true;

            player.endurance = 0f;

            player.armorPenetration = 0;

            player.immune = false;
            player.immuneNoBlink = false;
            player.immuneTime = 0;

			if (player.beetleDefense)
            {
                player.beetleOrbs = 0;
                player.beetleCounter = 0;
            }

            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                if (Main.debuff[i]) player.buffImmune[i] = false;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense = 0;
            npc.defDefense = 0;
            npc.GetGlobalNPC<NPCs.VadGlobalNPC>().godCurse2 = true;

			for (int i = 0; i < BuffLoader.BuffCount; i++)
	        {
			     if (Main.debuff[i]) npc.buffImmune[i] = false;
		    }
        }
    }
}