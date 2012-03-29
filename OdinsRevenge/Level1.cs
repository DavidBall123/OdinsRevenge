using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace OdinsRevenge
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Level1 : Microsoft.Xna.Framework.Game
    {

        #region background & graphic variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D middayLevel1;
        Texture2D sunSetLevel1;
        Texture2D nightLevel1;

        Animation birdAnimation = new Animation();
        Texture2D birdTexture;
        Bird bird;

        Ground ground;

        Sun sun; 
       

        #endregion 

        #region Player variables

        Player player;
        float playerMoveSpeed;
        bool playerFacingRight = true;
        bool walking = false; 
        PlayerAnimation walkingAnimation = new PlayerAnimation();
        Texture2D walkingTexture;
        
        #endregion 


        #region Movment variables

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
       

        #endregion 

        public Level1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            playerMoveSpeed = 4.0f;
            sun = new Sun();
            bird = new Bird();
            ground = new Ground(Content, "Backgrounds\\Level1");
            base.Initialize();

         
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Vector2 playerPostion = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, 435);
            walkingTexture = Content.Load<Texture2D>("Hero\\Walking");
            walkingAnimation.Initialize(walkingTexture, Vector2.Zero, 86, 109, 4, 100, Color.White, 0.8f, true);
            player.Initialize(Content.Load<Texture2D>("Hero\\Hero"), playerPostion, walkingAnimation);
            
            
            sun.Initialize(Content.Load<Texture2D>("Backgrounds\\Sun")); 
            middayLevel1 = Content.Load<Texture2D>("Backgrounds\\Midday");
            sunSetLevel1 = Content.Load<Texture2D>("Backgrounds\\Sunset");
            nightLevel1 = Content.Load<Texture2D>("Backgrounds\\Night");

            Vector2 birdPostion = new Vector2(800, 200);
            birdTexture = Content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(Content.Load<Texture2D>("Backgrounds\\GreyBirdFly"), birdPostion, birdAnimation);

            


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

                       
            currentKeyboardState = Keyboard.GetState();
            
            // Allows the game to exit
            

            UpdatePlayer(gameTime);
            player.Update(gameTime);
            sun.Update(gameTime);
            UpdateBird(); 
            bird.Update(gameTime);
                      

            
            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(middayLevel1, Vector2.Zero, Color.White); 
            ground.Draw(spriteBatch);
            DrawBackground();
            player.Draw(spriteBatch, playerFacingRight, walking);
            sun.Draw(spriteBatch); 
            walking = false;
            bird.Draw(spriteBatch);
            spriteBatch.End(); 

            base.Draw(gameTime);
        }

        /// <summary>
        /// Method handles player input. 
        /// </summary>
        /// <param name="gameTime"></param>

        private void UpdatePlayer(GameTime gameTime)
        {
            #region GamePad
   

            #endregion
             
            #region Keyboard

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.PlayerPosition.X -= playerMoveSpeed;
                if (player.PlayerPosition.X > 0 && player.PlayerPosition.X < 50 || previousKeyboardState.IsKeyDown(Keys.Left))
                {
                    ground.GroundOffset = ground.GroundOffset - 1;
                }
                playerFacingRight = false;
                walking = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) )
            {
                player.PlayerPosition.X += playerMoveSpeed;
                if (player.PlayerPosition.X > 750 && player.PlayerPosition.X < 800 || previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    ground.GroundOffset = ground.GroundOffset + 1;
                }
                playerFacingRight = true;
                walking = true;
            }

            #endregion

            // Make sure that the player does not go out of bounds
            player.PlayerPosition.X = MathHelper.Clamp(player.PlayerPosition.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.PlayerPosition.Y = MathHelper.Clamp(player.PlayerPosition.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
        }

        /// <summary>
        /// If the bird is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the bird somewhere to the right of the screen
        /// </summary>


        private void UpdateBird()
        {
            if (bird.BirdPosition.X >= -50)
            {
                bird.BirdPosition.X = bird.BirdPosition.X - 1;
            }
            else
            {
                Random rand1 = new Random();
                Random rand2 = new Random();

                bird.BirdPosition.X = rand1.Next(900, 1400);
                bird.BirdPosition.Y = rand1.Next(50, 250);
            }
        }

        /// <summary>
        /// method controls what background to draw based on the position of the sun
        /// 
        /// The sun class controls the location of the sun 
        /// </summary>

        private void DrawBackground()
        {
            if (sun.SunHeight == -10)
            {
                spriteBatch.Draw(middayLevel1, Vector2.Zero, Color.White);
            }
            else if (sun.SunHeight > 400)
            {
                spriteBatch.Draw(nightLevel1, Vector2.Zero, Color.White);
            }
            else
            {
                spriteBatch.Draw(sunSetLevel1, Vector2.Zero, Color.White);
            }
            
        }
    }
}


