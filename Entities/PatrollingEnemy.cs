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
using SpeezleGame.States;
using SpeezleGame.Entities.Players;
using SpeezleGame.MapComponents;

namespace SpeezleGame.Entities
{
    public class PatrollingEnemy : BaseEntity
    {
        private readonly RenderingStateMachine _renderingStateMachine = new RenderingStateMachine();
        private bool chasingPlayer;
        private List<Vector2> waypoints;
        private int currentWaypointIndex = 0;

        private float previousBottom;
        public Rectangle enemyBounds
        {
            get
            {

                return new Rectangle((int)Position.X + 2, (int)Position.Y, 14, 33);
            }
        }

        public bool IsAlive { get { return isAlive; } }
        bool isAlive = true;

        private int Damage = 20;
        private const float DamageCooldown = 3;
        private float DamageCooldownCounter;
        private bool isDamageLocked;
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private int samePosFrameCounter;
        private Vector2 previousPosition;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        private float movement;
        private float lastMovement;

        //horizontal movement constants
        private const float MoveAcceleration = 4000.0f;
        private const float MaxMoveSpeed = 1500.0f;
        private float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float GravityAcceleration = 2400.0f;
        private const float MaxFallSpeed = 300.0f;


        private bool isJumping;
        private bool wasJumping;
        private int jumpCounter;
        private float jumpTime;


        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -2400.0f;
        private const float JumpControlPower = 0.14f;
        public PatrollingEnemy(EnemyTextureContainer enemyTextureContainer, float groundDragFactor, List<Vector2> _waypoints)
        {
            this.Health = 150;
            GroundDragFactor = groundDragFactor;

            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Idle),
                new SpriteAnimation(enemyTextureContainer.Idle, 1, 16, 32));
            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Walk),
                new SpriteAnimation(enemyTextureContainer.Walk, 1, 16, 32));
            _renderingStateMachine.SetState(nameof(EnemyTextureContainer.Idle)); //set the initial state to idle
            _renderingStateMachine.CurrentState.Animation.Play();

            this.waypoints = _waypoints;

            this.Position = new Vector2(waypoints[0].X, waypoints[0].Y);
        }
        public void CalculateMovement(Vector2 playerPos)
        {
            float distanceToPlayer = (playerPos - Position).Length();
            if(distanceToPlayer < 100) 
            { 
                chasingPlayer = true; 
            }
            else { chasingPlayer = false; }
            
            if (chasingPlayer)
            {
                var direction = playerPos - this.Position;
                if (direction.X > 0) { movement = 1f; }
                else { movement = -1f; }
            }

            else if (!chasingPlayer)
            {


                var direction = waypoints[currentWaypointIndex] - this.Position;

                if (direction.X > 0) { movement = 1f; }
                else { movement = -1f; }

                if(Math.Abs(direction.X) < 4)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count();
                }
                

                
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteEffects flip = SpriteEffects.None;

            if (Velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X > 0)
                flip = SpriteEffects.None;
            else if (lastMovement == -1.0f)
                flip = SpriteEffects.FlipHorizontally;
            else if (lastMovement == 1.0f)
                flip = SpriteEffects.None;

            _renderingStateMachine.Draw(spriteBatch, Position, flip);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime, Vector2 playerPos ,List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, Player player)
        {
            CalculateMovement(playerPos);

            ApplyPhysics(gameTime, RectangleMapObjects, PolygonCollisionObjects,mapObjects, player);

            if (IsAlive)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0 && IsOnGround)
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Walk));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }
                else if (Velocity.X == 0 )
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Idle));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }


            }

            _renderingStateMachine.Update(gameTime);
        }
        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                isJumping = false;
                jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {

                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower)); //if jumping
                }
                else
                {
                    jumpTime = 0.0f;
                    isJumping = false;

                }
            }
            else
            {

                jumpTime = 0.0f;

            }

            
            wasJumping = isJumping;
            
            return velocityY;
        }

        public void ApplyPhysics(GameTime gameTime, List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, Player player)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            previousPosition = Position;
            //damage cooldown
            if(DamageCooldownCounter >= DamageCooldown)
            {
                DamageCooldownCounter = 0f;
                isDamageLocked = false;
            }
            if (isDamageLocked)
            {
                DamageCooldownCounter += elapsed;
            }
            //--------------

            Debug.WriteLine("movement is: " + movement);
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            Debug.WriteLine("enemy x vel is: " + Velocity.X);
            if (IsOnGround) //if the player is on the ground apply resistive forces
            { velocity.X *= GroundDragFactor; }
            else
            { velocity.X *= AirDragFactor; }

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed; //change the position by using the velocity
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions(RectangleMapObjects, new List<TiledPolygon>(),mapObjects, elapsed,  player);

            if (Position.X == previousPosition.X)
            { velocity.X = 0; }

            if (Position.Y == previousPosition.Y)
            { velocity.Y = 0; }


        }
        private void HandleCollisions( List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, float elapsed, Player player)
        {
            HandleRectangleRectangleCollisions(RectangleMapObjects,mapObjects, player); //handle collisions with rectangle objects
        }

        private void HandleRectangleRectangleCollisions(List<Rectangle> RectangleMapObjects, List<MapObject> mapObjects, Player player)
        {
            Rectangle bounds = enemyBounds;
            isOnGround = false;

            if (bounds.Intersects(player.playerBounds) && !isDamageLocked)
            {
                player.Health -= this.Damage;
                isDamageLocked = true;
            }

            foreach (var collisionObject in RectangleMapObjects)
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
                                



                            if (IsOnGround)
                            {
                                // Resolve the collision along the Y axis.
                                Position = new Vector2(Position.X, Position.Y + depth.Y);

                                bounds = enemyBounds;
                            }
                            else
                            {
                                Position = new Vector2(Position.X, Position.Y + depth.Y);
                                        
                            }

                        }
                        else
                        {
                            Position = new Vector2(Position.X + depth.X, Position.Y);
                            isJumping = true;
                            // Perform further collisions with the new bounds.
                            bounds = enemyBounds;
                        }
                    }
                }
                    
            }


        }
    }
}
