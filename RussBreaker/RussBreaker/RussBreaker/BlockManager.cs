using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBreaker
{
    class BlockManager
    {
        private Texture2D texture;
        public List<Block> blocks = new List<Block>();
        public bool blocksRemaining;
        public enum BlockType { purple, red };

        public BlockManager(Texture2D spriteSheet)
        {
            texture = spriteSheet;
        }

        public void AddBlocks()
        {
            Random rand = new Random();
            int max = rand.Next(50,200);
            for(int i = 0; i < max; i++)
            {
                blocks.Add(new Block(texture, new Vector2(rand.Next(10,1000), rand.Next(10,600)), BlockType.purple));
            }
            blocksRemaining = true;
        }

        public void Update(GameTime gameTime)
        {
            for (int x = blocks.Count - 1; x >= 0; x--)
            {
                blocks[x].Update(gameTime);
                if (!(blocks[x].IsActive())) blocks.RemoveAt(x);
                if (blocks.Count == 0) blocksRemaining = false;
            }
            if (blocks.Count < 11) AddBlocks();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Block block in blocks) block.Draw(spriteBatch);
        }
    }
}
