using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XELibrary;

namespace SimpleRPG
{
    class Enemy : DrawableGameComponent
    {
        ICelAnimationManager celAnimationManager;
        IInputHandler inputHandler;
        SpriteBatch spriteBatch;
        public List<Enemy> enemies = new List<Enemy>();
        Random walkDirection = new Random();
        public bool isAttacking;
        public DateTime atkTime = new DateTime();
        private Vector2 position;
        protected int health = 1;
        protected int speed = 30;
        public int id = 0;
        bool collision = false;
        Random rnd = new Random();
        public Vector2 playerPosition { get; set; }
        enum Direction { Left, Right, Up, Down };
        private Direction direction = Direction.Right;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public void decHealth()
        {
            health--;
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Enemy(Game game, Vector2 newPos, int id, ref Player player) : base(game)
        {
            this.id = id;
            playerPosition = player.Position;
            position = newPos;
            celAnimationManager = (ICelAnimationManager)game.Services.GetService(typeof(ICelAnimationManager));
            inputHandler = (IInputHandler)game.Services.GetService(typeof(IInputHandler));
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
            celAnimationManager.AddAnimation("skeleton_left", "..//Enemy//Skeleton//skeleton_left", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_right", "..//Enemy//Skeleton//skeleton_right", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_up", "..//Enemy//Skeleton//skeleton_up", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_down", "..//Enemy//Skeleton//skeleton_down", celCount, 10);
            celCount = new CelCount(6, 1);
            celAnimationManager.AddAnimation("skeleton_attac_left", "..//Enemy//Skeleton//skeleton_attac_left", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_attac_right", "..//Enemy//Skeleton//skeleton_attac_right", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_attac_up", "..//Enemy//Skeleton//skeleton_attac_up", celCount, 10);
            celAnimationManager.AddAnimation("skeleton_attac_down", "..//Enemy//Skeleton//skeleton_attac_down", celCount, 10);
        }


        public override void Update(GameTime gameTime)
        {
            if (health < 1)
            {
                isAttacking = false;
            }
            if (health > 0)
            {
                Vector2 dPos = playerPosition - position;
                if (Math.Sqrt(Math.Pow(dPos.X, 2) + Math.Pow(dPos.Y, 2)) <= 64)
                {
                    collision = true;
                }
                else
                {
                    collision = false;
                }
                dPos.Normalize();

                if (Math.Abs(dPos.X) > Math.Abs(dPos.Y))
                {
                    if (dPos.X > 0)
                    {
                        direction = Direction.Right;
                    }
                    if (dPos.X < 0)
                    {
                        direction = Direction.Left;
                    }
                }
                else
                {
                    if (dPos.Y < 0)
                    {
                        direction = Direction.Up;
                    }
                    if (dPos.Y > 0)
                    {
                        direction = Direction.Down;

                    }
                }
                if (!collision)
                {
                    position.X += dPos.X;
                    position.Y += dPos.Y;
                    isAttacking = false;
                }
                else
                {
                    if (collision)
                    {
                        isAttacking = true;
                        atkTime = DateTime.Now;
                    }

                    else
                    {

                    }
                }


                int celWidth = celAnimationManager.GetAnimationFrameWidth("skeleton_right");

                if (position.X > (Game.GraphicsDevice.Viewport.Width - celWidth))
                    position.X = (Game.GraphicsDevice.Viewport.Width - celWidth);
                if (position.X < 0)
                    position.X = 0;
            }
            base.Update(gameTime);
        }

        internal bool IsAttacking()
        {
            return isAttacking;
        }

        public override void Draw(GameTime gameTime)
        {
            if (health > 0)
            {
                string action = !collision ? "skeleton_" : "skeleton_attac_";
                Vector2 pos = collision ? position - new Vector2(64, 64) : position;
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
            }
            //  base.Draw(gameTime);
        }


    }
}
