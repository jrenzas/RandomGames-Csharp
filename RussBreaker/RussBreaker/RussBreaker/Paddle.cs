using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RussBreaker
{
    class Paddle
    {
        public Sprite paddleSprite1;
        public Sprite paddleSprite2;
        private int paddlesRemaining;
        public enum PaddleType { Normal, Small, Big };
        private Rectangle spriteSheetLoc;

        public Paddle(
            Texture2D texture,
            Vector2 location, PaddleType pt, int startingPaddleCount)
        {
            //assumes paddlesprite2 is right next door
            if (pt == PaddleType.Normal) spriteSheetLoc = new Rectangle(225, 161, Game1.spriteWidth, Game1.spriteWidth); //HARD CODED LOCATION OF NORMAL PADDLE
            else if (pt == PaddleType.Big) spriteSheetLoc = new Rectangle(257, 129, Game1.spriteWidth, Game1.spriteWidth); //HARD CODED LOCATION OF BIG PADDLE

            paddleSprite1 = new Sprite(location, texture, spriteSheetLoc, Vector2.Zero);
            location.X += (float)spriteSheetLoc.Width;
            spriteSheetLoc.X += spriteSheetLoc.Width + Game1.spritePadding;
            paddleSprite2 = new Sprite(location, texture, spriteSheetLoc, Vector2.Zero);

            paddlesRemaining = startingPaddleCount;
            SetPaddleType(pt);
        }

        //Contains spritesheet-specific info on bounding boxes and location on spritesheet
        private void SetPaddleType(PaddleType pt)
        {
            if (pt == PaddleType.Big)
            {
                paddleSprite1.BoundingXPadLeft = 3;
                paddleSprite2.BoundingXPadRight = 3;
            }
            else if (pt == PaddleType.Normal)
            {
                paddleSprite1.BoundingXPadLeft = 9;
                paddleSprite2.BoundingXPadRight = 8;
            }
            paddleSprite1.BoundingYPadTop = 14;
            paddleSprite1.BoundingYPadBottom = 6;
            paddleSprite1.BoundingXPadRight = -1;
            paddleSprite2.BoundingXPadLeft = -1;
            paddleSprite2.BoundingYPadBottom = 6;
            paddleSprite2.BoundingYPadTop = 14;
        }

        public void Update(GameTime gameTime, MouseState ms, Rectangle gameBounds)
        {
            Rectangle trialBox1 = paddleSprite1.BoundingBoxRect;
            trialBox1.X = ms.X-trialBox1.Width;
            Rectangle trialBox2 = paddleSprite2.BoundingBoxRect;
            trialBox2.X = ms.X;
            Vector2 newLoc = paddleSprite1.Location;
            if ((trialBox2.X+trialBox2.Width) > (gameBounds.X + gameBounds.Width))
            {
                newLoc.X = (float)(gameBounds.X + gameBounds.Width - trialBox2.Width - Game1.spriteWidth);
                
            } else if (trialBox1.X < gameBounds.X)
            {
                newLoc.X = (float)(gameBounds.X - paddleSprite1.BoundingXPadLeft);
            } else {
                newLoc.X = ms.X - Game1.spriteWidth;
            }
            paddleSprite1.Location = newLoc;
            newLoc.X += Game1.spriteWidth;
            paddleSprite2.Location = newLoc;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            paddleSprite1.Draw(spriteBatch);
            paddleSprite2.Draw(spriteBatch);
        }
    }
}
