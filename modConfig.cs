using Terraria.ModLoader;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace VadesContentMod
{
	internal class modConfig : ModConfig
	{
            public override ConfigScope Mode
            {
                get
		        {
		            return 0;
		        }
            }

            public override void OnLoaded()
	        {
		        VadesContentMod.modConfig = this;
	        }

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