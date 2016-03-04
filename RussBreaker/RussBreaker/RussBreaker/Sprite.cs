using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBreaker
{
    class Sprite
    {
        public Texture2D Texture;

        //each rectangle in the list is one frame of an animation strip (so iterating through the list animates the sprite)
        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        private int currentFrame;           //which frame within the animation you are on
        private float frameTime = 0.1f;     //how long to display a frame before you move to the next one in the animation
        private float timeForCurrentFrame = 0.0f;   //will be incremented until it hits frameTime, then move to the next frame and reset this to 0.0f

        //for manipulating the sprite
        private Color tintColor = Color.White;
        private float rotation = 0.0f;

        //for collision detection, using both bounding circle and bounding box detection
        public int CollisionRadius = 0;     //for bounding circle
        public int BoundingXPadLeft = 0;    //for bounding box X
        public int BoundingXPadRight = 0;
        public int BoundingYPadTop = 0;    //for bounding box Y
        public int BoundingYPadBottom = 0;

        //location and velocity, acceleration not used here at all but could be interesting
        protected Vector2 location = Vector2.Zero;  //current location, will be incremented by velocity each second
        protected Vector2 velocity = Vector2.Zero;  //how far in x and y that sprite travels in one second of game time

        //constructor of the class, sets it up
        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity)
        {
            this.location = location;
            Texture = texture;  //why not rename it texture and access with this.texture?
            this.velocity = velocity;

            //adds first frame to the list. extracts framewidth and height from the rectangle passed in as input
            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }

        //next group of functions are simple pass-throughs to alter or retrieve info from a sprite
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; } //just keeps the value between 0 and 2pi and throws away any increments of 360 degrees so nothing weird happens
        }

        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public Rectangle Destination
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public Vector2 Center
        {
            get
            {
                return location + new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }

        //supports collision detection
        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle(
                    (int)location.X + BoundingXPadLeft,
                    (int)location.Y + BoundingYPadTop,
                    frameWidth - (BoundingXPadLeft + BoundingXPadRight),
                    frameHeight - (BoundingYPadBottom+BoundingYPadTop));

            }
        }

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius)) return true;
            else return false;
        }
        
        //adds frames to the list
        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }

            location += (velocity * elapsed);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Center,
                Source,
                tintColor,
                rotation,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);
        }


    }
}