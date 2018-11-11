using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using XELibrary;

namespace SimpleRPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private CelAnimationManager celAnimationManager;

        private InputHandler inputHandler;
        private List<Enemy> enemy;
        private Player player;
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
            enemy = new List<Enemy>();
            Random random = new Random();
            for (int i = 0; i < 1; i++)
            {
                enemy.Add(new Enemy(this, new Vector2(random.Next(100, 900), random.Next(100, 500)),player.Position));
            }
            foreach (var item in enemy)
            {
                Components.Add(item);
            }
            Components.Add(player);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Load(spriteBatch);
            foreach (var item in enemy)
            {
                item.Load(spriteBatch);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            foreach (var item in enemy)
            {
                item.playerPosition = player.Position;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);

            base.Draw(gameTime);
        }
    }
}
