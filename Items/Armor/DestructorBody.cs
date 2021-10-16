using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DestructorBody : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 32;
            item.value = Item.sellPrice(gold: 5);
            item.defense = 50000;
        }
    }
}
