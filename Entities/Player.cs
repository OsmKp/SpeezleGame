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
using TiledCS;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Diagnostics;
using SpeezleGame.Physics;
using Microsoft.Xna.Framework.Content;
using SpeezleGame.Core;
using Microsoft.Xna.Framework.Audio;

namespace SpeezleGame.Entities.Players
{


    public class Player : BaseEntity
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

       
        //horizontal movement constants
        private const float MoveAcceleration = 8000.0f;
        private const float MaxMoveSpeed = 1500.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        //vertical movement constants
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -2400.0f;
        private const float GravityAcceleration = 2400.0f;
        private const float MaxFallSpeed = 300.0f;
        private const float JumpControlPower = 0.14f;

        private const float MaxDashTime = 0.2f;
        private const float DashSpeed = 600.0f;

        //movement direction
        private float movement;
        private float lastMovement;

        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        //variables and constants for dashing
        private bool isDashing;
        private bool isDashingPressed;
        private bool isDashLocked ;
        private bool wasDashing ;
        private float dashTime;
        private const float dashCD = 1.0f;
        private float dashCDTimer;

        //variables  for jumping
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        //player collider
        public Rectangle playerBounds
        {
            get
            {
                
                return new Rectangle((int)Position.X + 2, (int)Position.Y  , 28, 33);
            }
        }

        private const float EPSILON = 0.00001f;
        public int DrawOrder { get; set; }

        public int UpdateOrder { get; set; }






        public Player(PlayerTextureContainer container) 
        {

            

            Position = new Vector2(100, 200); // TEMP
            


            //add animations to the player animation container
            _renderingStateMachine.AddState(nameof(PlayerTextureContainer.Idle),
                new SpriteAnimation(container.Idle, WALK_ANIM_SPRITE_COUNT, WALK_ANIM_SPRITE_SIZE, WALK_ANIM_SPRITE_SIZE));
            _renderingStateMachine.AddState(nameof(PlayerTextureContainer.Walk),
                new SpriteAnimation(container.Walk, WALK_ANIM_SPRITE_COUNT, WALK_ANIM_SPRITE_SIZE, WALK_ANIM_SPRITE_SIZE));

            _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Idle)); //set the initial state to idle
            _renderingStateMachine.CurrentState.Animation.Play();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime/*, Matrix transformMatrix, SpriteHandler spriteHandler*/)
        {
            SpriteEffects flip = SpriteEffects.None;
            
            //get which direction the player is facing and flip animations accordingly
            if (Velocity.X < 0)
                flip = SpriteEffects.None;
            else if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (lastMovement == -1.0f)
                flip = SpriteEffects.None;
            else if (lastMovement == 1.0f)
                flip = SpriteEffects.FlipHorizontally;

            //draw player
            

            //spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
            _renderingStateMachine.Draw(spriteBatch, Position, flip);
            //spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, /*MouseState previousMouseState,*/ List<Rectangle> RectangleCollisionObjects, List<TiledPolygon> PolygonCollisionObjects)
        {


            GetInput(keyboardState, mouseState/*, previousMouseState*/ ); //first get what keys are pressed each frame

            ApplyPhysics(gameTime, RectangleCollisionObjects, PolygonCollisionObjects); //apply physics and process key presses


            //set the correct animation depending on the player velocity
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



            //camera.Follow();
            

            movement = 0.0f;
            isJumping = false;
            
        }

        //get key inputs
        private void GetInput(KeyboardState keyboardState, MouseState mouseState/* MouseState previousMouseState*/)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
                lastMovement = -1.0f;
                
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
                lastMovement = 1.0f;
            }

            isJumping = keyboardState.IsKeyDown(Keys.Space);

            


            if (!isDashLocked)
            {
                isDashingPressed = keyboardState.IsKeyDown(Keys.LeftShift) || (mouseState.LeftButton == ButtonState.Pressed/* && previousMouseState.LeftButton == ButtonState.Pressed*/);
                if (isDashingPressed)
                {
                    dashCDTimer = 0;
                    isDashLocked = true;
                    //isDashingReleased = false;
                    isDashing = true;
                }
            }




        }

        public void ApplyPhysics(GameTime gameTime, List<Rectangle> RectangleCollisionObjects, List<TiledPolygon> PolygonCollisionObjects)
        {



            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = Position;

            velocity.X += movement * MoveAcceleration * elapsed; //increase player horizontal velocity using the acceleration
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            //increase player vertical velocity using the gravity and other variables

            velocity.Y = DoJump(velocity.Y, gameTime);//if player has jumped re-adjust y velocity

            if (IsOnGround) //if the player is on the ground apply resistive forces
            { velocity.X *= GroundDragFactor; }
            else
            { velocity.X *= AirDragFactor; } //else apply air resistance

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed); //readjust horizontal velocity

            velocity.X = DoDash(movement, velocity.X, gameTime); //if player has dashed change vertical velocity accordingly

            Position += velocity * elapsed; //change the position by using the velocity
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions(RectangleCollisionObjects, new List<TiledPolygon>(), elapsed); //handle collisions after moving


            //if collision is detected re adjust position
            if (Position.X == previousPosition.X)
            { velocity.X = 0; }

            if (Position.Y == previousPosition.Y)
            { velocity.Y = 0; }

        }

        
        private float DoDash(float movement, float VelocityX, GameTime gameTime)
        {



            if (isDashing)
            {
                if (!wasDashing  || dashTime > 0.0f) //if dashing or just starting dash
                {
                   
                    dashTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (dashTime > 0.0f && dashTime <= MaxDashTime) //is the player dashing
                {
                    
                    VelocityX = DashSpeed * movement;
                    
                }
                else if(dashTime >= MaxDashTime && dashTime != 0.0f) //Check if player is dashing
                {
                    
                    isDashing = false;
                    
                }

            }
            else
            {

                dashTime = 0.0f;
            }

            if (dashCDTimer >= dashCD) //if timer exceeds the cooldown then make the dash available and reset the timer
            {
                Debug.WriteLine("Cooldown reset");
                dashCDTimer = 0;
                isDashLocked = false;
            }
            if (isDashLocked) //no need to change the timer if dash is available
            {
                dashCDTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            

            Debug.WriteLine("Cooldown: " + Math.Round(dashCD - dashCDTimer, MidpointRounding.AwayFromZero)); // debuggin purp


            wasDashing = isDashing;
            return VelocityX;

        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                if((!wasJumping && IsOnGround) || jumpTime > 0.0f) //if just starting jump or jumping
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower)); //if jumping
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
        private void HandleCollisions(List<Rectangle> RectangleCollisionObjects, List<TiledPolygon> PolygonCollisionObjects, float elapsed)
        {

            
            
            
            
            
            HandleRectangleRectangleCollisions(RectangleCollisionObjects); //handle collisions with rectangle objects
        }

        /*private void HandleRaycastCollision(List<Rectangle> RectangleCollisionObjects, float elapsed)
        {
            Debug.WriteLine("Inraycast");
            Rectangle bounds = playerBounds;
            Raycast2D raycast = new Raycast2D(new Vector2(playerBounds.Center.X, playerBounds.Center.Y), new Vector2(movement, 0));
            List<float?> raycastLengths = new List<float?>();
            foreach(var collisionObject in RectangleCollisionObjects)
            {
                float? raycastLength = raycast.Intersects(collisionObject);
                raycastLengths.Add(raycastLength);
            }

            float? distance = raycastLengths.Min();
            
            Vector2 nextFramePlayerPos = Position + (velocity * elapsed);
            nextFramePlayerPos = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
            float nextPlrPosX = nextFramePlayerPos.X;
            float distancePlrWillMove = Math.Abs(Position.X - nextPlrPosX);
            if(distance < distancePlrWillMove)
            {
                Debug.WriteLine("velocity made 0");
                velocity.X = 0.0f;
            }
            

        }*/
        private void HandleRectangleRectangleCollisions(List<Rectangle> RectangleCollisionObjects)
        {
            Rectangle bounds = playerBounds;
            isOnGround = false;

            foreach (var collisionObject in RectangleCollisionObjects)
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
                            
                            if (previousBottom <= collisionObject.Top)
                                isOnGround = true;

                            
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
            
        }


    }
}
