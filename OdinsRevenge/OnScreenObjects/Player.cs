using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Player 
    {
        Texture2D playerTexture;
        PlayerAnimation walkingAnimation;
        PlayerAnimation strikingAnimation;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 PlayerPosition;

        // Vector used for jumps
        Vector2 startingPosition = Vector2.Zero; 

        // State of the player
        bool active;

        // Amount of hit points that player has
        int health;

        // Get the width of the player 
        public int Width
        {
            get { return playerTexture.Width; }
        }

        // Height of the  object
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

       

        public void Initialize(Texture2D texture, Vector2 position, PlayerAnimation walkingAnimate, PlayerAnimation strikingAnimate)
        {
            playerTexture = texture;
            walkingAnimation = walkingAnimate;
            strikingAnimation = strikingAnimate;

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
            strikingAnimation.Position = PlayerPosition;
            strikingAnimation.Update(gameTime); 
        }
        

        /// <summary>
        /// Draws the player
        /// 
        /// Note the switch between a sprite sheet for walking and a stationary image for standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="playerFacingRight"></param>

        public void Draw(SpriteBatch spriteBatch, Direction direction, PlayerActions walking)
        {
            switch (walking)
            {
                case PlayerActions.Standing:
                    DrawStanding(spriteBatch, direction);
                    break;
                case PlayerActions.Walking:
                    DrawAnimation(spriteBatch, direction);
                    break;
                case PlayerActions.Striking:
                    DrawStriking(spriteBatch, direction);
                    break; 
                    
            }

        }

        private void DrawStriking(SpriteBatch spriteBatch, Direction direction)
        {
            strikingAnimation.Draw(spriteBatch, direction); 
        }

        private void DrawAnimation(SpriteBatch spriteBatch, Direction direction)
        {
            walkingAnimation.Draw(spriteBatch, direction);
        }

        

        /// <summary>
        /// The draw method for when the player is standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="direction"></param>

        private void DrawStanding(SpriteBatch spriteBatch, Direction direction)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}
