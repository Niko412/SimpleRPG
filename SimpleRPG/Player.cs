using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XELibrary;

namespace SimpleRPG
{
    public class Player : DrawableGameComponent
    {
        ICelAnimationManager celAnimationManager;
        IInputHandler inputHandler;
        SpriteBatch spriteBatch;
        private Vector2 position = new Vector2(100, 100);
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }

        }

        private Direction direction = Direction.Right;
        private float speed = 200.0F;
        public int health = 3;
        private int lives = 3;
        public bool isAttack = false, isShield = false;
        enum Direction { Left, Right, Up, Down };
        public Player(Game game) : base(game)
        {
            celAnimationManager = (ICelAnimationManager)game.Services.GetService(typeof(ICelAnimationManager));
            inputHandler = (IInputHandler)game.Services.GetService(typeof(IInputHandler));
        }
        public void incHealth()
        {
            if (lives < 3)
            {
                lives++;
            }
        }
        public void decHealth()
        {
            if (lives > 0)
            {
                lives--;
            }
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public void Load(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        protected override void LoadContent()
        {
            CelCount celCount = new CelCount(9, 1);
            celAnimationManager.AddAnimation("orc_left", "orc_left", celCount, 10);
            celAnimationManager.AddAnimation("orc_right", "orc_right", celCount, 10);
            celAnimationManager.AddAnimation("orc_up", "orc_up", celCount, 10);
            celAnimationManager.AddAnimation("orc_down", "orc_down", celCount, 10);
            celAnimationManager.ToggleAnimation("orc_left");
            celCount = new CelCount(6, 1);
            celAnimationManager.AddAnimation("orc_slash_left", "orc_slash_left", celCount, 10);
            celAnimationManager.AddAnimation("orc_slash_right", "orc_slash_right", celCount, 10);
            celAnimationManager.AddAnimation("orc_slash_up", "orc_slash_up", celCount, 10);
            celAnimationManager.AddAnimation("orc_slash_down", "orc_slash_down", celCount, 10);
            celCount = new CelCount(8, 1);
            celAnimationManager.AddAnimation("orc_shield_left", "orc_shield_left", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_right", "orc_shield_right", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_up", "orc_shield_up", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_down", "orc_shield_down", celCount, 10);
        }

        public override void Update(GameTime gameTime)
        {
            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Right))
            {
                celAnimationManager.ResumeAnimation("orc_right");
                direction = Direction.Right;
                position.X += (speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                celAnimationManager.PauseAnimation("orc_right");
            }
            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Left))
            {
                celAnimationManager.ResumeAnimation("orc_left");
                direction = Direction.Left;
                position.X -= (speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                celAnimationManager.PauseAnimation("orc_left");
            }
            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Up))
            {
                celAnimationManager.ResumeAnimation("orc_up");
                direction = Direction.Up;
                position.Y -= (speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                celAnimationManager.PauseAnimation("orc_up");
            }
            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Down))
            {
                celAnimationManager.ResumeAnimation("orc_down");
                direction = Direction.Down;
                position.Y += (speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                celAnimationManager.PauseAnimation("orc_down");
            }

            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Space))
            {
                switch (direction)
                {
                    case Direction.Down:
                        celAnimationManager.ResumeAnimation("orc_slash_down");
                        break;
                    case Direction.Up:
                        celAnimationManager.ResumeAnimation("orc_slash_up");
                        break;
                    case Direction.Left:
                        celAnimationManager.ResumeAnimation("orc_slash_left");
                        break;
                    case Direction.Right:
                        celAnimationManager.ResumeAnimation("orc_slash_right");
                        break;
                    default:
                        break;
                }
            }
            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.LeftShift))
            {
                switch (direction)
                {
                    case Direction.Down:
                        celAnimationManager.ResumeAnimation("orc_shield_down");
                        break;
                    case Direction.Up:
                        celAnimationManager.ResumeAnimation("orc_shield_up");
                        break;
                    case Direction.Left:
                        celAnimationManager.ResumeAnimation("orc_shield_left");
                        break;
                    case Direction.Right:
                        celAnimationManager.ResumeAnimation("orc_shield_right");
                        break;
                    default:
                        break;
                }
            }


            int celWidth = celAnimationManager.GetAnimationFrameWidth("orc_left");

            if (position.X > (Game.GraphicsDevice.Viewport.Width - celWidth))
                position.X = (Game.GraphicsDevice.Viewport.Width - celWidth);
            if (position.X < 0)
                position.X = 0;

            base.Update(gameTime);
        }
        public bool Arrack()
        {
            return inputHandler.KeyboardHandler.IsKeyDown(Keys.Space);
        }
        public bool Shield()
        {
            return inputHandler.KeyboardHandler.IsKeyDown(Keys.LeftShift);
        }
        public override void Draw(GameTime gameTime)
        {
            string action = inputHandler.KeyboardHandler.IsKeyDown(Keys.Space) ? "orc_slash_" : "orc_";
            action = inputHandler.KeyboardHandler.IsKeyDown(Keys.LeftShift) ? "orc_shield_" : action;
            Vector2 pos = inputHandler.KeyboardHandler.IsKeyDown(Keys.Space) ? position - new Vector2(64, 64) : position;
            spriteBatch.Begin();
            switch (direction)
            {
                case Direction.Down:
                    celAnimationManager.Draw(gameTime, action + "down", spriteBatch, pos, SpriteEffects.None);
                    break;
                case Direction.Up:
                    celAnimationManager.Draw(gameTime, action + "up", spriteBatch, pos, SpriteEffects.None);
                    break;
                case Direction.Left:
                    celAnimationManager.Draw(gameTime, action + "left", spriteBatch, pos, SpriteEffects.None);
                    break;
                case Direction.Right:
                    celAnimationManager.Draw(gameTime, action + "right", spriteBatch, pos, SpriteEffects.None);
                    break;
                default:
                    break;

            }
            spriteBatch.End();

            //  base.Draw(gameTime);
        }
    }
}
