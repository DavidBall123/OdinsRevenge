using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Enemies : BaseAnimatedOnScreenObjects
    {
        protected Animation attackAnimation = new Animation();
        protected Animation walkingAnimation = new Animation();
        protected Animation deathAnimation = new Animation(); 

        protected Rectangle enemyHitBox;
        protected Texture2D enemyHitBoxTexture;
        
        protected float attackingScale = 0.8f;
        protected int bw = 2; // Border width
        protected Texture2D borderLine;
        private int health;

        private bool death;
        private bool dying;
        private bool attacking;

        public bool Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        public bool Dying
        {
            get { return dying; }
            set { dying = value; }
        } 

        public bool Death
        {
            get { return death; }
            set { death = value; }
        } 


        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // Gets the hitbox

        public Rectangle HitBox
        {
            get { return enemyHitBox; }
        }
        

        public override void Initialize(Texture2D Texture, Vector2 Position, Animation AttackAnimation, Animation WalkingAnimation, Animation DeathAnimation, OdinLevels LevelController)
        {
            position = Position;
            levelController = LevelController;
            texture = Texture;
            attackAnimation = AttackAnimation;
            walkingAnimation = WalkingAnimation;
            deathAnimation = DeathAnimation; 

            // load the players hitbox
            enemyHitBoxTexture = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            enemyHitBoxTexture.SetData(new Color[] { Color.Transparent });

            borderLine = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            borderLine.SetData(new[] { Color.White });
            health = 100;
        }

        public override void Update(GameTime gameTime)
        {
            enemyHitBox = new Rectangle((int)(position.X - 30), (int)(position.Y), (int)((Width - 130) * attackingScale), (int)(Height * attackingScale));

            attackAnimation.Position = position;
            attackAnimation.Update(gameTime);

            walkingAnimation.Position = position;
            walkingAnimation.Update(gameTime);

            if (dying == false)
            {
                deathAnimation.Position = position;
            }
            deathAnimation.Update(gameTime); 

            UpdateEnemy();
        }

        protected virtual void UpdateEnemy()
        {

            if (levelController.Player.Direction == Direction.Left && levelController.Player.Action == PlayerActions.Walking || levelController.Player.Direction == Direction.Right && levelController.Player.PlayerHit == true)
            {
                if (levelController.Player.PlayerHit == true)
                {
                    position.X = position.X + 1;
                }
                else
                {
                    position.X = position.X + 0.1f;
                }
            }
            else if (levelController.Player.Direction == Direction.Right && levelController.Player.Action == PlayerActions.Walking || levelController.Player.Direction == Direction.Left && levelController.Player.PlayerHit == true)
            {
                if (levelController.Player.PlayerHit == true)
                {
                    position.X = position.X - 2.0f;
                }
                else
                {
                    position.X = position.X - 1.3f;
                }
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
