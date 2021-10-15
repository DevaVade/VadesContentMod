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
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Capture;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs;
using CalamityMod.World;
using Terraria.Graphics.Shaders;
using Terraria.UI;
using VadesContentMod;

namespace VadesContentMod
{
	public partial class VadPlayer : ModPlayer
	{
	    public bool GodlikePower; 
        public bool GodCurse; 
		public bool GodCurse2;
		public bool GodGauntlet;
		public bool OmniSet; 
		public bool MutantAttack; 
		public int MutantCD;
		public int BlastCD;
		public bool Evil;
		public bool Xiao;
		public bool Tiky;
        public bool GODSet;
        public bool Sus;
	   
	    public override void ResetEffects()
	    { 
	        GodlikePower = false;
            GodCurse = false;
			GodCurse2 = false;
			GodGauntlet = false;
			OmniSet = false;
			MutantAttack = false;
			Evil = false;
		    Xiao = false;
			Tiky = false;
            Sus = false;
            GODSet = false;
	    }
	    public override void UpdateDead()
        {
		    GodlikePower = false;
            GodCurse = false;
			GodCurse2 = false;
			GodGauntlet = false;
			OmniSet = false;
			MutantAttack = false;
			Evil = false;
			Xiao = false;
			Tiky = false;
			Sus = false;
            GODSet = false;
	    }
        public override void PostUpdateMiscEffects()
        {
             if (GodCurse)
             {
                player.statDefense = 0;
                player.endurance = 0;
				player.ghost = true;
             }
        }
        public override void UpdateBadLifeRegen()
        {
              if (GodCurse)
              {
                 if (player.lifeRegen > 0)
                     player.lifeRegen = 0;
                 player.lifeRegen -= 1700;

                 player.lifeRegenTime = 0;

                 if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                 player.lifeRegenCount -= 6000000;
              }

			  if (GodCurse2)
              {
                 if (player.lifeRegen > 0)
                     player.lifeRegen = 0;
                 player.lifeRegen -= 1700;

                 player.lifeRegenTime = 0;

                 if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                 player.lifeRegenCount -= 60000;
              }

			  if (Evil)
              {
                 if (player.lifeRegen > 0)
                     player.lifeRegen = 0;

                 player.lifeRegenTime = 0;

                 if (player.lifeRegenCount > 0)
                    player.lifeRegenCount = 0;

                 player.lifeRegenCount -= 4;
              }

			  if (this.GodlikePower)
	          {
				  if (ModLoader.GetMod("CalamityMod") != null)
				  {
				       CalamityPlayer calamityPlayer = (CalamityPlayer)base.player.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer");
		               if (base.player.lifeRegen > 20)
		               {
			               int num = base.player.lifeRegen - 20;
			               base.player.lifeRegen = 20 + num * 2;
		               }
		               if (base.player.endurance > 0.3f)
		               {
			               float num2 = base.player.endurance - 0.3f;
			               base.player.endurance = 0.3f + num2 * 4f;
		               }
	              }
			  }
        }

        public override void PreUpdate()
	    {
              if (GodlikePower)
	          {  
                   base.player.statLife = base.player.statLifeMax2;
		           if (base.player.statLifeMax < 100)
		           {
		           base.player.statLifeMax = 100;
		           }
		           if (base.player.statLifeMax2 < 100)
		           {
		           base.player.statLifeMax2 = 100;
		           }

				   base.player.statLife = base.player.statLifeMax2;
				   base.player.ghost = false;
				   base.player.dead = false;
              }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
               return !this.GodlikePower;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
               return !this.GodlikePower;
        }

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
		       if (GodGauntlet)
			   {
			        target.AddBuff(mod.BuffType("GodCurse2"), 1000);
			   }
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
		       if (GodGauntlet)
			   {
			        target.AddBuff(mod.BuffType("GodCurse2"), 1000);
			   }
		}

		public override void OnHitPvp(Item item, Player target, int damage, bool crit)
		{
		       if (GodGauntlet)
			   {
			        target.AddBuff(mod.BuffType("GodCurse2"), 2);
			   }
		}

		public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
		{
		       if (GodGauntlet)
			   {
			        target.AddBuff(mod.BuffType("GodCurse2"), 2);
			   }
		}

        public override void PreUpdateBuffs()
	    {
		    base.PreUpdateBuffs();
		    if (base.player == Main.LocalPlayer)
		    {
			    for (int i = base.player.CountBuffs() - ModContent.GetInstance<modConfig>().ExtraPlayerBuff; i > 0; i--)
			    {
				    int num = -1;
				    for (int j = 0; j < Player.MaxBuffs; j++)
				    {
					    if (!Main.debuff[base.player.buffType[j]] && base.player.buffTime[j] > 0)
					    {
						    num = j;
					    }
				    }
				    if (num == -1)
				    {
					    return;
				    }
				    base.player.DelBuff(num);
			    }
		    }
	    }
    }
}