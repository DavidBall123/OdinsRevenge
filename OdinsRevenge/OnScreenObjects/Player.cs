using System;
using System.Collections.Generic; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OdinsRevenge
{
    class Player
    {

        #region variables and properites 

        private const int MANA_REDUCTION = 20; // the amount of mana that should be reduced each time a spell is cast.



        private Texture2D playerTexture;
        private PlayerAnimation walkingAnimation;
        private PlayerAnimation strikingAnimation;
        private PlayerAnimation spellCastingAnimation;
        private PlayerAnimation deathAnimation; 
        private Direction direction;
        private PlayerActions action;
        private bool jumpInMotion;
        private Jumping jump = new Jumping();
        private OdinLevels levelController; 
        private Dictionary<string, Texture2D> spells;
        private bool attacking;
        private bool casting;
        private bool dying = false;
        private bool dead = false; 

       
        private int attackCounter;
        private int spellCounter; 
        private Rectangle playerHitBox;
        private Texture2D playerHitBoxTexture;
        float playerScale = 0.8f;
        float attackingScale = 1.3f; 
        private int bw = 2; // Border width
        Texture2D borderLine;
        private bool playerHit;

       

        // Position of the Player relative to the upper left side of the screen
        public Vector2 PlayerPosition;

        // Vector used for jumps
        Vector2 startingPosition = Vector2.Zero;

        // State of the player
        //bool active;

        // Amount of hit points that player has
        private int health;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        private int mana;

        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        private double energy;

        public double Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        // Gets the hitbox

        public Rectangle HitBox
        {
            get { return playerHitBox; }
        }

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


        // Stores the direction the player is facing 
        public Direction Direction
        {
            set { direction = value; }
            get { return direction; }
        }

        //Stores the current action the player is performing
        public PlayerActions Action
        {
            set { action = value; }
            get { return action; }
        }

        public bool JumpInMotion
        {
            set { jumpInMotion = value; }
        }

        public Jumping Jump
        {
            set { jump = value; } 
        }

        public bool PlayerHit
        {
            get { return playerHit; }
            set { playerHit = value; }
        }

        public bool Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        public bool Casting
        {
            get { return casting; }
            set { casting = value; }
        }
        

#endregion 

        public void Initialize(Texture2D texture, Vector2 position, PlayerAnimation walkingAnimate, PlayerAnimation strikingAnimate, PlayerAnimation spellAnimate, PlayerAnimation deathAnimate, Dictionary<string, Texture2D> spellsDict, OdinLevels LevelController)
        {
            playerTexture = texture;
            walkingAnimation = walkingAnimate;
            strikingAnimation = strikingAnimate;
            spellCastingAnimation = spellAnimate;
            deathAnimation = deathAnimate;
            levelController = LevelController; 
            spells = spellsDict; 

            // Set the starting position of the player around the middle of the screen and to the back
            PlayerPosition = position;

            // Set the player to be active
            //active = true;

            // Set the player attributes
            health = 10;
            playerHit = false; 
            mana = 100; 
            energy = 100;

            // load the players hitbox
            playerHitBoxTexture = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            playerHitBoxTexture.SetData(new Color[] { Color.Transparent });

            borderLine = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            borderLine.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                dying = true; 
            }
            if (dead == false || dying == false)
            {
                if (attacking == true)
                {
                    playerHitBox = new Rectangle((int)(PlayerPosition.X - 30), (int)(PlayerPosition.Y), (int)(Width * attackingScale), (int)(Height * attackingScale));
                }
                else
                {
                    playerHitBox = new Rectangle((int)(PlayerPosition.X - 30), (int)(PlayerPosition.Y), (int)(Width * playerScale), (int)(Height * playerScale));
                }

                if (energy <= 100)
                {
                    energy = energy + 0.4;
                }
                walkingAnimation.Position = PlayerPosition;
                walkingAnimation.Update(gameTime);
                strikingAnimation.Position = PlayerPosition;
                strikingAnimation.Update(gameTime);
                deathAnimation.Position = PlayerPosition;
                deathAnimation.Update(gameTime);
                spellCastingAnimation.Position = PlayerPosition;
                spellCastingAnimation.Update(gameTime);
                Movement();
            }
        }

        #region Drawing methods

        /// <summary>
        /// Draws the player
        /// 
        /// Note the switch between a sprite sheet for walking and a stationary image for standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="playerFacingRight"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerHitBoxTexture, playerHitBox, Color.Green);
            spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Top, bw, playerHitBox.Height), Color.Purple);
            spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Right, playerHitBox.Top, bw, playerHitBox.Height), Color.Purple);
            spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Top, playerHitBox.Width, bw), Color.Purple);
            spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Bottom, playerHitBox.Width, bw), Color.Purple);

            if (dying == true)
            {
                DrawDeath(spriteBatch);
            }
            else
            {
                if (playerHit == true)
                {
                    PlayerAttacked();
                    DrawStanding(spriteBatch, direction);
                }
                else
                {
                    if (attacking == false && casting == false)
                    {
                        CheckPlayerAction(spriteBatch);
                    }
                    else if (attacking == true)
                    {
                        PlayerAttack(spriteBatch);
                    }
                    else if (casting == true)
                    {
                        PlayerCasting(spriteBatch);
                    }
                }
            }
               

        }

        private void DrawDeath(SpriteBatch spriteBatch)
        {
            deathAnimation.Draw(spriteBatch); 
        }

        private void PlayerAttack(SpriteBatch spriteBatch)
        {
            if (attackCounter < 60)
            {
                DrawStriking(spriteBatch);
                attackCounter++;
            }
            else
            {
                attackCounter = 0;
                attacking = false;
            }
        }

         private void PlayerCasting(SpriteBatch spriteBatch)
        {
            if (spellCounter < 30)
            {
                DrawSpellCasting(spriteBatch); 
                spellCounter++;
            }
            else
            {
                spellCounter = 0;
                casting = false;
            }
        }

           

        private void CheckPlayerAction(SpriteBatch spriteBatch)
        {
            switch (action)
            {
                case PlayerActions.Standing:
                    DrawStanding(spriteBatch, direction);
                    break;
                case PlayerActions.Walking:
                    DrawAnimation(spriteBatch);
                    break;
                case PlayerActions.Striking:
                    if (energy >= 100)
                    {
                        attacking = true;
                        DrawStriking(spriteBatch);
                        energy = 0;
                    }
                    else
                    {
                        DrawStanding(spriteBatch, direction);
                    }
                    break;
                case PlayerActions.SpellCasting:
                    if (mana > 0)
                    {
                        casting = true;
                        DrawSpellCasting(spriteBatch);
                        mana = mana - MANA_REDUCTION; 
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the player casting a spell
        /// </summary>
        /// <param name="spriteBatch"></param>

        private void DrawSpellCasting(SpriteBatch spriteBatch)
        {
        

            spellCastingAnimation.Draw(spriteBatch, direction);
            spriteBatch.Draw(spells["Power of Thor"], new Vector2(0, 0), Color.White); 
            

        }

        /// <summary>
        /// Draws the striking animation
        /// </summary>
        /// <param name="spriteBatch"></param>

        private void DrawStriking(SpriteBatch spriteBatch)
        {
            strikingAnimation.Draw(spriteBatch, direction);
        }

        /// <summary>
        /// Draws the player walking 
        /// </summary>
        /// <param name="spriteBatch"></param>

        private void DrawAnimation(SpriteBatch spriteBatch)
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


        #endregion


        #region Movement methods

        /// <summary>
        /// controls the movement on the player
        /// </summary>

        private void Movement()
        {
            // check to see if the player is on solid ground
            // if he is not then place him into a falling state

            if (jumpInMotion == false)
            {
                if (PlayerPosition.Y != levelController.GroundLevel)
                {

                    jump = Jumping.Falling;
                }
            }

            if (jump == Jumping.Falling)
            {
                Falling();
            }

            if (jumpInMotion == true && jump != Jumping.Falling)
            {
                PlayerJump();
            }

            switch (direction)
            {
                case Direction.Left:
                    MoveLeft();
                    break;
                case Direction.Right:
                    MoveRight();
                    break;
            }
        }

        /// <summary>
        /// When the player is not at ground level and not jumping
        /// he is brought back down to ground level. 
        /// </summary>

        private void Falling()
        {

            if (PlayerPosition.Y <= levelController.GroundLevel)
            {
                PlayerPosition.Y = PlayerPosition.Y + 1;
            }
            else
            {
                PlayerPosition.Y = levelController.GroundLevel;
                jumpInMotion = false;
                jump = Jumping.Stationary;
                action = PlayerActions.Standing;

            }
        }

        /// <summary>
        /// When a jump commences a player moves upwards until he hits
        /// the predefined jump height (roofHeight). When he reaches this height
        /// his jumping action is turned to falling. 
        /// </summary>

        private void PlayerJump()
        {

            if (PlayerPosition.Y > levelController.RoofHeight)
            {
                PlayerPosition.Y = PlayerPosition.Y - 2;
            }
            else
            {
                jump = Jumping.Falling;
            }
        }

        private void PlayerAttacked()
        {
            if (PlayerPosition.Y > levelController.RoofHeight)
            {
                PlayerPosition.Y = PlayerPosition.Y - 2;
            }
            else
            {
                jump = Jumping.Falling;
            }
            levelController.Ground.GroundOffset = levelController.Ground.GroundOffset - 5;

            //if (direction == Direction.Left)
            //{
            //    levelController.Ground.GroundOffset = levelController.Ground.GroundOffset - 5;
            //}
            //else
            //{
            //    levelController.Ground.GroundOffset = levelController.Ground.GroundOffset + 5;
            //}
        }

        private void MoveRight()
        {
            if (levelController.PreviousKeyboardState.IsKeyDown(Keys.Right))
            {
                levelController.Ground.GroundOffset = levelController.Ground.GroundOffset + 1;
                levelController.Stars.GroundOffset = levelController.Stars.GroundOffset + 1;
                 
                
                if (jumpInMotion == true && jump != Jumping.Falling)
                {
                    PlayerJump();
                }
            }
            
        }
        /// <summary>
        /// moves the player to the left of the screen 
        /// </summary>

        private void MoveLeft()
        {

            if (levelController.PreviousKeyboardState.IsKeyDown(Keys.Left))
            {
                levelController.Ground.GroundOffset = levelController.Ground.GroundOffset - 1;
                levelController.Stars.GroundOffset = levelController.Stars.GroundOffset - 1;
                if (jumpInMotion == true && jump != Jumping.Falling)
                {
                    PlayerJump();
                }
            }

            
        }

    }
    #endregion
}
