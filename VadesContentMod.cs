using Terraria.ModLoader;

namespace VadesContentMod
{
	public class VadesContentMod : Mod
	{
		internal static modConfig modConfig;

		internal bool ThoriumLoaded;
		internal bool CalamityLoaded;
		internal bool SoALoaded;
		internal bool DLCLoaded;
		internal bool FargoLoaded;
		internal bool SoulsLoaded;

		public override uint ExtraPlayerBuffSlots => 300U;

		public VadesContentMod()
		{
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