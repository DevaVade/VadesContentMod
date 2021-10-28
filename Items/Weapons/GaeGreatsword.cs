using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using VadesContentMod.Projectiles.GaeGreatsword;

namespace VadesContentMod.Items.Weapons
{
    public class GaeGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Finality");
            Tooltip.SetDefault("'Does not need an edgy tooltip'");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 20));
        }

        public override void SetDefaults()
        {
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.melee = true;
            item.damage = 1;
            item.knockBack = 100f;
            item.crit = 100;
            item.rare = ItemRarityID.Purple;
            item.channel = true;
            item.value = Item.sellPrice(platinum: 5);
            item.shoot = ModContent.ProjectileType<InfinitySlash>();
            item.shootSpeed = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                tt.text = "∞ damage";
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.mod == "Terraria" && line.Name == "Damage")
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

                float interpolate = ((float)Math.Sin(Main.GameUpdateCount * 0.06f) + 1f) / 2f;
                Color color = Color.Lerp(new Color(248, 9, 79), new Color(168, 29, 100), interpolate);
                Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), color, 1);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);

                return false;
            }

            return true;
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
