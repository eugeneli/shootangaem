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
using System.Diagnostics;

namespace ShootanGaem
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        StreamReader level;
        int levelNum;
        ShootanEngine Engine;

        //backgrounds & images
        Background background;
        Background level1bg;
        Background level2bg;
        Background level3bg;
        Texture2D logo;

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

        SGButton mode1;
        SGButton mode2;
        SGButton mode3;

        //Keep track of game state
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

            //Level backgrounds
            Texture2D level1BgTexture = Content.Load<Texture2D>(@"backgrounds\level1bg");
            level1bg = new Background(level1BgTexture, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, true);
            level1bg.setViewport(new Rectangle(0, level1BgTexture.Height - 768, 1024, 768));

            Texture2D level2BgTexture = Content.Load<Texture2D>(@"backgrounds\level2bg");
            level2bg = new Background(level2BgTexture, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, true);
            level2bg.setViewport(new Rectangle(0, level2BgTexture.Height - 768, 1024, 768));

            Texture2D level3BgTexture = Content.Load<Texture2D>(@"backgrounds\level1bg");
            level3bg = new Background(level3BgTexture, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, true);
            level3bg.setViewport(new Rectangle(0, level3BgTexture.Height - 768, 1024, 768));

            //Load logo
            logo = Content.Load<Texture2D>(@"sprites\reallybadlogo");

            //Create engine
            Engine = new ShootanEngine(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Content);

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

            //Mode selection buttons
            mode1 = new SGButton(Content.Load<Texture2D>(@"menu\mode1up"), Content.Load<Texture2D>(@"menu\mode1down"), new Rectangle(340, 300, 100, 100), Content.Load<SpriteFont>("MenuFont"));
            mode1.setText("");

            mode2 = new SGButton(Content.Load<Texture2D>(@"menu\mode2up"), Content.Load<Texture2D>(@"menu\mode2down"), new Rectangle(450, 300, 100, 100), Content.Load<SpriteFont>("MenuFont"));
            mode2.setText("");

            mode3 = new SGButton(Content.Load<Texture2D>(@"menu\mode3up"), Content.Load<Texture2D>(@"menu\mode3down"), new Rectangle(560, 300, 100, 100), Content.Load<SpriteFont>("MenuFont"));
            mode3.setText("");
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
                        levelNum = 1;
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
                    if (DLCButton.clicked)
                    {
                        Process.Start("http://ea.com");
                        DLCButton.clicked = false;
                    }
                    if (quitButton.clicked)
                    {
                        Exit();
                    }
                    if (level1.clicked)
                    {
                        levelNum = 1;
                        level = new StreamReader(@"levels\level1.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        level1.clicked = false;
                    }
                    if (level2.clicked)
                    {
                        levelNum = 2;
                        level = new StreamReader(@"levels\level2.txt");
                        CurrentGameState = GameState.InitializeLevel;

                        level2.clicked = false;
                    }
                    if (level3.clicked)
                    {
                        levelNum = 3;
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
                    //Update Background
                    background.update(gameTime);

                    //Mode selection
                    mode1.update(Mouse.GetState());
                    mode2.update(Mouse.GetState());
                    mode3.update(Mouse.GetState());

                    if (mode1.clicked)
                    {
                        Engine.createPlayer(Content.Load<Texture2D>(@"sprites\player_default"), new Vector2((graphics.PreferredBackBufferWidth / 2) - 30, (graphics.PreferredBackBufferHeight - 120)));
                        Engine.addPlayerPattern(PatternManager.Straight);
                        Engine.setPlayerDelay(50);

                        Engine.setPlayerSpeed(8);
                        Engine.addPlayerBullets(Content.Load<Texture2D>(@"sprites\round_bullet"), 100, Color.White, 15, 20);

                        //Load Level
                        Engine.loadLevel(level);

                        //Change game state
                        CurrentGameState = GameState.InGame;
                    }
                    else if (mode2.clicked)
                    {
                        Engine.createPlayer(Content.Load<Texture2D>(@"sprites\player_default"), new Vector2((graphics.PreferredBackBufferWidth / 2) - 30, (graphics.PreferredBackBufferHeight - 120)));
                        Engine.addPlayerPattern(PatternManager.Straight);
                        Engine.setPlayerDelay(50);

                        Engine.setPlayerSpeed(14);
                        Engine.addPlayerBullets(Content.Load<Texture2D>(@"sprites\round_bullet"), 100, Color.White, 7, 20);

                        //Load Level
                        Engine.loadLevel(level);

                        //Change game state
                        CurrentGameState = GameState.InGame;
                    }
                    else if (mode3.clicked)
                    {
                        Engine.createPlayer(Content.Load<Texture2D>(@"sprites\player_default"), new Vector2((graphics.PreferredBackBufferWidth / 2) - 30, (graphics.PreferredBackBufferHeight - 120)));
                        Engine.addPlayerPattern(PatternManager.Straight);
                        Engine.setPlayerDelay(50);

                        Engine.setPlayerSpeed(10);
                        Engine.addPlayerBullets(Content.Load<Texture2D>(@"sprites\round_bullet"), 100, Color.White, 7, 30);

                        //Load Level
                        Engine.loadLevel(level);

                        //Change game state
                        CurrentGameState = GameState.InGame;
                    }

                    break;
                case GameState.InGame:
                    //Update Background
                    if (levelNum == 1)
                        level1bg.update(gameTime);
                    else if (levelNum == 2)
                        level2bg.update(gameTime);
                    else if (levelNum == 3)
                        level3bg.update(gameTime);

                    //Update game
                    Engine.update(gameTime);

                    if (!Engine.gameIsRunning())
                        CurrentGameState = GameState.MainMenu;

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:     
                    //Draw images
                    background.draw(spriteBatch);
                    spriteBatch.Draw(logo, new Vector2(), Color.White);

                    //Draw menu
                    mainMenu.draw(spriteBatch);
                    levelMenu.draw(spriteBatch);

                    break;
                case GameState.InitializeLevel:
                    //Draw background image
                    background.draw(spriteBatch);

                    mode1.draw(spriteBatch);
                    mode2.draw(spriteBatch);
                    mode3.draw(spriteBatch);

                    break;
                case GameState.InGame:
                    //Draw background image
                    if (levelNum == 1)
                        level1bg.draw(spriteBatch);
                    else if (levelNum == 2)
                        level2bg.draw(spriteBatch);
                    else if (levelNum == 3)
                        level3bg.draw(spriteBatch);
                    

                    Engine.draw(spriteBatch);

                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
