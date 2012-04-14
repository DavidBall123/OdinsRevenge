using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Enemies : BaseAnimatedOnScreenObjects
    {
        public override void Update(GameTime gameTime)
        {
            animation.Position = Position;
            animation.Update(gameTime);
            UpdateEnemy();
        }

        protected virtual void UpdateEnemy()
        {
            
            if (levelController.Player.Direction == Direction.Left && levelController.Player.Action == PlayerActions.Walking)
            {
                position.X = position.X + 0.1f;
            }
            else if (levelController.Player.Direction == Direction.Right && levelController.Player.Action == PlayerActions.Walking)
            {
                position.X = position.X - 1.3f;
            }
            else
            {
                position.X = position.X - 1;
            }
        }
    }
}
