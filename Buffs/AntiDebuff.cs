using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.CalPlayer;
using FargowiltasSouls;

namespace VadesContentMod.Buffs
{
    public class AntiDebuff : ModBuff
    { 
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Peak Evolution");
            Description.SetDefault("Immune to every debuffs");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[base.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < BuffLoader.BuffCount; i++)
	        {
		        if (Main.debuff[i])
	            {
			           player.buffImmune[i] = true;
			           if (this.calamity != null)
			           {
				                player.buffImmune[this.calamity.BuffType("RageMode")] = false;
				                player.buffImmune[this.calamity.BuffType("AdrenalineMode")] = false;
			           }
	            }
	        }
        }
    }
}