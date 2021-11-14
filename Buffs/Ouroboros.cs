using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Buffs
{
    public class Ouroboros : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "Terraria/Buff_" + BuffID.Fishing;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ouroboros");
            Description.SetDefault("A dimensional worm of energy will fight for you");

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.GaeGreatsword.OuroborosHead>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
