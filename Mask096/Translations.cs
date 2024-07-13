using Exiled.API.Interfaces;

namespace Mask096
{
    public sealed class Translations : ITranslation
    {
        public string Scp096Hint { get; private set; } = "Mask on";
        public string PlayerSpawnHint { get; private set; } = "You have been given a mask for SCP-096";
        public string PlayerMaskHint { get; private set; } = "You are putting the mask on SCP-096, <color=yellow> {0} </color> seconds left";
        public string Scp096MaskHint { get; private set; } = "They put a mask on you, <color=yellow> {0} </color> seconds left";
    }
}
