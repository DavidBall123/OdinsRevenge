using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace OdinsRevenge
{
    class PlayerController
    {

        #region variables and properites 

        PlayerAnimationController playerAnimationController;

        SoundEffect playerAttackingSound;
        SoundEffect heroWalkingSound;
        SoundEffect lightningSound;
        SoundEffect heroDying; 

        int walkingSoundCounter = 0; 
        
        


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
        private int deathCounter;
        private int dyingCounter;
        
        float playerScale = 0.8f;
        float attackingScale = 1.3f; 
        
        private bool playerHit;

        


        // Position of the Player relative to the upper left side of the screen
        public Vector2 PlayerPosition;

        // Vector used for jumps
        Vector2 startingPosition = Vector2.Zero;

        // State of the player
        //bool active;

        PlayerResourceController playerResources;

        internal PlayerResourceController PlayerResources
        {
            get { return playerResources; }
            set { playerResources = value; }
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

        internal PlayerAnimationController PlayerAnimationController
        {
            get { return playerAnimationController; }
            set { playerAnimationController = value; }
        } 
        

#endregion 

        public void Initialize(Vector2 position, Dictionary<string, Texture2D> spellsDict, OdinLevels LevelController)
        {
            deathCounter = 0;
            dyingCounter = 0; 
            playerResources = new PlayerResourceController();
            playerAnimationController = new PlayerAnimationController(); 
            levelController = LevelController; 
            spells = spellsDict; 

            //sounds
            
            playerAttackingSound = LevelController.Content.Load<SoundEffect>("Soundeffects\\PlayerAttack");
            heroWalkingSound = levelController.Content.Load<SoundEffect>("Soundeffects\\heroWalking");
            lightningSound = levelController.Content.Load<SoundEffect>("Soundeffects\\lightning");
            heroDying = levelController.Content.Load<SoundEffect>("Soundeffects\\heroDying"); 

            // Set the starting position of the player around the middle of the screen and to the back
            PlayerPosition = position;

            // Set the player to be active
            //active = true;

            playerHit = false;

            // load the players hitbox
            playerAnimationController.PlayerHitBoxTexture = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            playerAnimationController.PlayerHitBoxTexture.SetData(new Color[] { Color.Transparent });

            playerAnimationController.BorderLine = new Texture2D(levelController.ScreenManager.GraphicsDevice, 1, 1);
            playerAnimationController.BorderLine.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime)
        {
            if (dead == true)
            {
                deathCounter++;
            }

            if (playerResources.Health <= 0)
            {
                if (dying == false)
                {
                    heroDying.Play(); 
                }
                dying = true;
                dyingCounter++;
                if (dyingCounter == 60)
                {
                    dead = true;
                }
            }
            if (dead == false || dying == false)
            {
                if (attacking == true)
                {
                    playerAnimationController.PlayerHitBox = new Rectangle((int)(PlayerPosition.X - 30), (int)(PlayerPosition.Y), (int)(playerAnimationController.Width * attackingScale), (int)(playerAnimationController.Height * attackingScale));
                }
                else
                {
                    playerAnimationController.PlayerHitBox = new Rectangle((int)(PlayerPosition.X - 30), (int)(PlayerPosition.Y), (int)(playerAnimationController.Width * playerScale), (int)(playerAnimationController.Height * playerScale));
                }

                if (playerResources.Energy <= 100)
                {
                    playerResources.EnergyRecharge(); 
                }
                playerAnimationController.Update(gameTime, PlayerPosition);
                Movement();
            }
        }

        private void EndGame()
        {
            throw new NotImplementedException();
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
            playerAnimationController.Draw(spriteBatch); 

            if (dying == true)
            {
                playerAnimationController.DrawDeath(spriteBatch);
            }
            else
            {
                if (playerHit == true)
                {
                    PlayerAttacked();
                    playerAnimationController.DrawStanding(spriteBatch, direction, PlayerPosition);
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
            if (deathCounter >= 60)
            {
                levelController.EndGame(spriteBatch);
            }
        }
       

        private void PlayerAttack(SpriteBatch spriteBatch)
        {
            if (attackCounter < 60)
            {
                playerAnimationController.DrawStriking(spriteBatch, direction);
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
              
                playerAnimationController.DrawSpellCasting(spriteBatch, direction, spells, levelController.Enemey1List, levelController.Enemey2List);
                foreach (Enemy1 e in levelController.Enemey1List)
                {
                    e.Death = true;
                    levelController.Score = levelController.Score + 1;
                }
                foreach (Enemy2 e in levelController.Enemey2List)
                {
                    e.Death = true;
                    levelController.Score = levelController.Score + 1;
                }
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
                    playerAnimationController.DrawStanding(spriteBatch, direction, PlayerPosition);
                    break;
                case PlayerActions.Walking:
                    playerAnimationController.DrawAnimation(spriteBatch, direction);
                    break;
                case PlayerActions.Striking:
                    if (playerResources.Energy >= 100)
                    {
                        playerAttackingSound.Play(); 
                        attacking = true;
                        playerAnimationController.DrawStriking(spriteBatch, direction);
                        playerResources.Energy = 0;
                    }
                    else
                    {
                        playerAnimationController.DrawStanding(spriteBatch, direction, PlayerPosition);
                    }
                    break;
                case PlayerActions.SpellCasting:
                    if (playerResources.Mana > 0)
                    {
                        casting = true;
                        lightningSound.Play(); 
                        playerAnimationController.DrawSpellCasting(spriteBatch, direction, spells, levelController.Enemey1List, levelController.Enemey2List); 
                        playerResources.ReduceMana(); 
                        
                    }
                    break;
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
                PlayWalkingSound();
                levelController.Ground.GroundOffset = levelController.Ground.GroundOffset + 1;
                levelController.Stars.GroundOffset = levelController.Stars.GroundOffset + 1;

                if (levelController.LevelNumber == 2)
                {
                    levelController.Snow.GroundOffset = levelController.Snow.GroundOffset + 1;
                }


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
                PlayWalkingSound();
                levelController.Ground.GroundOffset = levelController.Ground.GroundOffset - 1;
                levelController.Stars.GroundOffset = levelController.Stars.GroundOffset - 1;

                if (levelController.LevelNumber == 2)
                {
                    levelController.Snow.GroundOffset = levelController.Snow.GroundOffset - 1;
                }

                if (jumpInMotion == true && jump != Jumping.Falling)
                {
                    PlayerJump();
                }
            }
            

            
        }

        private void PlayWalkingSound()
        {
            walkingSoundCounter--; 
            if (walkingSoundCounter <= 0)
            {
                walkingSoundCounter = 15;
                heroWalkingSound.Play();
            }
            
        }

    }
    #endregion
}
