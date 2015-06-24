using System.Collections.Generic;
using UnityEngine;

namespace Noise
{
    /// <summary>
    /// Provides a series of gradient presets
    /// </summary>
    public static class GradientPresets
    {
        #region Fields

        private static readonly Gradient _empty;
        private static readonly Gradient _grayscale;
        private static readonly Gradient _cloud;
        private static readonly Gradient _rgb;
        private static readonly Gradient _rgba;
        private static readonly Gradient _terrain;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Gradient.
        /// </summary>
        static GradientPresets()
        {
            // Generic gradient alpha keys
            var alphaKeys = new List<GradientAlphaKey> { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };

            // RGBA gradient alpha keys
            var rgbaAlphaKeys = new List<GradientAlphaKey> { new GradientAlphaKey(0, 2 / 3f), new GradientAlphaKey(1, 1) };

            // cloud alpha keys
            var cloudAlphaKeys = new List<GradientAlphaKey> { new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 1) };

            // Grayscale gradient color keys
            var grayscaleColorKeys = new List<GradientColorKey>
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1)
            };

            var cloudColorKeys = new List<GradientColorKey>
            {
                new GradientColorKey(new Color(1,1,1,0), 0f),
                new GradientColorKey(new Color(1,1,1,0), 0.5f),
                new GradientColorKey(new Color(1,1,1,0), 1f)
            };

            // RGB gradient color keys
            var rgbColorKeys = new List<GradientColorKey>
            {
                new GradientColorKey(Color.red, 0),
                new GradientColorKey(Color.green, 0.5f),
                new GradientColorKey(Color.blue, 1)
            };

            // RGBA gradient color keys
            var rgbaColorKeys = new List<GradientColorKey>
            {
                new GradientColorKey(Color.red, 0),
                new GradientColorKey(Color.green, 1 / 3f),
                new GradientColorKey(Color.blue, 2 / 3f),
                new GradientColorKey(Color.black, 1)
            };

            var one = 1f / 255f;
            
            // Terrain gradient color keys
            var terrainColorKeys = new List<GradientColorKey>
            {
                new GradientColorKey(new Color(one * 13,  one * 48,  one * 76), 0),      // deep ocean
                new GradientColorKey(new Color(one * 28,  one * 74,  one * 107), 0.42f), // shallow water
                new GradientColorKey(new Color(one * 169, one * 160, one * 124), 0.45f), // beach
                new GradientColorKey(new Color(one * 92,  one * 131, one * 57), 0.46f),  // green land
                new GradientColorKey(new Color(one * 125, one * 112, one * 88), 0.55f),  // arid / desert
                new GradientColorKey(new Color(one * 92,  one * 131, one * 57), 0.60f),  // green land
                new GradientColorKey(new Color(one * 11,  one * 40,  one * 4), 0.94f),   // forest
                new GradientColorKey(new Color(one * 236, one * 240, one * 249), 1)      // mountain peaks
            };

            _empty = new Gradient();

            _rgb = new Gradient();
            _rgb.SetKeys(rgbColorKeys.ToArray(), alphaKeys.ToArray());

            _rgba = new Gradient();
            _rgba.SetKeys(rgbaColorKeys.ToArray(), rgbaAlphaKeys.ToArray());

            _grayscale = new Gradient();
            _grayscale.SetKeys(grayscaleColorKeys.ToArray(), alphaKeys.ToArray());

            _cloud = new Gradient();
            _cloud.SetKeys(cloudColorKeys.ToArray(), cloudAlphaKeys.ToArray());

            _terrain = new Gradient();
            _terrain.SetKeys(terrainColorKeys.ToArray(), alphaKeys.ToArray());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty instance of Gradient.
        /// </summary>
        public static Gradient Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets the grayscale instance of Gradient.
        /// </summary>
        public static Gradient Grayscale
        {
            get { return _grayscale; }
        }

        /// <summary>
        /// Gets the RGB instance of Gradient.
        /// </summary>
        public static Gradient RGB
        {
            get { return _rgb; }
        }

        /// <summary>
        /// Gets the RGBA instance of Gradient.
        /// </summary>
        public static Gradient RGBA
        {
            get { return _rgba; }
        }

        /// <summary>
        /// Gets the terrain instance of Gradient.
        /// </summary>
        public static Gradient Terrain
        {
            get { return _terrain; }
        }

        public static Gradient Cloud { get { return _cloud; } }

        #endregion
    }
}