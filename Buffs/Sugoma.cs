using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class Sugoma : ModBuff
    { 
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sussy");
            Description.SetDefault("Makes you extremely suspicoiusus and 690% damage bonus");

            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.allDamage += 6.9f;
            player.GetModPlayer<VadPlayer>().sus = true;
        }
    }
}