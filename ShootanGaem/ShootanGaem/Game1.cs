using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShootanGaem
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle backgroundRect;

        StreamReader level;
        ShootanEngine Engine;

        //background
        Background background;

        //UI objects
        SGButtonContainer mainMenu;
        SGButton startButton;
        SGButton loadButton;
        SGButton DLCButton;
        SGButton quitButton;

        SGButtonContainer levelMenu;
        SGButton level1;
        SGButton level2;
        SGButton level3;
        SGButton backButton;

        enum GameState
        {
            MainMenu,
            InitializeLevel,
            InGame,
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set size of game
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //scrolling background
            background = new Background(Content.Load<Texture2D>(@"backgrounds\bg"), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            background.setScroll("DOWNLEFT");

            //Create engine
            Engine = new ShootanEngine(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Content);

            /* ---------
             * UI Stuff
               --------*/
            //Main Menu
            mainMenu = new SGButtonContainer();

            startButton = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth/2 - 200, -320, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            startButton.setText("New Game");

            loadButton = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth / 2 - 200, -240, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            loadButton.setText("Load level");

            DLCButton = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth / 2 - 200, -160, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            DLCButton.setText("DLC");

            quitButton = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth / 2 - 200, -80, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            quitButton.setText("Quit");

            //Add MainMenu buttons to MainMenu container
            mainMenu.add(startButton);
            mainMenu.add(loadButton);
            mainMenu.add(DLCButton);
            mainMenu.add(quitButton);

            //Set the stopping position for sliding animation and slide it
            mainMenu.setSlideGoal(graphics.PreferredBackBufferWidth / 2 - 200, 400);
            mainMenu.slideDown();

            //Level selection menu
            levelMenu = new SGButtonContainer();

            //Create level selection buttons
            level1 = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth + 400, 400, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            level1.setText("Level One");

            level2 = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth + 400, 480, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            level2.setText("Level Two");

            level3 = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth + 400, 560, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            level3.setText("Level Three");

            backButton = new SGButton(Content.Load<Texture2D>(@"menu\button"), Content.Load<Texture2D>(@"menu\button_down"), new Rectangle(graphics.PreferredBackBufferWidth + 400, 560, 400, 70), Content.Load<SpriteFont>("MenuFont"));
            backButton.setText("Back");

            //Add level selection buttons to container
            levelMenu.add(level1);
            levelMenu.add(level2);
            levelMenu.add(level3);
            levelMenu.add(backButton);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    //Update menus/buttons/background
                    mainMenu.update(Mouse.GetState());
                    levelMenu.update(Mouse.GetState());
                    background.update(gameTime);

                    //Handle menu clicks
                    if (startButton.clicked)
                    {
                        level = new StreamReader(@"levels\level1.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        startButton.clicked = false;
                    }
                    if (loadButton.clicked)
                    {
                        mainMenu.setSlideGoal(-400, 380);
                        mainMenu.slideLeft();
                        
                        levelMenu.setSlideGoal(graphics.PreferredBackBufferWidth / 2 - 200, 380);
                        levelMenu.slideLeft();

                        loadButton.clicked = false;
                    }
                    if (level1.clicked)
                    {
                        level = new StreamReader(@"levels\level1.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        level1.clicked = false;
                    }
                    if (level2.clicked)
                    {
                        level = new StreamReader(@"levels\level2.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        level2.clicked = false;
                    }
                    if (level3.clicked)
                    {
                        level = new StreamReader(@"levels\level3.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        level3.clicked = false;
                    }
                    if (backButton.clicked)
                    {
                        levelMenu.setSlideGoal(graphics.PreferredBackBufferWidth+400, 380);
                        levelMenu.slideRight();

                        mainMenu.setSlideGoal(graphics.PreferredBackBufferWidth / 2 - 200, 380);
                        mainMenu.slideRight();

                        backButton.clicked = false;
                    }

                    break;
                case GameState.InitializeLevel:
                    Engine.createPlayer(Content.Load<Texture2D>(@"sprites\player_default"), new Vector2((graphics.PreferredBackBufferWidth / 2) - 30, (graphics.PreferredBackBufferHeight - 120)));
                    Engine.addPlayerBullets(Content.Load<Texture2D>(@"sprites\round_bullet"), 100, Color.Orange);
                    Engine.addPlayerPattern(PatternManager.Mix);
                    Engine.setPlayerDelay(20);

                    //Load Level
                    Engine.loadLevel(level);

                    //Change game state
                    CurrentGameState = GameState.InGame;
                    break;
                case GameState.InGame:
                    Engine.update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    
                    //Draw background image
                    background.draw(spriteBatch);

                    //Draw menu
                    mainMenu.draw(spriteBatch);
                    levelMenu.draw(spriteBatch);

                    spriteBatch.End();
                    break;
                case GameState.InitializeLevel:
                    //?? loading screen perhaps?
                    break;
                case GameState.InGame:
                    spriteBatch.Begin();
                    Engine.draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
