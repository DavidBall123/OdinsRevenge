using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace OdinsRevenge
{
    abstract class  OdinLevels : GameScreen
    {

        #region constants

        

        #endregion
        
        #region Player variables

        protected PlayerController player = new PlayerController();

        protected PlayerAnimation walkingAnimation = new PlayerAnimation();
        protected PlayerAnimation strikingAnimation = new PlayerAnimation();
        protected PlayerAnimation spellCastingAnimation = new PlayerAnimation();
        protected PlayerAnimation deathAnimation = new PlayerAnimation();
        
        protected Texture2D walkingTexture;
        protected Texture2D strikingTexture;
        protected Texture2D spellCastingTexture;
        protected Texture2D deathTexture; 

        protected Texture2D powerOfThorTexture; 
        protected Dictionary<string, Texture2D> spells = new Dictionary<string,Texture2D>();

        protected HealthBar healthBar = new HealthBar();
        protected ManaBar manaBar = new ManaBar();
        protected EnergyBar energyBar = new EnergyBar(); 

        protected Vector2 playerPostion;

        
        #endregion

        #region Level Control variables

        protected int playerHit = 0;
        private int gameOverCounter = 0;

        protected int distanceToTravel = 1000;

        private int score;

        

        protected int levelEndCounter;
        private int levelNumber;

   

        #endregion

        #region background & graphic variables

        protected  ContentManager content;

     
        protected SpriteFont gameFont;
        protected SpriteFont statsFont; 


        protected Vector2 position = new Vector2(800, 200);

        protected bool day;
        protected bool night;
        protected Sun sun = new Sun();

        protected BackGround ground;
        protected BackGround stars;
        private BackGround snow;

        protected Cloud cloud1 = new Cloud();
        protected Cloud cloud2 = new Cloud();
        protected Cloud cloud3 = new Cloud();
        protected List<BaseStaticOnScreenObjects> cloudList = new List<BaseStaticOnScreenObjects>();


        protected Random random = new Random();
        

        protected float pauseAlpha;

        private float groundLevel = 435;
        private float roofHeight = 250;

        protected Texture2D middayLevel1;
        protected Texture2D sunSetLevel1;
        protected Texture2D nightLevel1;
        protected Texture2D smoke; 

        private KeyboardState previousKeyBoardState; 

        #endregion

        #region EnemyVariables

        protected int enemy1Spawner;
        protected Enemy1 enemy1;
        protected Random enemy1Timer = new Random();
        protected int next1Spawn;

        protected List<Enemy1> enemey1List = new List<Enemy1>();

        protected int enemy2Spawner;
        protected Enemy2 enemy2;
        protected Random enemy2Timer = new Random();
        protected int next2Spawn;


        protected List<Enemy2> enemey2List = new List<Enemy2>();


        #endregion

        #region Properties

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        } 

        public int LevelNumber
        {
            get { return levelNumber; }
            set { levelNumber = value; }
        }

        public BackGround Snow
        {
            get { return snow; }
            set { snow = value; }
        }

        public float GroundLevel
        {
            get { return groundLevel; } 
        }

        public float RoofHeight
        {
            get { return roofHeight; } 
        }

        public BackGround Ground
        {
            get { return ground; }
            set { ground = value; }
        }

        public BackGround Stars
        {
            get { return ground; } 
            set { stars = value; }
        }

        public KeyboardState PreviousKeyboardState
        {
            get { return previousKeyBoardState; }
        }

        public PlayerController Player
        {
            get { return player; }
            set { player = value; }
        }

        internal List<Enemy1> Enemey1List
        {
            get { return enemey1List; }
            set { enemey1List = value; }
        }

        internal List<Enemy2> Enemey2List
        {
            get { return enemey2List; }
            set { enemey2List = value; }
        }
        
        #endregion

        #region Load Content Methods

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");
            statsFont = content.Load<SpriteFont>("statsFont"); 
            smoke = content.Load<Texture2D>("Cloud8");
            LoadBackGrounds();
            LoadAnimations();

            playerPostion = new Vector2(300, 435);

            player.Initialize(playerPostion, spells, this);
            player.PlayerAnimationController.Intialize(content.Load<Texture2D>("Hero\\Hero"), walkingAnimation, strikingAnimation, spellCastingAnimation, deathAnimation, smoke);
            LevelSpecificContent();

            LoadStatusBars();
            player.Direction = Direction.Right;
            player.Action = PlayerActions.Standing;

            LoadClouds();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        private void LoadStatusBars()
        {
            Vector2 healthBarPosition = new Vector2(20, 20);
            healthBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\HealthBar"), healthBarPosition);

            Vector2 manaBarPosition = new Vector2(20, 60);
            manaBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\ManaBar"), manaBarPosition);

            Vector2 eneryBarPosition = new Vector2(20, 100);
            energyBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\EnergyBar"), manaBarPosition);
        }

        private void LoadAnimations()
        {
            walkingTexture = content.Load<Texture2D>("Hero\\Walking");
            strikingTexture = content.Load<Texture2D>("Hero\\HeroStriking");
            spellCastingTexture = content.Load<Texture2D>("Hero\\HeroSpellCasting");
            deathTexture = content.Load<Texture2D>("Hero\\HeroDeath");
            powerOfThorTexture = content.Load<Texture2D>("Spells\\LightningSix");
            spells.Add("Power of Thor", powerOfThorTexture);

            walkingAnimation.Initialize(walkingTexture, Vector2.Zero, 86, 109, 4, 100, Color.White, 0.8f, true);
            strikingAnimation.Initialize(strikingTexture, Vector2.Zero, 150, 150, 6, 100, Color.White, 0.8f, true);
            spellCastingAnimation.Initialize(spellCastingTexture, Vector2.Zero, 85, 131, 2, 250, Color.White, 0.8f, true);
            deathAnimation.Initialize(deathTexture, Vector2.Zero, 91, 100, 5, 250, Color.White, 0.8f, true);
        }

        private void LoadBackGrounds()
        {
            sun.Initialize(content.Load<Texture2D>("Backgrounds\\Sun"));
            middayLevel1 = content.Load<Texture2D>("Backgrounds\\Midday");
            sunSetLevel1 = content.Load<Texture2D>("Backgrounds\\Sunset");
            nightLevel1 = content.Load<Texture2D>("Backgrounds\\Night");
            ground = new BackGround(content, "Backgrounds\\Level1");
            stars = new BackGround(content, "Backgrounds\\Stars");
        }

        protected abstract void LevelSpecificContent(); 
        

        /// <summary>
        /// Methods loads the cloud content
        /// </summary>
        

        protected void LoadClouds()
        {
            Vector2 cloudPosition = new Vector2();
            Random rand1 = new Random();

            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300);

            cloud1.Initialize(content.Load<Texture2D>("Backgrounds\\Clouds1"), cloudPosition, this);
            cloudList.Add(cloud1);

            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300);

            cloud2.Initialize(content.Load<Texture2D>("Backgrounds\\Clouds2"), cloudPosition, this);
            cloudList.Add(cloud2);

            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300);

            cloud3.Initialize(content.Load<Texture2D>("Backgrounds\\Clouds3"), cloudPosition, this);
            cloudList.Add(cloud3);

        }

    #endregion

        #region background control methods

        /// <summary>
        /// method controls what background to draw based on the position of the sun
        /// 
        /// The sun class controls the location of the sun 
        /// </summary>

        protected void DrawBackground(SpriteBatch spriteBatch)
        {
            if (sun.SunHeight == -10)
            {
                day = true;
                night = false;
                spriteBatch.Draw(middayLevel1, Vector2.Zero, Color.White);
            }
            else if (sun.SunHeight > 400)
            {
                day = false;
                night = true;
                spriteBatch.Draw(nightLevel1, Vector2.Zero, Color.White);
            }
            else
            {
                day = true;
                night = false;
                spriteBatch.Draw(sunSetLevel1, Vector2.Zero, Color.White);
            }

        }

#endregion

        #region input methods

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState currentKeyboardState = input.CurrentKeyboardStates[playerIndex];


            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!

            if (levelEndCounter < 0)
            {
                LoadingScreen.Load(ScreenManager, true, 0,
                               new Level2(score));
            }

            if (gameOverCounter == 100)
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                          new MainMenuScreen());
            }

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F1))
            {
                   ScreenManager.AddScreen(new HelpScreen(), ControllingPlayer);
            }
            else
            {
                
                // add my keyboard stuff here 
                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    player.Direction = Direction.Left;
                    player.Action = PlayerActions.Walking;
                    distanceToTravel++;
                    

                }
                else if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    player.Direction = Direction.Right;
                    player.Action = PlayerActions.Walking;
                    distanceToTravel--;
                }
                else
                {
                    player.Action = PlayerActions.Standing;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    player.JumpInMotion = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.LeftControl))
                {
                    player.Action = PlayerActions.Striking;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.LeftAlt))
                {
                    player.Action = PlayerActions.SpellCasting;
                }
                previousKeyBoardState = currentKeyboardState; 
            }
        }

#endregion

        #region comman game logic

        public void EndGame(SpriteBatch spriteBatch)
        {
            
            Vector2 textPosition = new Vector2(300,335);
            spriteBatch.DrawString(gameFont, "GAME OVER", textPosition, Color.Red);
            gameOverCounter++;
            
        }

        /// <summary>
        /// Method checks to see if the players hit box & the bugs hit box are intersecting 
        /// </summary>

        protected void DetectCollision()
        {
            for (int i = 0; i < enemey1List.Count; i++)
            {
                if (player.PlayerAnimationController.HitBox.Intersects(enemey1List[i].HitBox))
                {
                    if (player.Attacking == true)
                    {
                        enemey1List[i].Health = 0;
                        
                    }
                    else if (enemey1List[i].Dying == false && enemey1List[i].Attacking == true || enemey1List[i].Death == false && enemey1List[i].Attacking == true)
                    {
                        if (playerHit == 0)
                        {
                            player.PlayerResources.ReduceHealth();
                            playerHit = 80;
                        }
                    }
                }
            }

            for (int i = 0; i < enemey2List.Count; i++)
            {
                if (player.PlayerAnimationController.HitBox.Intersects(enemey2List[i].HitBox))
                {
                    if (player.Attacking == true)
                    {
                        enemey2List[i].Health = 0;
                    }
                    else if (enemey2List[i].Dying == false && enemey2List[i].Attacking == true || enemey2List[i].Death == false && enemey2List[i].Attacking == true)
                    {
                        if (playerHit == 0)
                        {
                            player.PlayerResources.ReduceHealth();
                            playerHit = 80;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Draws the coulds
        /// </summary>

        protected void DrawClouds(SpriteBatch spriteBatch)
        {
            foreach (BaseStaticOnScreenObjects e in cloudList)
            {
                if (e.Position.X >= -150)
                {
                    e.Position.X--;
                    e.Draw(spriteBatch);
                }
                else
                {
                    Random rand1 = new Random();
                    e.Position.X = rand1.Next(800, 2000);
                    e.Position.Y = rand1.Next(0, 300);
                }
            }
        }

        protected void UpdateEnemy1(GameTime gameTime)
        {
            enemy1Spawner++;
            if (enemy1Spawner == next1Spawn)
            {
                FirstEnemy(); 
                enemy1Spawner = 0;
                next1Spawn = enemy1Timer.Next(200, 500);
            }


            foreach (Enemy1 e in enemey1List)
            {
                e.Update(gameTime);
            }

            for (int i = 0; i < enemey1List.Count; i++)
            {
                if (enemey1List[i].Position.X < -200)
                {
                    enemey1List.RemoveAt(i);
                }
                else if (enemey1List[i].Death == true)
                {
                    enemey1List.RemoveAt(i);
                    score++;
                }
            }
        }

        protected abstract void FirstEnemy(); 

        protected void UpdateEnemy2(GameTime gameTime)
        {
            enemy2Spawner++;
            if (enemy2Spawner == next2Spawn)
            {
                SecondEnemy(); 
                enemy2Spawner = 0;
                next2Spawn = enemy2Timer.Next(200, 500);
            }


            foreach (Enemy2 e in enemey2List)
            {
                e.Update(gameTime);
            }

            for (int i = 0; i < enemey2List.Count; i++)
            {
                if (enemey2List[i].Position.X > 900)
                {
                    enemey2List.RemoveAt(i);
                }
                else if (enemey2List[i].Death == true)
                {
                    enemey2List.RemoveAt(i);
                    score++;
                }
            }
        }

        protected abstract void SecondEnemy(); 

    }
#endregion
}
