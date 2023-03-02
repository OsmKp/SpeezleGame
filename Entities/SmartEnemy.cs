using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeezleGame.AI;
using SpeezleGame.Entities.Players;
using SpeezleGame.Graphics;
using SpeezleGame.MapComponents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace SpeezleGame.Entities
{
    public class SmartEnemy : BaseEntity
    {
        private readonly RenderingStateMachine _renderingStateMachine = new RenderingStateMachine();
        private bool chasingPlayer;
        private float XDistanceToJumPoint;

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

        private string targetName = "player";
        private Vector2 targetLocation;
        private float movement;

        //horizontal movement constants
        private const float MoveAcceleration = 4000.0f;
        private const float MaxMoveSpeed = 1500.0f;
        private float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float GravityAcceleration = 2400.0f;
        private const float MaxFallSpeed = 300.0f;
        private float lastMovement;

        private bool isJumping;
        private bool wasJumping;
        private int jumpCounter;
        private float jumpTime;

        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -2400.0f;
        private const float JumpControlPower = 0.4f;

        PathfindingAI AI;

        public SmartEnemy(EnemyTextureContainer enemyTextureContainer, float groundDragFactor, NodeMap nodeMap, Rectangle enemyArea, Vector2 spawnPos)
        {
            GroundDragFactor = groundDragFactor;
            this.Health = 150;

            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Idle),
                new SpriteAnimation(enemyTextureContainer.Idle, 1, 16, 32));
            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Walk),
                new SpriteAnimation(enemyTextureContainer.Walk, 1, 16, 32));
            _renderingStateMachine.SetState(nameof(EnemyTextureContainer.Idle)); //set the initial state to idle
            _renderingStateMachine.CurrentState.Animation.Play();

            Position = spawnPos;

            AI = new PathfindingAI(enemyArea, nodeMap);

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
        public override void Update(GameTime gameTime, Vector2 playerPos, List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, Player player)
        {
            Stack<Node> path = AI.DoCalculations(Position, playerPos, player.playerBounds);

            CalculateMovement(path, playerPos, player.playerBounds);

            ApplyPhysics(gameTime, RectangleMapObjects, PolygonCollisionObjects, mapObjects, player);

            if (IsAlive)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0 && IsOnGround)
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Walk));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }
                else if (Velocity.X == 0)
                {
                    _renderingStateMachine.SetState(nameof(PlayerTextureContainer.Idle));
                    _renderingStateMachine.CurrentState.Animation.Play();
                }

            }

            _renderingStateMachine.Update(gameTime);
        }
        private void CalculateMovement(Stack<Node> path, Vector2 playerPos, Rectangle playerBounds)
        {
            if (path.Count == 0) 
            { 
                if(Math.Abs(playerPos.X - Position.X) > 8 && playerBounds.Intersects(AI.EntityArea))
                {
                    movement = CalculateMovementDir(playerPos);
                }
                else
                {
                    movement = 0f;
                    isJumping = false;
                }
                return; 
            }

            Node nextNode = path.Peek();
            
            bool closeEnoughToJump = CloseEnoughToJump(nextNode.Position);
            bool nodeIsAbove = IsNodeAbove(nextNode.Position);
            movement = CalculateMovementDir(nextNode.Position);

            if(nodeIsAbove && closeEnoughToJump)
            {
                
                isJumping = true;
            }

        }

        private bool IsNodeAbove(Vector2 nodePos)
        {
            float heightDiff = Position.Y - nodePos.Y;
            if(heightDiff > 16)
            {

                return true;
            }
            else{
                
                return false;
            }
        }
        private bool CloseEnoughToJump(Vector2 nodePos)
        {
            float xDiff = Math.Abs(nodePos.X - Position.X);
            if (xDiff < 32)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private float CalculateMovementDir(Vector2 nodePos)
        {
            if(nodePos.X - Position.X >= 0)
            {
                
                return 1f;
            }
            else {  return -1f; }
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
            if (DamageCooldownCounter >= DamageCooldown)
            {
                DamageCooldownCounter = 0f;
                isDamageLocked = false;
            }
            if (isDamageLocked)
            {
                DamageCooldownCounter += elapsed;
            }
            //--------------

            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);


            if (IsOnGround) //if the enemy is on the ground apply resistive forces
            { velocity.X *= GroundDragFactor; }
            else
            { velocity.X *= AirDragFactor; }

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed; //change the position by using the velocity
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions(RectangleMapObjects, new List<TiledPolygon>(), mapObjects, elapsed, player);

            if (Position.X == previousPosition.X)
            { velocity.X = 0; }

            if (Position.Y == previousPosition.Y)
            { velocity.Y = 0; }


        }
        private void HandleCollisions(List<Rectangle> RectangleMapObjects, List<TiledPolygon> PolygonCollisionObjects, List<MapObject> mapObjects, float elapsed, Player player)
        {
            HandleRectangleRectangleCollisions(RectangleMapObjects, mapObjects, player); //handle collisions with rectangle objects
        }
        private void HandleRectangleRectangleCollisions(List<Rectangle> RectangleMapObjects, List<MapObject> mapObjects, Player player)
        {
            Rectangle bounds = enemyBounds;
            isOnGround = false;

            if (bounds.Intersects(player.playerBounds) && !isDamageLocked)
            {
                player.DamagePlayer(Damage);
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
                            
                            // Perform further collisions with the new bounds.
                            bounds = enemyBounds;
                        }
                    }
                }

            }

        }
    }
}
