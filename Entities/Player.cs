using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpeezleGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpeezleGame.Entities.Players
{


    public class Player : IGameEntity
    {
        private const int WALK_ANIM_SPRITE_COUNT = 4;
        private const int WALK_ANIM_SPRITE_SIZE = 32;
        private readonly RenderingStateMachine _renderingStateMachine = new RenderingStateMachine();
        private readonly Vector2 gravity = new Vector2(0, 10f);

        public bool IsAlive { get { return isAlive; } }
        bool isAlive = true;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

       

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3500.0f;
        private const float MaxFallSpeed = 500.0f;
        private const float JumpControlPower = 0.14f;

        private float movement;

        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        public Rectangle playerBounds
        {
            get
            {
                //return new Rectangle((int)Position.X - 8, (int)Position.Y - 16, 16, 33);
                return new Rectangle((int)Position.X + 8 , (int)Position.Y  , 16, 33);
            }
        }

        private const float EPSILON = 0.00001f;
        public int DrawOrder { get; set; }

        public int UpdateOrder { get; set; }




       // public float Speed { get; set; } = 100f; //pixels per second

       // private bool JumpAvailable { get; set; } = false;

       // private bool IsFacingRight { get; set; } = true;

        //private KeyboardState _previousKeyboardState;

        public Player(PlayerTextureContainer textureContainer)
        {
            Position = new Vector2(100, 200); // TEMP
            //playerBounds = new Rectangle((int)Position.X-8, (int)Position.Y-16, 16,33);

            _renderingStateMachine.AddState(nameof(PlayerTextureContainer.Idle),
                new SpriteAnimation(textureContainer.Idle, WALK_ANIM_SPRITE_COUNT, WALK_ANIM_SPRITE_SIZE, WALK_ANIM_SPRITE_SIZE));
            _renderingStateMachine.AddState(nameof(PlayerTextureContainer.Walk),
                new SpriteAnimation(textureContainer.Walk, WALK_ANIM_SPRITE_COUNT, WALK_ANIM_SPRITE_SIZE, WALK_ANIM_SPRITE_SIZE));

            _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Idle));
            _renderingStateMachine.CurrentState.Animation.Play();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix transformMatrix)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (Velocity.X > 0)
                flip = SpriteEffects.None;
            else if (Velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
            _renderingStateMachine.Draw(spriteBatch, Position, flip);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, List<Rectangle> collisionObjects)
        {
            GetInput(keyboardState);

            ApplyPhysics(gameTime, collisionObjects);

            //_velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;   
            //Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(IsAlive && IsOnGround)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Walk));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }
                else
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Idle));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }
            }

            _renderingStateMachine.Update(gameTime);

            

            
            //ProcessControls();

            movement = 0.0f;
            isJumping = false;
        }

        private void GetInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            isJumping = keyboardState.IsKeyDown(Keys.Space);
        }

        public void ApplyPhysics(GameTime gameTime, List<Rectangle> collisionObjects)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = Position;

            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            if (IsOnGround)
            { velocity.X *= GroundDragFactor; }
            else
            { velocity.X *= AirDragFactor; }

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions(collisionObjects);

            if (Position.X == previousPosition.X)
            { velocity.X = 0; }

            if (Position.Y == previousPosition.Y)
            { velocity.Y = 0; }

        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                if((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    jumpTime = 0.0f;
                }
            }
            else
            {
                jumpTime = 0.0f;
            }

            wasJumping = isJumping;
            return velocityY;
        }

        private void HandleCollisions(List<Rectangle> collisionObjects)
        {
            Rectangle bounds = playerBounds;
            isOnGround = false;

            foreach (var collisionObject in collisionObjects)
            {
                if (bounds.Intersects(collisionObject))
                {
                    Vector2 depth = Physics.RectangleExtensions.GetIntersectionDepth(bounds, collisionObject);
                    if (depth != Vector2.Zero)
                    {
                        float absDepthX = Math.Abs(depth.X);
                        float absDepthY = Math.Abs(depth.Y);

                        // Resolve the collision along the shallow axis.
                        if (absDepthY < absDepthX)
                        {
                            // If we crossed the top of a tile, we are on the ground.
                            if (previousBottom <= collisionObject.Top)
                                isOnGround = true;

                            // Ignore platforms, unless we are on the ground.
                            if (IsOnGround)
                            {
                                // Resolve the collision along the Y axis.
                                Position = new Vector2(Position.X, Position.Y + depth.Y);

                                bounds = playerBounds;
                            }
                            else
                            {
                                Position = new Vector2(Position.X, Position.Y + depth.Y);
                            }

                        }
                        else
                        {
                            Position = new Vector2(Position.X + depth.X, Position.Y);

                            // Perform further collisions with the new bounds.
                            bounds = playerBounds;
                        }
                    }
                }
            }
            previousBottom = bounds.Bottom;
            //ProcessBounds();
        }





        /*public void CheckPlatformCollision(List<Rectangle> collisionObjects, GameTime gameTime)
        {
            foreach(var collisionObject in collisionObjects)
            {
                if (IntersectsFromTop(collisionObject))
                {
                    Position.Y = collisionObject.Y - collisionObject.Height;
                }
                else if (IntersectsFromRight(collisionObject))
                {
                    Position.X = collisionObject.X + collisionObject.Width;
                }
            }
            
        }*/

        private bool IntersectsFromTop(Rectangle collisionObject)
        {
            var intersection = Rectangle.Intersect(playerBounds, collisionObject);
            return playerBounds.Intersects(collisionObject) && intersection.Y == collisionObject.Y && intersection.Width >= intersection.Height;
        }
        private bool IntersectsFromRight(Rectangle collisionObject)
        {
            var intersection = Rectangle.Intersect(playerBounds, collisionObject);
            return playerBounds.Intersects(collisionObject) && intersection.X + intersection.Width == collisionObject.X + collisionObject.Width && intersection.Width <= intersection.Height;

        }
        /*private bool IntersectsFromLeft(Rectangle collisionObject)
        {
            var intersection = Rectangle.Intersect(playerBounds, collisionObject);
            return playerBounds.Intersects(collisionObject) && 
        }*/






    }
}
