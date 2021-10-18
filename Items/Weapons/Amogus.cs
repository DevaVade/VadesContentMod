using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Weapons
{
    public class Amogus : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.mana = 8;
            item.damage = 2000;
            item.shoot = ModContent.ProjectileType<Projectiles.AmogusProjectile>();
            item.shootSpeed = 20f;
            item.useTime = item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(gold: 10);
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Items/Amogus");
            item.noMelee = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
            return false;
        }
    }
}
