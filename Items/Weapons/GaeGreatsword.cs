using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
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
            Tooltip.SetDefault("Developer Cheat Item\nThe final blade of your journey.\nA blade that embodies not war and violence but a hope for those that are worthy.\nThe pure unfiltered infinite power of the blade that cuts timelines and omniverses out of existence.\nLeft click normally and oblitirate everything with one single slash, Right click to fire a infinity beam that will eviscerate anything touching it.\nLeft click and press UP to summon omniverse destroying entities by your side.");
            //Edit the description if adding more attacks to this weapon

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 20));
        }

        public override void SetDefaults()
        {
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.melee = true;
            item.damage = 999999;
            item.knockBack = 100f;
            item.crit = 100;
            item.rare = ItemRarityID.Purple;
            item.channel = true;
            item.value = Item.sellPrice(platinum: 699);
            item.shoot = ModContent.ProjectileType<InfinitySlash>();
            item.shootSpeed = 1;
        }

        public override bool CanUseItem(Player player)
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.channel = true;
            item.useTime = item.useAnimation = 0;
            item.UseSound = SoundID.Item15;
            item.buffType = 0;
            item.melee = true;
            item.summon = false;

            if (player.altFunctionUse != 2)
            {
                if (player.controlUp)
                {
                    item.channel = false;
                    item.useTime = item.useAnimation = 45;
                    item.useStyle = ItemUseStyleID.SwingThrow;
                    item.UseSound = SoundID.Item77;
                    item.melee = false;
                    item.summon = true;
                    item.buffType = ModContent.BuffType<Buffs.Ouroboros>();
                    item.shoot = ModContent.ProjectileType<OuroborosHead>();
                } else
                {
                    item.shoot = ModContent.ProjectileType<InfinitySlash>();
                }
            } else
            {
                item.shoot = ModContent.ProjectileType<InfinityBeam>();
            }

            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ModContent.ProjectileType<OuroborosHead>())
            {
                position = Main.MouseWorld;

                if (player.ownedProjectileCounts[type] < 1)
                {
                    player.AddBuff(item.buffType, 2);

                    Projectile.NewProjectile(
                        position, 
                        Vector2.Normalize(player.Center - position).RotatedByRandom(MathHelper.PiOver4) * 30f, 
                        type, damage, knockBack, player.whoAmI, 
                        -1, -1);

                }

                int dusts = 30;
                for (int d = 0; d < dusts; d++)
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.LifeDrain, Scale: 5f);
                    dust.noGravity = true;
                    dust.velocity = Vector2.UnitY.RotatedBy(MathHelper.TwoPi / dusts * d) * 12f;
                }

                for (int d = 0; d < 10; d++)
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.TopazBolt, Scale: 2f);
                    dust.velocity *= 2.5f;
                }

                Lighting.AddLight(position, Color.Red.ToVector3());

                return false;
            }

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                tt.text = "∞ damage";
            }
        }

        public override void HoldItem(Player player)
        {
            int starsTotal = 6;
            int lifeStar = ModContent.ProjectileType<LifeStar>();
            if (Main.myPlayer != player.whoAmI || player.ownedProjectileCounts[lifeStar] >= starsTotal) return;

            float separation = MathHelper.TwoPi / starsTotal;
            for (int i = 0; i < starsTotal; i++)
            {
                float rotation = i * separation;
                Projectile.NewProjectile(
                    player.Center + rotation.ToRotationVector2() * 64f,
                    Vector2.Zero,
                    lifeStar,
                    16969,
                    100f,
                    player.whoAmI,
                    rotation, -1f);
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
            player.AddBuff(ModContent.BuffType<Buffs.AntiDebuff>(), 2);

            if (ModLoader.GetMod("CalamityMod") != null)
            {
                if (player.lifeRegen > 20)
                {
                    int num = player.lifeRegen - 20;
                    player.lifeRegen = 20 + num * 2;
                }
                if (player.endurance > 0.3f)
                {
                    float num2 = player.endurance - 0.3f;
                    player.endurance = 0.3f + num2 * 4f;
                }
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

        public override bool AltFunctionUse(Player player) => true;

        public override Color? GetAlpha(Color lightColor) => Color.White;
    }
}
