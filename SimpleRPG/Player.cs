using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using XELibrary;

namespace SimpleRPG
{
    public class Player : DrawableGameComponent
    {
        ICelAnimationManager celAnimationManager;
        IInputHandler inputHandler;
        SpriteBatch spriteBatch;
        public DateTime atkTime = new DateTime();
        private Vector2 position = new Vector2(100, 100);
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }

        }

        private Direction direction = Direction.Right;
        private float speed = 200.0F;
        private int lives = 300;
        private bool isAttacking = false, isDead = false;
        private bool canMove = true;

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
            if (lives == 0)
            {
                isDead = true;
            }
        }
        public int GetLives()
        {
            return lives;
        }
        public bool IsDead()
        {
            return isDead;
        }
        public bool IsAttacking()
        {
            return isAttacking;
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
            celAnimationManager.AddAnimation("orc_dead", "orc_dead", celCount, 10);
            celCount = new CelCount(8, 1);
            celAnimationManager.AddAnimation("orc_shield_left", "orc_shield_left", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_right", "orc_shield_right", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_up", "orc_shield_up", celCount, 10);
            celAnimationManager.AddAnimation("orc_shield_down", "orc_shield_down", celCount, 10);
            celCount = new CelCount(1, 1);
            celAnimationManager.AddAnimation("heart", "..//Misc//heart", celCount, 10);
            celAnimationManager.AddAnimation("heart_broken", "..//Misc//heart_broken", celCount, 10);
            celAnimationManager.AddAnimation("orc_grave", "orc_grave", celCount, 10);



        }

        internal void SetEnemies(int enemiesNumber)
        {
            if (enemiesNumber <= 0)
                canMove = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
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
                    atkTime = DateTime.Now;
                    isAttacking = true;
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
                else
                {
                    atkTime = new DateTime();
                    isAttacking = false;
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
            }

            int celWidth = celAnimationManager.GetAnimationFrameWidth("orc_left");

            if (position.X > (Game.GraphicsDevice.Viewport.Width - celWidth))
                position.X = (Game.GraphicsDevice.Viewport.Width - celWidth);
            if (position.X < 0)
                position.X = 0;

            base.Update(gameTime);
        }
        public DateTime Arrack()
        {

            if (inputHandler.KeyboardHandler.IsKeyDown(Keys.Space))
            {
                DateTime now = DateTime.Now;
                return now;
            }
            else return new DateTime();
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
            int i = 0;
            for (; i < lives; i++)
            {
                celAnimationManager.Draw(gameTime, "hart", spriteBatch, new Vector2(100, 100), SpriteEffects.None);
            }
            for (; i < 3; i++)
            {
                celAnimationManager.Draw(gameTime, "hart_broken", spriteBatch, new Vector2(10 + 64 * i, 10), SpriteEffects.None);
            }
            if (lives > 0)
            {
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
            }
            else
            {
                if (isDead)
                {
                    celAnimationManager.Draw(gameTime, "orc_grave", spriteBatch, pos, SpriteEffects.None);
                }
            }

            spriteBatch.End();

        }
    }
}
