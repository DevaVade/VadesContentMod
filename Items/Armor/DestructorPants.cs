using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DestructorPants : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
            item.value = Item.sellPrice(gold: 5);
            item.defense = 50000;
        }
    }
}
