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
using SpeezleGame.AI;

namespace SpeezleGame.Entities
{
    public class PatrollingEnemy : BaseEntity
    {
        private readonly RenderingStateMachine _renderingStateMachine = new RenderingStateMachine();
        private bool chasingPlayer;
        private float XDistanceToJumPoint;

        private List<Vector2> waypoints;
        private List<JumpTrigger> jumpPoints;
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

        //pathfinding
        private Node startNode;
        private Node endNode;
        private List<Node> openList;
        private List<Node> closedList;
        private List<Node> path;
        private int[,] map;
        private int mapWidth;
        private int mapHeight;





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
        private const float JumpControlPower = 0.14f;
        public PatrollingEnemy(EnemyTextureContainer enemyTextureContainer, float groundDragFactor, List<Vector2> _waypoints, List<JumpTrigger> jumpPoints)
        {
            this.Health = 150;
            GroundDragFactor = groundDragFactor;

            this.jumpPoints = jumpPoints;

            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Idle),
                new SpriteAnimation(enemyTextureContainer.Idle, 1, 16, 32));
            _renderingStateMachine.AddState(nameof(EnemyTextureContainer.Walk),
                new SpriteAnimation(enemyTextureContainer.Walk, 1, 16, 32));
            _renderingStateMachine.SetState(nameof(EnemyTextureContainer.Idle)); //set the initial state to idle
            _renderingStateMachine.CurrentState.Animation.Play();

            this.waypoints = _waypoints;

            this.Position = new Vector2(waypoints[0].X, waypoints[0].Y);
        }

        private void PickTarget(Vector2 playerPos) 
        {
            if(targetName == "jump") //if the enemy is going to the jump point allow it to get away from the player.
            {
                if (Math.Abs(Position.X - playerPos.X) < XDistanceToJumPoint * 2 && Position.Y - playerPos.Y > 5) //if player is above the enemy
                {
                    targetName = "jump";
                }
                else
                {
                    targetName = "player";
                }
            }
            else
            {
                if (Math.Abs(Position.X - playerPos.X) < 2f && Position.Y - playerPos.Y > 5) //if their X coordinate is the same but the enemy is standing under the player
                {
                    targetName = "jump";
                }
                else
                {
                    targetName = "player";
                }
            }


        }

        private void FindNearestJumpPoint() //finding the nearest jump trigger point to be able to jump up 
        {
            float closestDist = 9999;

            foreach (var obj in jumpPoints)
            {
                Vector2 objPos = new Vector2(obj.Bounds.X, obj.Bounds.Y);
                float distX = Math.Abs(objPos.X - Position.X);
                float distY = Math.Abs(objPos.Y - Position.Y);
                if (distX < closestDist && distY < 5f)
                {
                    closestDist = distX;
                    targetLocation = objPos + new Vector2(movement * 3, 0);
                    XDistanceToJumPoint = distX;
                }

            }
        }

        /*public void DoAStar(Vector2 playerPosition)
        {
            // Set the start and end nodes
            startNode = new Node { Position = this.Position };
            endNode = new Node { Position = playerPosition };


            // Initialize the open and closed lists
            openList = new List<Node>();
            closedList = new List<Node>();

            // Add the start node to the open list
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Get the node with the lowest cost from the open list
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].Cost < currentNode.Cost)
                    {
                        currentNode = openList[i];
                    }
                }

                // Remove the current node from the open list and add it to the closed list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // If the current node is the end node, we have found a path
                if (currentNode == endNode)
                {
                    BuildPath(currentNode);
                    break;
                }

            }

        }*/
            
        public void CalculateMovement(Vector2 playerPos) //This function allows the enemy to chase the player and adds a level of pathfinding to it
        {
            float distanceToPlayer = (playerPos - Position).Length();
            if (distanceToPlayer < 500)
            {
                chasingPlayer = true;
                Debug.WriteLine("TARGET IS: " + targetName);
            }
            else { chasingPlayer = false; targetName = "player"; }

            if (chasingPlayer)
            {
                PickTarget(playerPos);
                if (targetName == "player")
                {
                    if (Math.Abs(playerPos.Y - Position.Y) < 2f)
                    {
                        targetLocation = playerPos;
                        Debug.WriteLine("NORMAL CHASE");
                    }
                    else if ((playerPos.Y - Position.Y) > 2f && Math.Abs(playerPos.X - Position.X) < 20) //right underneath the player
                    {
                        targetLocation = new Vector2(Position.X - 20, Position.Y);
                        Debug.WriteLine("RIGHT UNDER THE PLAYER");
                    }
                    else
                    {
                        targetLocation = playerPos;
                        
                    }
                    

                    if (targetName == "jump")
                    {
                        FindNearestJumpPoint();
                    }

                    //Code below moves the enemy in the correct direction depending on where its target is
                    var direction = targetLocation - this.Position;
                    if (direction.X > 0)
                    {
                        movement = 1f;
                    }
                    else if (direction.Y == 0 && direction.X == 0)
                    {
                        movement = 0f;
                        targetName = "player";
                    }
                    else { movement = -1f; }
                }
                else
                {
                    var direction = waypoints[currentWaypointIndex] - this.Position;

                    if (direction.X > 0) { movement = 1f; }
                    else { movement = -1f; }

                    if (Math.Abs(direction.X) < 4)
                    {
                        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count();
                    }
                }




                /*float distanceToPlayer = (playerPos - Position).Length();
                if(distanceToPlayer < 500) 
                { 
                    chasingPlayer = true;
                    Debug.WriteLine("TARGET IS: " + targetName);
                }
                else { chasingPlayer = false; targetName = "player"; }

                if(playerPos.Y <= Position.Y)
                {
                    targetName = "player";
                }

                if (chasingPlayer && targetName == "player")
                {
                    var direction = playerPos - this.Position;
                    if (direction.X > 0 && Math.Abs(direction.Y) !=  0)
                    {
                        movement = 1f;
                    }
                    else if (direction.Y == 0 && direction.X == 0)
                    {

                        movement = 0f;
                    }
                    else if (direction.Y < 0 && direction.X == 0)
                    {
                        targetName = "jump";
                        float closestDist = 9999;

                        foreach (var obj in jumpPoints)
                        {
                            Vector2 objPos = new Vector2(obj.Bounds.X, obj.Bounds.Y);
                            float dist = (objPos - Position).Length();
                            if (dist < closestDist)
                            {
                                closestDist = dist;
                                targetLocation = objPos;
                            }

                        }
                    }




                    else { movement = -1f; }


                }

                else if (chasingPlayer && targetName == "jump")
                {
                    var direction = targetLocation - this.Position;
                    if (direction.X > 0)
                    {
                        movement = 1f;
                    }
                    else if (direction.Y == 0 && direction.X == 0)
                    {

                        targetName = "player";
                    }
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



                }*/
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
                            isJumping = true;
                            // Perform further collisions with the new bounds.
                            bounds = enemyBounds;
                        }
                    }
                }
                    
            }
            

            foreach (var mapObject in mapObjects)
            {
                

                if (bounds.Intersects(mapObject.Bounds))
                {
                    JumpTrigger jumpObj = mapObject as JumpTrigger;

                    //if enemy collides with a jump trigger then make a decision
                    

                    if (jumpObj != null)
                    {

                        var direction = player.Position - this.Position;
                        if(direction.Y < 0 && !isJumping)
                        {

                            isJumping = true; 

                        }
                    }
                    
                }
            }


        }
    }
}
