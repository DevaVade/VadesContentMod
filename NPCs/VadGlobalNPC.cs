using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Shaders;
using Terraria.Localization;

namespace VadesContentMod.NPCs
{
	public partial class VadGlobalNPC : GlobalNPC
	{
        public override bool InstancePerEntity
        {
             get
             {
                  return true;
             }
        }
        public bool FirstTick = false;
	    public bool GodlikePower; 
        public bool GodCurse; 
        public bool GodCurse2;
	   
	    public override void ResetEffects(NPC npc)
	    { 
	        GodlikePower = false;
            GodCurse = false;
            GodCurse2 = false;
	    }

        public override void SetDefaults(NPC npc)
        {
		    npc.buffType = new int[ModContent.GetInstance<VadConfig>().ExtraNpcBuff];
	        npc.buffTime = new int[ModContent.GetInstance<VadConfig>().ExtraNpcBuff];
	    }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (GodCurse)
            {
                if (Main.rand.Next(7) < 6)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.2f;
                    Main.dust[dust].velocity.Y -= 0.15f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 0.15f, 0.03f, 0.09f);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (GodCurse)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 8000;
                if (damage < 20000000)
                {
                    damage = 20000000;
                }
            }

            if (GodCurse2)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 1200;
                if (damage < 150000)
                {
                    damage = 150000;
                }
            }
        }
    }
}