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

        Animation boatAnimation = new Animation();
        Texture2D boatTexture; 
        
        BaseAnimatedOnScreenObjects bird;
        BaseAnimatedOnScreenObjects boat; 

        BaseStaticOnScreenObjects cloud1;
        BaseStaticOnScreenObjects cloud2;
        BaseStaticOnScreenObjects cloud3;

        List<BaseAnimatedOnScreenObjects> animatedObjectsList;
        List<BaseStaticOnScreenObjects> staticObjectsList; 
        
        Ground ground;
        Ground ocean1;
        Ground stars; 

        bool day;
        bool night; 

        Sun sun; 

        #endregion 

        #region Player variables

        Player player;
        
        PlayerAnimation walkingAnimation = new PlayerAnimation();
        Texture2D walkingTexture;

        PlayerAnimation strikingAnimation = new PlayerAnimation();
        Texture2D strikingTexture;

        Direction direction = new Direction();
        PlayerActions action = new PlayerActions();
        Jumping jump = new Jumping();
        

        float roofHeight = 350;
        float groundLevel = 435;

        bool jumpInMotion = false; 
        
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
            sun = new Sun();
            bird = new BaseAnimatedOnScreenObjects();
            boat = new BaseAnimatedOnScreenObjects(); 
            cloud1 = new BaseStaticOnScreenObjects();
            cloud2 = new BaseStaticOnScreenObjects();
            cloud3 = new BaseStaticOnScreenObjects(); 
            animatedObjectsList = new List<BaseAnimatedOnScreenObjects>();
            staticObjectsList = new List<BaseStaticOnScreenObjects>(); 
            ground = new Ground(Content, "Backgrounds\\Level1");
            ocean1 = new Ground(Content, "Backgrounds\\Ocean1");
            stars = new Ground(Content, "Backgrounds\\Stars");
            direction = Direction.Right;
            action = PlayerActions.Standing; 
         
            
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
            Vector2 playerPostion = new Vector2(300, 435);
            walkingTexture = Content.Load<Texture2D>("Hero\\Walking");
            strikingTexture = Content.Load<Texture2D>("Hero\\HeroStriking"); 
            walkingAnimation.Initialize(walkingTexture, Vector2.Zero, 86, 109, 4, 100, Color.White, 0.8f, true);
            strikingAnimation.Initialize(strikingTexture, Vector2.Zero, 150, 150, 6, 100, Color.White, 0.8f, true);
            player.Initialize(Content.Load<Texture2D>("Hero\\Hero"), playerPostion, walkingAnimation, strikingAnimation);
            
            
            sun.Initialize(Content.Load<Texture2D>("Backgrounds\\Sun")); 
            middayLevel1 = Content.Load<Texture2D>("Backgrounds\\Midday");
            sunSetLevel1 = Content.Load<Texture2D>("Backgrounds\\Sunset");
            nightLevel1 = Content.Load<Texture2D>("Backgrounds\\Night");

            Vector2 position = new Vector2(800, 200);
            birdTexture = Content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(birdTexture, position, birdAnimation);

            position.X = -150;
            position.Y = 350;

            boatTexture = Content.Load<Texture2D>("Backgrounds\\Boat");
            boatAnimation.Initialize(boatTexture, Vector2.Zero, 63, 69, 4, 100, Color.White, 0.8f, true);
            boat.Initialize(boatTexture, position, boatAnimation);
            
            LoadClouds();

        }

        /// <summary>
        /// Methods loads the cloud content
        /// </summary>

        private void LoadClouds()
        {
            Vector2 cloudPosition = new Vector2(); 
            Random rand1 = new Random();
            
            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300); 

            cloud1.Initialize(Content.Load<Texture2D>("Backgrounds\\Clouds1"), cloudPosition);
            staticObjectsList.Add(cloud1);

            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300);

            cloud2.Initialize(Content.Load<Texture2D>("Backgrounds\\Clouds2"), cloudPosition);
            staticObjectsList.Add(cloud2);

            cloudPosition.X = rand1.Next(800, 2000);
            cloudPosition.Y = rand1.Next(0, 300);

            cloud3.Initialize(Content.Load<Texture2D>("Backgrounds\\Clouds3"), cloudPosition);
            staticObjectsList.Add(cloud3);

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
            currentKeyboardState = Keyboard.GetState();
            Input(gameTime);
            Movement();
            player.Update(gameTime);
            sun.Update(gameTime);
            UpdateBird();
            UpdateBoat(); 
            bird.Update(gameTime);
            boat.Update(gameTime);
            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
                        
            spriteBatch.Begin();
            ground.Draw(spriteBatch);
            ocean1.Draw(spriteBatch);
            DrawBackground();
            player.Draw(spriteBatch, direction, action);
            sun.Draw(spriteBatch);
            bird.Draw(spriteBatch);
            boat.Draw(spriteBatch);
            if (night == true)
            {
                stars.Draw(spriteBatch);
            }
            DrawStaticObjects(spriteBatch);
            spriteBatch.End(); 

            base.Draw(gameTime);
        }

        /// <summary>
        /// Method handles player input. 
        /// </summary>
        /// <param name="gameTime"></param>

        private void Input(GameTime gameTime)
        {
            #region GamePad
   

            #endregion
             
            #region Keyboard
            //
             
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = Direction.Left;
                action = PlayerActions.Walking;
                
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = Direction.Right;
                action = PlayerActions.Walking;
            }
            else
            {
                action = PlayerActions.Standing;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                jumpInMotion = true;
            }

            if (currentKeyboardState.IsKeyDown(Keys.LeftControl))
            {
                action = PlayerActions.Striking;
            }
            
            
            
   
            #endregion

         
        }

        /// <summary>
        /// If the bird  is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the  somewhere to the right of the screen
        /// </summary>


        private void UpdateBird()
        {
            if (bird.Position.X >= -50 && day==true)
            {
                bird.Position.X = bird.Position.X - 1;
            }
            else   
            {
                Random rand1 = new Random();

                bird.Position.X = rand1.Next(900, 1400);
                bird.Position.Y = rand1.Next(50, 250);
            }
        }

        /// <summary>
        /// If the boat  is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the  somewhere to the right of the screen
        /// </summary>


        private void UpdateBoat()
        {
            if (boat.Position.X <= 950)
            {
                boat.Position.X = boat.Position.X + 1;
            }
            else
            {
                Random rand1 = new Random();

                boat.Position.X = rand1.Next(-1100, -400);
                
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
        /// Draws all the objects inside the static object list
        /// </summary>

        private void DrawStaticObjects(SpriteBatch spriteBatch)
        {   
            foreach (BaseStaticOnScreenObjects e in staticObjectsList)
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

        /// <summary>
        /// controls the movement on the player
        /// </summary>

        private void Movement()
        {
            // check to see if the player is on solid ground
            // if he is not then place him into a falling state

            if (jumpInMotion == false)
            {
                if (player.PlayerPosition.Y != groundLevel)
                {

                    jump= Jumping.Falling;
                }
            }
        
            if (jump == Jumping.Falling)
            {
                Falling(); 
            }

            if (jumpInMotion == true && jump != Jumping.Falling)
            {
                Jump();
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

      
        private void MoveRight()
        {
            if (previousKeyboardState.IsKeyDown(Keys.Right))
            {
                ground.GroundOffset = ground.GroundOffset + 5;
                stars.GroundOffset = stars.GroundOffset + 1;
                if (jumpInMotion == true && jump != Jumping.Falling)
                {
                    Jump();
                }
            }
        }
        /// <summary>
        /// moves the player to the left of the screen 
        /// </summary>

        private void MoveLeft()
        {
            
            if (previousKeyboardState.IsKeyDown(Keys.Left))
            {
                ground.GroundOffset = ground.GroundOffset - 5;
                stars.GroundOffset = stars.GroundOffset - 1;
                if (jumpInMotion == true && jump != Jumping.Falling)
                {
                    Jump();
                }

            }
        }

        /// <summary>
        /// Method controls upwards jumps
        /// </summary>

        private void Jump()
        {   
            
            if (player.PlayerPosition.Y > roofHeight)
            {
                player.PlayerPosition.Y = player.PlayerPosition.Y - 5;
            }
            else
            {
                jump = Jumping.Falling;
            }
                      
        }

      

        private void Falling()
        {
            
            if (player.PlayerPosition.Y <= groundLevel)
            {
                player.PlayerPosition.Y = player.PlayerPosition.Y + 10;
            }
            else
            {
                player.PlayerPosition.Y = groundLevel;
                jumpInMotion = false;
                jump = Jumping.Stationary;
                action = PlayerActions.Standing; 

            }
        }
    }
}


