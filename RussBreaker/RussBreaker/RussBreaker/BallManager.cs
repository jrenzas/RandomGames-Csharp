using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBreaker
{
    class BallManager
    {
        private Texture2D texture;
        public List<Ball> balls = new List<Ball>();
        public bool ballsRemaining;

        public BallManager(Texture2D texture)
        {
            this.texture = texture;
            ballsRemaining = false;
        }

        public void SpawnNormalBall(Vector2 location, Vector2 velocity)
        {
            balls.Add(new Ball(texture,location,velocity,Ball.BallType.Normal));
            ballsRemaining = true;
        }

        public void Update(GameTime gameTime)
        {
            for (int x = balls.Count - 1; x >= 0; x--)
            {
                balls[x].Update(gameTime);
                if (!(balls[x].IsActive())) balls.RemoveAt(x);
                if (balls.Count == 0) ballsRemaining = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ball ball in balls) ball.Draw(spriteBatch);
        }

    }
}
