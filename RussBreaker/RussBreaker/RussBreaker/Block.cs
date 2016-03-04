using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBreaker
{
    class Block
    {
        public Sprite blockSprite;
        public BlockManager.BlockType blockType;
        private Rectangle initialFrame;
        private int leftBoundPad;
        private int rightBoundPad;
        private int topBoundPad;
        private int bottomBoundPad;
        private bool active;
        private int hp;
        bool destructable;

        public Block(Texture2D texture,
            Vector2 location,
            BlockManager.BlockType bt)
        {
            hp = 1;
            destructable = true;
            this.blockType = bt;
            SetBlockType();
            blockSprite = new Sprite(location, texture, initialFrame, new Vector2(0, 0));
            blockSprite.BoundingXPadLeft = leftBoundPad;
            blockSprite.BoundingXPadRight = rightBoundPad;
            blockSprite.BoundingYPadTop = topBoundPad;
            blockSprite.BoundingYPadBottom = bottomBoundPad;
            active = true;
        }

        private void SetBlockType()
        {
            int xIndex=0;
            int yIndex=0;
            switch (blockType)
            {
                case BlockManager.BlockType.purple:
                    xIndex = 0;
                    yIndex = 0;
                    leftBoundPad = 1;
                    rightBoundPad = 1;
                    topBoundPad = 1;
                    bottomBoundPad = 1;
                    break;
            }
            initialFrame = new Rectangle(xIndex * Game1.spriteWidth + Game1.spritePadding, yIndex * Game1.spriteWidth + Game1.spritePadding, Game1.spriteWidth, Game1.spriteWidth / 2);


        }

        public bool IsActive()
        {
            if (active) return true;
            return false;
        }

        public void Collided()
        {
            hp--;
            if((hp < 1) && (destructable)) active = false;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive()) blockSprite.Draw(spriteBatch);
        }

    }
}
