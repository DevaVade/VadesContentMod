using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class ReviveCooldown : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Confused;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Revive Cooldown");
            Description.SetDefault("You can't revive");

            Main.buffNoSave[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VadPlayer>().reviveCooldown = true;
        }
    }
}
