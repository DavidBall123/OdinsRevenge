using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class BaseStaticOnScreenObjects 
    {

        protected Texture2D objectTexture;
        protected OdinLevels levelController; 


        // Position of the object relative to the upper left side of the screen
        public Vector2 Position;

        // State of the object
        protected bool Active;

        // Get the width of the object ship
        public int Width
        {
            get { return objectTexture.Width; }
        }

        // Get the height of the object ship
        public int Height
        {
            get { return objectTexture.Height; }
        }

        

        public virtual void Initialize(Texture2D texture, Vector2 position, OdinLevels LevelController)
        {
            objectTexture = texture;
            levelController = LevelController;
            
            // Set the starting position of the object around the middle of the screen and to the back
            Position = position;

            // Set the object to be active
            Active = true;
            
        }


     
        /// <summary>
        /// Base draw class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="objectFacingRight"></param>

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objectTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }



        
    }
}
