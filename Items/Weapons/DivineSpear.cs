using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Weapons
{
    public class DivineSpear : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 106;
            item.height = 106;
            item.damage = 2000;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noUseGraphic = true;
            item.useAnimation = item.useTime = 28;
            item.noMelee = true;
            item.melee = true;
            item.shootSpeed = 20f;
            item.knockBack = 8f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(gold: 10);
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.DivineSpear.DivineSpearProjectile>();
            item.UseSound = SoundID.Item15;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] < 1;
    }
}
