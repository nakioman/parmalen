using System.Media;
using System.Reflection;

namespace Parmalen.Engine
{
    public static class Helpers
    {
        public static void PlayResourceSound(string soundName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(soundName))
            {
                using (var soundPlayer = new SoundPlayer(stream))
                {
                    soundPlayer.Play();
                }
            }
        }
    }
}