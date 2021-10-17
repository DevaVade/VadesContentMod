using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class OneShotCooldown : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.BrokenArmor;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Overpowered Cooldown");
            Description.SetDefault("You can't get overpowered");

            Main.buffNoSave[Type] = true; //to bypass trhough the antidebuff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VadPlayer>().oneShotCooldown = true;
        }
    }
}
