using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBreaker
{
    class Ball
    {
        public Sprite ballSprite;
        private bool active;
        public enum BallType {Small, Normal, Large};
        private BallType ballType;
        private Rectangle initialFrame;
        private int frameCount;
        private float speed = 200f;
        private const float maxSpeed = 300f;

        public Ball(
            Texture2D texture,
            Vector2 location,
            Vector2 velocity, BallType bt)
        {
            this.ballType = bt;
            SetBallType();
            ballSprite = new Sprite(location, texture, initialFrame, velocity);
            SetBoundingBox();

            for (int x = 0; x < frameCount; x++)
            {
                ballSprite.AddFrame(new Rectangle(
                    initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));

                active = true;
            }

        }

        private void SetBallType()
        {
            if (ballType == BallType.Normal)
            {
                initialFrame = new Rectangle(450, 130, Game1.spriteWidth-1, Game1.spriteWidth-1);
                frameCount = 1;
            }
        }

        private void SetBoundingBox()
        {
            if (ballType == BallType.Normal)
            {
                ballSprite.BoundingXPadLeft = 14;
                ballSprite.BoundingXPadRight = 11;
                ballSprite.BoundingYPadBottom = 12;
                ballSprite.BoundingYPadTop = 13;
            }
        }

        public void Collided()
        {
            if(speed<maxSpeed) speed *= 1.03f;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive())
            {
                if ((ballSprite.Location.Y - (float)ballSprite.CollisionRadius) > Game1.gameBounds.Bottom)
                {
                    active = false;
                    return;
                }
                Vector2 vel = ballSprite.Velocity;
                vel.Normalize();
                ballSprite.Velocity = vel*speed;
                ballSprite.Update(gameTime);
            }
        }

        public bool IsActive()
        {
            if (active) return true;
            else return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive()) ballSprite.Draw(spriteBatch);
        }
    }
}
