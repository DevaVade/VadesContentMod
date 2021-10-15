using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace VadesContentMod
{
    internal class VadConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Range(0, 1000)]
        [DefaultValue(50)]
        [Label("Extra npc buffs")]
        [Tooltip("Put the max buffs needed for the npcs")]
        public int ExtraNpcBuff;

        [Label("Buff Slots")]
        [Tooltip("Put how much buff slots you need")]
        [Range(1, 300)]
        [Increment(1)]
        [DrawTicks]
        [DefaultValue(210)]
        public int ExtraPlayerBuff;
    }
}