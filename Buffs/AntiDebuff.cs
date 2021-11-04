using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class AntiDebuff : ModBuff
    { 
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Peak Evolution");
            Description.SetDefault("Immune to every debuff");

            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            Mod calamity = ModLoader.GetMod("CalamityMod");
            int rage = calamity?.BuffType("RageMode") ?? -1;
            int adrenaline = calamity?.BuffType("AdrenalineMode") ?? -1;

            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                if (Main.debuff[i] && i != rage && i != adrenaline)
	            {
                    player.buffImmune[i] = true;
	            }
	        }
        }
    }
}
