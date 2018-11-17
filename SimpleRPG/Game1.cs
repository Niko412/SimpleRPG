using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using XELibrary;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

namespace SimpleRPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int SCREEN_HEIGHT = 600, SCREEN_WIDTH = 1000;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private CelAnimationManager celAnimationManager;
        private SpriteFont spriteFont;
        TiledMapRenderer mapReader;
        int EnemiesNumber = 10;
        private InputHandler inputHandler;
        private List<Enemy> enemies;
        private Player player;
        private Texture2D youDeadTitle;
        private Texture2D youWinTitle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            inputHandler = new InputHandler(this);
            Components.Add(inputHandler);

            celAnimationManager = new CelAnimationManager(this, "Player\\");

            Components.Add(celAnimationManager);

            player = new Player(this);
            enemies = new List<Enemy>();
            Random random = new Random();
            for (int i = 0; i < EnemiesNumber; i++)
            {
                enemies.Add(new Enemy(this, new Vector2(random.Next(100, 900), random.Next(100, 500)), i, ref player));
            }
            foreach (var item in enemies)
            {
                Components.Add(item);
            }
            Components.Add(player);
        }

        protected override void Initialize()
        {
            mapReader = new TiledMapRenderer(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Misc\\Font");
            youDeadTitle = Content.Load<Texture2D>("Misc\\you_dead");
            youWinTitle = Content.Load<Texture2D>("Misc\\you_win");
            player.Load(spriteBatch);
            foreach (var enemy in enemies)
            {
                enemy.Load(spriteBatch);
            }

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            foreach (var item in enemies)
            {
                item.playerPosition = player.Position;
            }
            foreach (var enemy in enemies)
            {
                if (enemy.atkTime != new DateTime() && enemy.isAttacking)
                {
                    if (player.IsAttacking() && player.atkTime != new DateTime())
                    {
                        if (enemy.atkTime >= player.atkTime)
                        {
                            enemy.decHealth();
                            EnemiesNumber--;
                            player.SetEnemies(EnemiesNumber);
                        }
                        else if (enemy.IsAttacking())
                        {
                            player.decHealth();
                        }
                    }
                    else if (enemy.IsAttacking())
                    {
                        player.decHealth();
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (EnemiesNumber > 0)
                GraphicsDevice.Clear(Color.LightGreen);

            if (player.GetLives() == 0)
            {
                GraphicsDevice.Clear(Color.Salmon);
            }
            spriteBatch.Begin();
            if (EnemiesNumber == 0)
            {
                GraphicsDevice.Clear(Color.Gold);
                spriteBatch.Draw(youWinTitle, new Vector2((SCREEN_WIDTH - youWinTitle.Width - 100) / 2, (SCREEN_HEIGHT - youWinTitle.Height) / 2), Color.Gold);
            }
            if (player.IsDead())
            {
                GraphicsDevice.Clear(Color.DarkSlateGray);

                spriteBatch.Draw(youDeadTitle, new Vector2((SCREEN_WIDTH - youDeadTitle.Width) / 2, (SCREEN_HEIGHT - youDeadTitle.Height) / 2), Color.Red);
            }
            spriteBatch.DrawString(spriteFont, "Health: " + player.GetLives().ToString() + "/300", new Vector2(10, 10), Color.Green);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
