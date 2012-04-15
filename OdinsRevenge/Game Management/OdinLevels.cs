using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OdinsRevenge
{
    abstract class  OdinLevels : GameScreen
    {
        #region Player variables

        protected Player player = new Player();

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

        protected int playerHit = 0; 
        
        #endregion

        #region background & graphic variables

        protected ContentManager content;
        protected SpriteFont gameFont;

        protected Vector2 position = new Vector2(800, 200);

        protected bool day;
        protected bool night;
        protected Sun sun = new Sun();

        protected BackGround ground;
        protected BackGround stars;

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

        private KeyboardState previousKeyBoardState; 

        #endregion

        #region Properties

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

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");

            sun.Initialize(content.Load<Texture2D>("Backgrounds\\Sun"));

            middayLevel1 = content.Load<Texture2D>("Backgrounds\\Midday");
            sunSetLevel1 = content.Load<Texture2D>("Backgrounds\\Sunset");
            nightLevel1 = content.Load<Texture2D>("Backgrounds\\Night");

            ground = new BackGround(content, "Backgrounds\\Level1");
            stars = new BackGround(content, "Backgrounds\\Stars");

            playerPostion = new Vector2(300, 435);

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

            LevelSpecificContent(); 

            Vector2 healthBarPosition = new Vector2(20,20);
            healthBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\HealthBar"), healthBarPosition);

            Vector2 manaBarPosition = new Vector2(20, 60);
            manaBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\ManaBar"), manaBarPosition);

            Vector2 eneryBarPosition = new Vector2(20, 100);
            energyBar.Initialize(content.Load<Texture2D>("Hero\\Bar"), content.Load<Texture2D>("Hero\\EnergyBar"), manaBarPosition);
            

            player.Direction = Direction.Right;
            player.Action = PlayerActions.Standing;

            LoadClouds();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
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

            if (input.IsPauseGame(ControllingPlayer))
            {
                // ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // add my keyboard stuff here 
                if (currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    player.Direction = Direction.Left;
                    player.Action = PlayerActions.Walking;
                    

                }
                else if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    player.Direction = Direction.Right;
                    player.Action = PlayerActions.Walking;
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

    }
#endregion
}
