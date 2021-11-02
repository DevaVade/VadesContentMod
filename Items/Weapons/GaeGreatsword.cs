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
            Tooltip.SetDefault("'It seems your journey has come to an end, join me then as we watch the sunset and after that we say goodbye so that i can reunite with her.' -before he faded\nThe final piece of your journey, now what shall you do?\nLore:\nAn Supposedly omnipotent god or a close friend has been defeated by the hands of a terrarian.\nBefore he draws his last breath, he gave you this gift with the intent of protecting you from any danger.\nEven if you can't talk to him anymore, his soul still within this blade with the other gods.");

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
            item.value = Item.sellPrice(platinum: 699);
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
        public override void UpdateInventory(Player player)
        {
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;
            player.accFishFinder = true;
            player.accDreamCatcher = true;
            player.accOreFinder = true;
            player.accStopwatch = true;
            player.accCritterGuide = true;
            player.accJarOfSouls = true;
            player.accThirdEye = true;
            player.accCalendar = true;
            player.accWeatherRadio = true;

            player.statManaMax2 += 999;

            player.statLifeMax2 += 120000;

            player.endurance += 500f;

            player.lavaImmune = true;
            player.manaFlower = true;
            player.manaMagnet = true;
            player.magicCuffs = true;
            player.ignoreWater = true;
            player.pStone = true;
            player.findTreasure = true;
            player.noKnockback = true;
            player.thorns = 100f;
            player.AddBuff(mod.BuffType("AntiDebuff"), 2);
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
