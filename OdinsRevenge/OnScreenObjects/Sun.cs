using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Sun : BaseStaticOnScreenObjects
    {
       
        public float SunHeight
        {
            get { return Position.Y; } 
        }

        public void Initialize(Texture2D sun)
        {
            objectTexture = sun; 
            Position.X = -100;
            Position.Y = 450; 
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds < 20 && Position.Y > -10)
            {
                Position.X = Position.X + 1;
                Position.Y = Position.Y + -1;
            }
            else if (gameTime.TotalGameTime.Seconds > 30)
            {
                Position.X = Position.X + 1;
                Position.Y = Position.Y + +1;
            }

            if (gameTime.TotalGameTime.Seconds > 55)
            {
                Position.X = -100;
                Position.Y = 450; 
            }
        }

       
        
    }
}
