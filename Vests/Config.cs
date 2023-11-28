using Exiled.API.Interfaces;

namespace Vests
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = true;

        public float LightArmorValue { get; set; } = 50;

        public float CombatArmorValue { get; set; } = 70;

        public float HeavyArmorValue { get; set; } = 100;
    }
}
