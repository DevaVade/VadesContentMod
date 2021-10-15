using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class GodCurse : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Non-Existent");
            Description.SetDefault("You are no longer existent");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            longerExpertDebuff = true;
			Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VadPlayer>().godCurse = true;

            player.endurance = 0f;
            player.meleeDamage = 0.01f;
            player.rangedDamage = 0.01f;
            player.magicDamage = 0.01f;
            player.minionDamage = 0.01f;
            player.meleeCrit = 0;
            player.rangedCrit = 0;
            player.magicCrit = 0;
            player.maxMinions = 1;
            player.maxTurrets = 1;

            player.manaCost += 0.90f;

            player.armorPenetration = 0;
            
            player.statLifeMax2 = 100;
            player.statManaMax2 = 10;

            player.moveSpeed -= 0.9f;

            player.immune = false;
            player.immuneNoBlink = false;
            player.immuneTime = 0;
            player.noFallDmg = false;

            player.ichor = true;
			player.onFire = true;
			player.onFire2 = true;
			player.onFrostBurn = true;
			player.poisoned = true;
			player.venom = true;

			player.wingTime = 0;
            player.wingTimeMax = 0;
            player.rocketTime = 0;
			player.slow = true;
			player.moonLeech = true;

			if (player.beetleDefense)
            {
                player.beetleOrbs = 0;
                player.beetleCounter = 0;
            }

            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                if (Main.debuff[i]) player.buffImmune[i] = false;
            }

            player.KillMe(PlayerDeathReason.ByOther(player.Male ? 14 : 15), 1.0, 0, false);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense = 0;
            npc.defDefense = 0;
            npc.GetGlobalNPC<NPCs.VadNPC>().GodCurse = true;

            npc.ichor = true;
			npc.onFire = true;
			npc.onFire2 = true;
			npc.onFrostBurn = true;
			npc.poisoned = true;
			npc.shadowFlame = true;
			npc.venom = true;
            npc.betsysCurse = true;

			for (int i = 0; i < BuffLoader.BuffCount; i++)
	        {
			     if (Main.debuff[i]) npc.buffImmune[i] = false;
		    }
        }
    }
}