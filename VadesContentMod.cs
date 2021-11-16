using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace VadesContentMod
{
    public class VadesContentMod : Mod
    {
        public static Effect TrailEffect;
        public static ModHotKey OneShothotKey;

        internal bool ThoriumLoaded;
        internal bool CalamityLoaded;
        internal bool SoALoaded;
        internal bool DLCLoaded;
        internal bool FargoLoaded;
        internal bool SoulsLoaded;

        public override uint ExtraPlayerBuffSlots => 300U;

        public override void Load()
        {
            OneShothotKey = RegisterHotKey("Destructor Armor One-Shot", "F");

            if (Main.netMode != NetmodeID.Server)
            {
                TrailEffect = GetEffect("Effects/TrailShader");

                Ref<Effect> invertRef = new Ref<Effect>(GetEffect("Effects/Grayscale"));
                Ref<Effect> shockwaveRef = new Ref<Effect>(GetEffect("Effects/Shockwave"));

                Filters.Scene["VadesContentMod:Grayscale"] = new Filter(new ScreenShaderData(invertRef, "Main"), EffectPriority.VeryHigh);
                Filters.Scene["VadesContentMod:Shockwave"] = new Filter(new ScreenShaderData(shockwaveRef, "Shockwave"), EffectPriority.VeryHigh);
            }
        }

        public override void Unload()
        {
            OneShothotKey = null;
            TrailEffect = null;
        }

        public override void PostSetupContent()
        {
            ThoriumLoaded = ModLoader.GetMod("ThoriumMod") != null;
            CalamityLoaded = ModLoader.GetMod("CalamityMod") != null;
            DLCLoaded = ModLoader.GetMod("FargowiltasSoulsDLC") != null;
            SoulsLoaded = ModLoader.GetMod("FargowiltasSouls") != null;
            FargoLoaded = ModLoader.GetMod("Fargowiltas") != null;
            SoALoaded = ModLoader.GetMod("SacredTools") != null;
        }
    }
}