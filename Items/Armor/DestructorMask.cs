using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DestructorMask : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 26;
            item.value = Item.sellPrice(gold: 5);
            item.defense = 50000;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) =>
            body.type == ModContent.ItemType<DestructorBody>() && legs.type == ModContent.ItemType<DestructorPants>();
    }
}
