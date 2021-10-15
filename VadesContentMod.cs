using Terraria.ModLoader;

namespace VadesContentMod
{
	public class VadesContentMod : Mod
	{
		internal static VadesContentMod Instance;

		internal bool ThoriumLoaded;
		internal bool CalamityLoaded;
		internal bool SoALoaded;
		internal bool DLCLoaded;
		internal bool FargoLoaded;
		internal bool SoulsLoaded;

		public override uint ExtraPlayerBuffSlots
		{
			get
			{
				return 300U;
			}
		}

		public VadesContentMod()
		{
			base.Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
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

		public override void Load()
		{
			Instance = this;
		}

		internal static modConfig modConfig;

		internal bool calamityLoaded;
	}
}