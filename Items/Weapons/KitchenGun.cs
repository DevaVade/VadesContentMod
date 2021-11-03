using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Weapons
{
    public class KitchenGun : ModItem
    {
        public override void SetDefaults()
        {
            item.ranged = true;
            item.noMelee = true;
            item.damage = 50;
            item.knockBack = 2f;
            item.useAnimation = 12;
            item.useTime = 4;
            item.reuseDelay = 14;
            item.shoot = 10;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
            item.UseSound = SoundID.Item31;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.sellPrice(gold: 5);
            item.rare = ItemRarityID.Pink;
            item.autoReuse = true;
        }

        public override bool ConsumeAmmo(Player player) => player.itemAnimation >= item.useAnimation - 2;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 46f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            if (player.itemAnimation < 4)
            {
                type = ModContent.ProjectileType<Projectiles.KitchenBullet>();
                knockBack *= 2f;
            } else
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(position, velocity.RotatedByRandom(MathHelper.ToRadians(3)), type, damage / 2, knockBack, player.whoAmI);
                }

                return false;
            }

            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(6f, 2f);

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.ClockworkAssaultRifle);
            recipe.AddIngredient(ItemID.SoulofMight, 20);
            recipe.AddIngredient(ItemID.HallowedBar, 15);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);

            recipe.AddRecipe();
        }
    }
}
