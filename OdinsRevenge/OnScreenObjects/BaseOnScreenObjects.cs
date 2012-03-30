using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class BaseOnScreenObjects
    {
        // Texture represetning a object standing still 
        protected Texture2D Texture;
        protected Animation Animation;

        // Position of the object relative to the upper left side of the screen
        public Vector2 Position;

        // State of the object
        protected bool active;

        // Amount of hit points that object has


        // Width of the Object
        public int Width
        {
            get { return Texture.Width; }
        }

        // Height of the 
        public int Height
        {
            get { return Texture.Height; }
        }

        // Width of the object 
        public int AnimationWidth
        {
            get { return Animation.FrameWidth; }
        }

        // Get the height of the object
        public int AnimationHeight
        {
            get { return Animation.FrameHeight; }
        }



        public void Initialize(Texture2D texture, Vector2 position, Animation animate)
        {
            Texture = texture;
            Animation = animate;

            Position = position;

            // Set the object to be active
            active = true;

            // Set the object health

        }

        public void Update(GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);
        }

        /// <summary>
        /// Base draw class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="objectFacingRight"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);

        }
    }
}
