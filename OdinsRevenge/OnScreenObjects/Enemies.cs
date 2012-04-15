using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Enemies : BaseAnimatedOnScreenObjects
    {
        protected Animation attackAnimation = new Animation();
        protected Animation walkingAnimation = new Animation();

        protected Rectangle enemyHitBox;
        protected Texture2D enemyHitBoxTexture;
        
        protected float attackingScale = 0.8f;
        protected int bw = 2; // Border width
        protected Texture2D borderLine;
        

        public override void Initialize(Texture2D Texture, Vector2 Position, Animation AttackAnimation, Animation WalkingAnimation, OdinLevels LevelController)
        {
            position = Position;
            levelController = LevelController;
            texture = Texture;
            attackAnimation = AttackAnimation;
            walkingAnimation = WalkingAnimation;

            // load the players hitbox
            enemyHitBoxTexture = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            enemyHitBoxTexture.SetData(new Color[] { Color.Transparent });

            borderLine = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            borderLine.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            enemyHitBox = new Rectangle((int)(position.X - 30), (int)(position.Y), (int)((Width - 130) * attackingScale), (int)(Height * attackingScale));

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

        protected void DrawHitBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyHitBoxTexture, enemyHitBox, Color.Green);
            spriteBatch.Draw(borderLine, new Rectangle(enemyHitBox.Left, enemyHitBox.Top, bw, enemyHitBox.Height), Color.Red);
            spriteBatch.Draw(borderLine, new Rectangle(enemyHitBox.Right, enemyHitBox.Top, bw, enemyHitBox.Height), Color.Red);
            spriteBatch.Draw(borderLine, new Rectangle(enemyHitBox.Left, enemyHitBox.Top, enemyHitBox.Width, bw), Color.Red);
            spriteBatch.Draw(borderLine, new Rectangle(enemyHitBox.Left, enemyHitBox.Bottom, enemyHitBox.Width, bw), Color.Red);
        }
     
    }
}
