using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Bird
    {

        // Texture represetning a player standing still 
        Texture2D birdTexture;
        Animation birdAnimation; 

        // Position of the Player relative to the upper left side of the screen
        public Vector2 BirdPosition;

        // State of the player
        bool active;

        // Amount of hit points that player has
        

        // Get the width of the player 
        public int Width
        {
            get { return birdTexture.Width; }
        }

        // Get the height of the player
        public int Height
        {
            get { return birdTexture.Height; }
        }

        // Get the width of the player 
        public int WalkingAnimationWidth
        {
            get { return birdAnimation.FrameWidth; }
        }

        // Get the height of the player
        public int WalkingAnimationHeight
        {
            get { return birdAnimation.FrameHeight; }
        }

    

        public void Initialize(Texture2D texture, Vector2 position, Animation birdAnimate)
        {
            birdTexture = texture;
            birdAnimation = birdAnimate; 
                        
            BirdPosition = position;

            // Set the player to be active
            active = true;

            // Set the player health
            
        }

        public void Update(GameTime gameTime)
        {
            birdAnimation.Position = BirdPosition;
            birdAnimation.Update(gameTime);
        }

        /// <summary>
        /// Draws the player
        /// 
        /// Note the switch between a sprite sheet for walking and a stationary image for standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="playerFacingRight"></param>

        public void Draw(SpriteBatch spriteBatch)
        {   
                birdAnimation.Draw(spriteBatch);

        }
    }
}
