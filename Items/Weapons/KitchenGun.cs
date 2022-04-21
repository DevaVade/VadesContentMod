using Terraria.DataStructures;
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
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.damage = 50;
            Item.knockBack = 2f;
            Item.useAnimation = 12;
            Item.useTime = 4;
            Item.reuseDelay = 14;
            Item.shoot = 10;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item31;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
        }

        public override bool CanConsumeAmmo(Player player) => player.itemAnimation >= Item.useAnimation - 2;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 46f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

            if (player.itemAnimation < 4)
            {
                type = ModContent.ProjectileType<Projectiles.KitchenBullet>();
                knockback *= 2f;
            } else
            {
                for (int i = 0; i < 2; i++)
                {
                    return base.Shoot(player, source, position, velocity.RotatedByRandom(MathHelper.ToRadians(3)), type, damage / 2, knockback);
                }

                return false;
            }

            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(6f, 2f);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ClockworkAssaultRifle)
                .AddIngredient(ItemID.SoulofMight, 20)
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
