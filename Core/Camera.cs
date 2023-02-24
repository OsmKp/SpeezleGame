using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SpeezleGame.Entities.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Core
{
    public class Camera
    {
        Player playerToFollow;
        public Matrix TransformMatrix { get; private set; }
        Vector2 camPos;
        Vector2 plrPos;
        Viewport viewport;

        private const int halfRenderTargetWidth = 320;
        private const int halfRenderTargetHeight = 180;

        private int mapWidth;
        private int mapHeight;



        public Camera(Player targetPlayer, Viewport vp, int camW, int camH)
        {
            camPos = Vector2.Zero;

            playerToFollow = targetPlayer;

            mapWidth = camW;
            mapHeight = camH;

            
            viewport = vp;
        }

        private void Smoothing(Vector2 target) //This function makes the camera movement smooth when player is making sharp moves.
        {
            float xFactor = (target.X - camPos.X) / 5;
            if(xFactor < 0.01f)
                xFactor = xFactor * 2;
            camPos.X += xFactor;

            if(camPos.X < 0)
            {
                camPos.X = 0;
            }
            else if(camPos.X + 640 > mapWidth)
            {
                camPos.X = mapWidth - 640;
            }

            float yFactor = (target.Y - camPos.Y) / 8;
            if (yFactor < 0.01f)
                yFactor = yFactor * 10;
            Debug.WriteLine(yFactor);
            camPos.Y += yFactor;

        }



        private Vector2 GetTarget() //Gets the player position
        {
            var playerRectangle = playerToFollow.playerBounds;
            Vector2 target = new Vector2(playerToFollow.Position.X + (playerRectangle.Width / 2) - halfRenderTargetWidth,
                playerToFollow.Position.Y + (playerRectangle.Height / 2) - halfRenderTargetHeight);
            plrPos = target;
            return target;
        }

        public void SetMatrix() //Creates a transformation matrix to draw everything in the right position
        {
            
            TransformMatrix = Matrix.CreateTranslation(new Vector3(-camPos.X, -camPos.Y, 0)) * Matrix.CreateScale(new Vector3(1f, 1f, 1)); 
            
        }

        public void Follow() //This function is called every frame so that camera position can be updated
        {
            Vector2 target = GetTarget();
            
            Smoothing(target);

            SetMatrix();

            
        }

    }
}
