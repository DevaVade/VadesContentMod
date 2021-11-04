using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Projectiles
{
    public class VadGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int counter = 0;
        private bool firstTick = true;

        public const int TimeFreezeMoveDuration = 10;
        public int TimeFrozen = 0;
        public bool TimeFreezeImmune;
        public bool TimeFreezeCheck;

        public override bool PreAI(Projectile projectile)
        {
            bool doAI = true;
            counter++;

            if (TimeFrozen > 0 && !firstTick && !TimeFreezeImmune)
            {
                if (counter % projectile.MaxUpdates == 0)
                    TimeFrozen--;

                if (counter > TimeFreezeMoveDuration * projectile.MaxUpdates)
                {
                    projectile.position = projectile.oldPosition;
                    if (projectile.frameCounter > 0)
                        projectile.frameCounter--;

                    projectile.timeLeft++;
                    doAI = false;
                }
            }

            if (firstTick)
                firstTick = false;

            return doAI;
        }

        public override void PostAI(Projectile projectile)
        {
            if (!TimeFreezeCheck)
            {
                TimeFreezeCheck = true;
                if (projectile.whoAmI == Main.player[projectile.owner].heldProj)
                    TimeFreezeImmune = true;
            }
        }
    }
}
