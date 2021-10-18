using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace VadesContentMod.Sounds.Item
{
	public class SUS : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			soundInstance = sound.CreateInstance();
			soundInstance.Volume = volume * .5f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = 0f;
			Main.PlaySoundInstance(soundInstance);
			return soundInstance;
		}
	}
}
