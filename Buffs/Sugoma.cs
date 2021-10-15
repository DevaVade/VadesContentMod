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
    public class Sugoma : ModBuff
    { 
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sussy");
            Description.SetDefault("makes you extremely suspicoiusus and 690% damage bonus");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[base.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.allDamage += 6.9f;
            player.GetModPlayer<VadPlayer>().Sus = true;
        }
    }
}