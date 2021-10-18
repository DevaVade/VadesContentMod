using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Sounds.Music
{
	public class AMONGDRIP : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .9f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 0f;
			Main.PlaySoundInstance(soundInstance);
			return soundInstance;
		}
	}
}
