using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RussBreaker
{
    class CollisionManager
    {
        private Paddle paddle;
        private BallManager balls;
        private BlockManager blockManager;
        private Rectangle topWall;
        private Rectangle rightWall;
        private Rectangle leftWall;

        public CollisionManager(Paddle paddle, BallManager balls, BlockManager blocks)
        {
            this.paddle = paddle;
            this.balls = balls;
            this.blockManager = blocks;
            leftWall = new Rectangle((Game1.gameBounds.X - 25), Game1.gameBounds.Y, 25, Game1.gameBounds.Height);
            rightWall = new Rectangle(Game1.gameBounds.X + Game1.gameBounds.Width, Game1.gameBounds.Y, 25, Game1.gameBounds.Height);
            topWall = new Rectangle(Game1.gameBounds.X, Game1.gameBounds.Y - 25, Game1.gameBounds.Width, 25);
        }

        //types of collisions:
        //ball to wall
        //ball to paddle
        //paddle to wall    HANDLED DIRECTLY IN PADDLE UPDATE FUNCTION

        //ball to wall
        private void checkBallToWall()
        {
            foreach (Ball ball in balls.balls)
            {
                if (ball.ballSprite.IsBoxColliding(leftWall) || ball.ballSprite.IsBoxColliding(rightWall))
                {
                    ball.ballSprite.Velocity = new Vector2(ball.ballSprite.Velocity.X * -1, ball.ballSprite.Velocity.Y);
                    ball.Collided();
                }
                if (ball.ballSprite.IsBoxColliding(topWall))
                {
                    ball.ballSprite.Velocity = new Vector2(ball.ballSprite.Velocity.X, ball.ballSprite.Velocity.Y * -1);
                    ball.Collided();
                }
            }
        }

        private void checkBallToPaddle()
        {
            foreach (Ball ball in balls.balls)
            {
                bool hit = false;
                float xVel = 0.0f;
                if (ball.ballSprite.IsBoxColliding(paddle.paddleSprite1.BoundingBoxRect) || (ball.ballSprite.IsBoxColliding(paddle.paddleSprite2.BoundingBoxRect)))
                {
                    //float hitSpot = (float)(paddle.paddleSprite1.BoundingBoxRect.Right - ball.ballSprite.BoundingBoxRect.Center.X)/paddle.paddleSprite1.BoundingBoxRect.Width;
                    float hitSpot = (float)(ball.ballSprite.BoundingBoxRect.Center.X - paddle.paddleSprite1.BoundingBoxRect.Right);
                    hitSpot = (hitSpot / (paddle.paddleSprite1.BoundingBoxRect.Width + paddle.paddleSprite2.BoundingBoxRect.Width - ball.ballSprite.BoundingBoxRect.Width));
                    xVel = hitSpot*2.5f;
                    hit = true;
                }
               /* else if (ball.ballSprite.IsBoxColliding(paddle.paddleSprite2.BoundingBoxRect))
                {
                    float hitSpot = (ball.ballSprite.BoundingBoxRect.Center.X - paddle.paddleSprite2.BoundingBoxRect.Left) / paddle.paddleSprite2.BoundingBoxRect.Width;
                    xVel = ball.ballSprite.Velocity.X * hitSpot*10.0f;
                    hit = true;
                }*/
                if (hit)
                {
                    ball.ballSprite.Velocity = new Vector2(xVel*100.0f, ball.ballSprite.Velocity.Y * -1);
                    ball.Collided();
                }
            }
        }

        private void checkBallToBlocks()
        {
            foreach (Block block in blockManager.blocks)
            {
                foreach(Ball ball in balls.balls)
                {
                    if (ball.ballSprite.IsBoxColliding(block.blockSprite.BoundingBoxRect))
                    {
                        if (ball.ballSprite.BoundingBoxRect.Bottom > block.blockSprite.BoundingBoxRect.Top)
                        {
                            //top hit
                            ball.ballSprite.Velocity = new Vector2 (ball.ballSprite.Velocity.X,-ball.ballSprite.Velocity.Y);
                        }
                        else if (ball.ballSprite.BoundingBoxRect.Top < block.blockSprite.BoundingBoxRect.Bottom)
                        {
                            ball.ballSprite.Velocity = new Vector2(ball.ballSprite.Velocity.X, -ball.ballSprite.Velocity.Y);
                            //bottom hit
                        }
                        else if (ball.ballSprite.BoundingBoxRect.Left < block.blockSprite.BoundingBoxRect.Right)
                        {
                            ball.ballSprite.Velocity = new Vector2(-ball.ballSprite.Velocity.X, ball.ballSprite.Velocity.Y);
                            //right hit
                        }
                        else if (ball.ballSprite.BoundingBoxRect.Right > block.blockSprite.BoundingBoxRect.Left)
                        {
                            //left hit
                            ball.ballSprite.Velocity = new Vector2(-ball.ballSprite.Velocity.X, ball.ballSprite.Velocity.Y);
                        }
                        block.Collided();
                    }
                }
            }
        }

        public void checkCollisions()
        {
            checkBallToWall();
            checkBallToPaddle();
            checkBallToBlocks();
        }

    }
}
