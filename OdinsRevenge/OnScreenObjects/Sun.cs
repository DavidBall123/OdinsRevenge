using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Sun
    {
        Texture2D sunTexture;
        Vector2 sunPosition;

        public float SunHeight
        {
            get { return sunPosition.Y; } 
        }

        public void Initialize(Texture2D sun)
        {
            sunTexture = sun; 
            sunPosition.X = -100;
            sunPosition.Y = 450; 
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds < 20 && sunPosition.Y > -10)
            {
                sunPosition.X = sunPosition.X + 1;
                sunPosition.Y = sunPosition.Y + -1;
            }
            else if (gameTime.TotalGameTime.Seconds > 30)
            {
                sunPosition.X = sunPosition.X + 1;
                sunPosition.Y = sunPosition.Y + +1;
            }

            if (gameTime.TotalGameTime.Seconds > 55)
            {
                sunPosition.X = -100;
                sunPosition.Y = 450; 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sunTexture, sunPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
        
    }
}
