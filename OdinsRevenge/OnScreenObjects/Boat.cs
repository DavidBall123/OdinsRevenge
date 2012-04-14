using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Boat : BaseAnimatedOnScreenObjects
    {

        public override void Update(GameTime gameTime)
        {
            animation.Position = Position;
            animation.Update(gameTime);
            UpdateBoat();
        }

        /// <summary>
        /// If the boat  is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the  somewhere to the right of the screen
        /// </summary>


        private void UpdateBoat()
        {
            if (Position.X <= 950)
            {
                if (levelController.Player.Direction == Direction.Left && levelController.Player.Action == PlayerActions.Walking)
                {
                    position.X = position.X + 1f;
                }
                else if (levelController.Player.Direction == Direction.Right && levelController.Player.Action == PlayerActions.Walking)
                {
                    position.X = position.X + 0.3f;
                }
                else
                {
                    position.X = position.X + 1;
                }
            }
            else
            {
                Random rand1 = new Random();

                position.X = rand1.Next(-1100, -400);

            }
        }
    }
}
