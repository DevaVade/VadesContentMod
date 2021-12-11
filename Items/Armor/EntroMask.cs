using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EntroMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropion Mask");
            Tooltip.SetDefault(
                "20% increased melee damage\n" +
                "10% increased melee critical strike chance\n" +
                "10% increased damage reduction");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 26;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(platinum: 1);
            item.defense = 110;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.1f;
            player.meleeDamage += 0.2f;
            player.meleeCrit += 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => 
            body.type == ModContent.ItemType<EntroBody>() && legs.type == ModContent.ItemType<EntroPants>();

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<VadPlayer>().entroSet = true;

            player.setBonus =
                "10% increased damage reduction\n" +
                "Melee attacks increase damage by 20% each\n" +
                "this effect is capped at 60% damage\n" +
                "not attacking with melee will cause the bonus to reduce";

            player.endurance += 0.1f;
            player.meleeDamage += player.GetModPlayer<VadPlayer>().entroDamageBonus;
        }
    }
}
