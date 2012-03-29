using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Player
    {

        // Texture represetning a player standing still 
        Texture2D playerTexture;
        PlayerAnimation walkingAnimation; 

        // Position of the Player relative to the upper left side of the screen
        public Vector2 PlayerPosition;

        // State of the player
        bool active;

        // Amount of hit points that player has
        int health;

        // Get the width of the player 
        public int Width
        {
            get { return playerTexture.Width; }
        }

        // Get the height of the player
        public int Height
        {
            get { return playerTexture.Height; }
        }

        // Get the width of the player 
        public int WalkingAnimationWidth
        {
            get { return walkingAnimation.FrameWidth; }
        }

        // Get the height of the player
        public int WalkingAnimationHeight
        {
            get { return walkingAnimation.FrameHeight; }
        }

    

        public void Initialize(Texture2D texture, Vector2 position, PlayerAnimation walkingAnimate)
        {
            playerTexture = texture;
            walkingAnimation = walkingAnimate; 

            // Set the starting position of the player around the middle of the screen and to the back
            PlayerPosition = position;

            // Set the player to be active
            active = true;

            // Set the player health
            health = 100;
        }

        public void Update(GameTime gameTime)
        {
            walkingAnimation.Position = PlayerPosition;
            walkingAnimation.Update(gameTime);
        }

        /// <summary>
        /// Draws the player
        /// 
        /// Note the switch between a sprite sheet for walking and a stationary image for standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="playerFacingRight"></param>

        public void Draw(SpriteBatch spriteBatch, bool playerFacingRight, bool walking)
        {

            if (walking == false)
            {
                if (playerFacingRight == true)
                {
                     spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            else if (walking == true)
            {
                walkingAnimation.Draw(spriteBatch, playerFacingRight);
            }



        }
    }
}
