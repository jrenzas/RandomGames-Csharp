using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TileEngine.cs
{
    public static class TileMap
    {
        public const int TileWidth = 31;
        public const int TileHeight = 31;
        public const int MapWidth = 25;
        public const int MapHeight = 25;
        public const int MapLayers = 3;
        private const int greenBack = 6;

        static private MapSquare[,] mapCells = new MapSquare[MapWidth, MapHeight];
        public static bool EditorMode = false;
        public static SpriteFont spriteFont;
        static private Texture2D tileSheet;

        static public void Initialize(Texture2D tileTexture)
        {
            tileSheet = tileTexture;
            for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                    for (int z = 0; z < MapLayers; z++)
                        mapCells[x, y] = new MapSquare(greenBack, 0, 0, "", true);
        }

        public static int TilesPerRow
        {
            get { return tileSheet.Width / TileWidth; }
        }

        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                (tileIndex % TilesPerRow) * TileWidth,
                (tileIndex / TilesPerRow) * TileHeight,
                TileWidth, TileHeight);
        }
    }
}
