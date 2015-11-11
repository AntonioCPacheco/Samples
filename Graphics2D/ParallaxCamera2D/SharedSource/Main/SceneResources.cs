﻿using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Graphics;

namespace ParallaxCamera2D
{
    public class SceneResources
    {
        public static Color BackgroundColor = new Color("#d6d1a4");
        public static Color Grass2ndPlaneColor = new Color("#41403b");
        public static Color Trees1stLayerColor = new Color("#41403b");
        public static Color Trees2ndLayerColor = new Color("#7a7663");
        public static Color Trees3rdLayerColor = new Color("#a6a284");
        public static Color TreeMistColor = new Color("#ede8c6");
        public static Color DarknessColor = Color.Black;
        public static Color Mushroom1Color = new Color("#292825");
        public static Color Mushroom2Color = new Color("#69675c");
        public static Color Mushroom3Color = Color.Black;
        public static Color DustParticleColor = new Color("#ece6ca");
        public static Color FenceColor = new Color("#2a2a2a");
        public static Color GraveyardColor = new Color("#767465");
        public static Color HouseHillColor = new Color("#3a3a3a");
        public static Color HouseHillMist = new Color("#f4f2d6");
        public static Color HauntedHouseColor = new Color("#3f3f3f");
        public static Color ScarecrowColor = new Color("#47463a");
        public static Color GhostColor = new Color("#3f3e32");
        public static Color CrowColor = new Color("#47463a");
        public static Color SunColor = new Color("#f7f3e3");       

        public static string[] GrassContent = new string[] { WaveContent.Assets.Textures.Grass.grass_0_png, WaveContent.Assets.Textures.Grass.grass_1_png, WaveContent.Assets.Textures.Grass.grass_2_png };
        public static string[] FenceContent = new string[] { WaveContent.Assets.Textures.Fence_png };
    }
}
