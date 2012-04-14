using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    abstract class BaseAnimatedOnScreenObjects
    {
        
        protected Texture2D texture;
        protected Animation animation;
        protected Vector2 position;
        protected OdinLevels levelController; 

        // Position of the object relative to the upper left side of the screen
        
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        // State of the object
        protected bool active;

        // Amount of hit points that object has


        // Width of the Object
        public int Width
        {
            get { return texture.Width; }
        }

        // Height of the 
        public int Height
        {
            get { return texture.Height; }
        }

        // Width of the object 
        public int AnimationWidth
        {
            get { return animation.FrameWidth; }
        }

        // Get the height of the object
        public int AnimationHeight
        {
            get { return animation.FrameHeight; }
        }



        public virtual void Initialize(Texture2D Texture, Vector2 position, Animation animate, OdinLevels LevelController)
        {
            texture = Texture;
            animation = animate;
            levelController = LevelController; 

            Position = position;

            // Set the object to be active
            active = true;

            // Set the object health
        }

        public virtual void Initialize(Texture2D Texture, Vector2 postion, Animation AttackAnimation, Animation WalkingAnimation, OdinLevels LevelController) 
        {
        }

        

        public virtual void Update(GameTime gameTime)
        {
            animation.Position = Position;
            animation.Update(gameTime);
        }

        public virtual void Update(GameTime gameTime, bool day)
        {
        }

        /// <summary>
        /// Base draw class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="objectFacingRight"></param>

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);

        }
    }
}
