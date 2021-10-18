using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles.NPCs
{
    public class Fort : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("fard");
		}

		public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.penetrate = -1;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.friendly = true;
			base.projectile.magic = true;
			base.projectile.timeLeft = 180;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;
		}

		public override void AI()
		{
			Projectile projectile = base.projectile;
			projectile.velocity.X = projectile.velocity.X * 1.04f;
			Projectile projectile2 = base.projectile;
			projectile2.velocity.Y = projectile2.velocity.Y * 1.04f;
			base.projectile.rotation = base.projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			this.timer++;
			if (this.timer > 20)
			{
				for (int i = 0; i < 200; i++)
				{
					this.possibleTarget = Main.npc[i];
					this.distance = (this.possibleTarget.Center - base.projectile.Center).Length();
					if (this.distance < this.maxDistance && this.possibleTarget.active && !this.possibleTarget.dontTakeDamage && !this.possibleTarget.friendly && this.possibleTarget.chaseable && this.possibleTarget.lifeMax > 5 && !this.possibleTarget.immortal)
					{
						this.target = Main.npc[i];
						this.foundTarget = true;
						this.maxDistance = (this.target.Center - base.projectile.Center).Length();
					}
				}
				if (this.foundTarget)
				{
					Vector2 direction2 = this.target.Center - base.projectile.Center;
					direction2.Normalize();
					direction2.X *= this.projectileAcceleration;
					direction2.Y *= this.projectileAcceleration;
					base.projectile.velocity += direction2;
					if (base.projectile.velocity.Length() > this.topSpeed)
					{
						base.projectile.velocity = base.projectile.velocity.SafeNormalize(-Vector2.UnitY) * this.topSpeed;
					}
					if (!this.target.active)
					{
						this.foundTarget = false;
					}
				}
			}
			this.maxDistance = 500f;
			Lighting.AddLight(base.projectile.Center, 1f, 0.1f, 0.1f);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(214, 94, 106, base.projectile.alpha));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2((float)Main.projectileTexture[base.projectile.type].Width * 0.5f, (float)base.projectile.height * 0.5f);
			for (int i = 0; i < base.projectile.oldPos.Length; i++)
			{
				Vector2 drawPos = base.projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, base.projectile.gfxOffY);
				Color color = base.projectile.GetAlpha(lightColor) * ((float)(base.projectile.oldPos.Length - i) / (float)base.projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[base.projectile.type], drawPos, new Rectangle?(new Rectangle(0, base.projectile.frame * Main.projectileTexture[base.projectile.type].Height / Main.projFrames[base.projectile.type], Main.projectileTexture[base.projectile.type].Width, Main.projectileTexture[base.projectile.type].Height / Main.projFrames[base.projectile.type])), color, base.projectile.rotation, drawOrigin, base.projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Fard"));
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Boom"));
		}

		private bool firstTick = true;

		private float direction;

		private float projectileAcceleration = 16f;

		private float topSpeed = 32f;

		private int timer;

		private NPC target;

		private NPC possibleTarget;

		private bool foundTarget;

		private float maxDistance = 500f;

		private float distance;
	}
}
