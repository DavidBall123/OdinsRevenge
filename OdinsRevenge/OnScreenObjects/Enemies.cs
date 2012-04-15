using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Enemies : BaseAnimatedOnScreenObjects
    {
        protected Animation attackAnimation = new Animation();
        protected Animation walkingAnimation = new Animation();
        

        public override void Initialize(Texture2D Texture, Vector2 Position, Animation AttackAnimation, Animation WalkingAnimation, OdinLevels LevelController)
        {
            position = Position;
            levelController = LevelController;
            texture = Texture;
            attackAnimation = AttackAnimation;
            walkingAnimation = WalkingAnimation; 
        }

        public override void Update(GameTime gameTime)
        {

            attackAnimation.Position = position;
            attackAnimation.Update(gameTime);

            walkingAnimation.Position = position;
            walkingAnimation.Update(gameTime);

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
