using Noise.Modules.Generator;
using Noise.Modules.Operator;

namespace Noise.Modules.Complex
{
    public class Planet : Billow
    {
        ModuleBase _land;

        public Planet()
        {
            var flat = new ScaleBias(0.2, 0.1, new Perlin { OctaveCount = 3 });
            var mountains = new ScaleBias(0.5, 0.5, new RidgedMultifractal { Frequency = 4f, OctaveCount = 5 });
            var terrainType = new Perlin { Frequency = 0.5f, Lacunarity = 0.1f, OctaveCount = 4, Persistence = 1 };
            _land = new Select(flat, mountains, terrainType);
        }

        public override double GetValue(double x, double y, double z)
        {
            var selectorVal = base.GetValue(x, y, z);

            if (selectorVal > 0)
                return _land.GetValue(x, y, z);

            return selectorVal;
        }
    }
}