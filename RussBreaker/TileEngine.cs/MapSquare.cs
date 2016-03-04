using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine.cs
{
    [Serializable]
    public class MapSquare
    {
        public int[] LayerTiles = new int[3];
        public string CodeValue = "";
        public bool Destructable = true;

        public MapSquare(int background, int interactive, int foreground, string code, bool destructable)
        {
            LayerTiles[0] = background;
            LayerTiles[1] = interactive;
            LayerTiles[2] = foreground;
            CodeValue = code;
            Destructable = destructable;
        }

        public void ToggleDestructable()
        {
            Destructable = !Destructable;
        }
    }

}
