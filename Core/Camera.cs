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
        



        public Camera(Player targetPlayer, Viewport vp)
        {
            camPos = Vector2.Zero;

            playerToFollow = targetPlayer;

            

            
            viewport = vp;
        }

        private void Smoothing(Vector2 target)
        {
            float xFactor = (target.X - camPos.X) / 5;
            if(xFactor < 0.01f)
                xFactor = xFactor * 2;
            camPos.X += xFactor;

            if(camPos.X < 0)
            {
                camPos.X = 0;
            }
            else if(camPos.X + 640 > 1920)
            {
                camPos.X = 1920 - 640;
            }

            float yFactor = (target.Y - camPos.Y) / 8;
            if (yFactor < 0.01f)
                yFactor = yFactor * 10;
            Debug.WriteLine(yFactor);
            camPos.Y += yFactor;

        }



        private Vector2 GetTarget()
        {
            var playerRectangle = playerToFollow.playerBounds;
            Vector2 target = new Vector2(playerToFollow.Position.X + (playerRectangle.Width / 2) - halfRenderTargetWidth,
                playerToFollow.Position.Y + (playerRectangle.Height / 2) - halfRenderTargetHeight);
            plrPos = target;
            return target;
        }

        public void SetMatrix()
        {
            //Matrix scaleMatrix = Matrix.CreateTranslation(new Vector3(plrPos.X, plrPos.Y, 0)) * Matrix.CreateScale(new Vector3(0.8f, 0.8f, 1));   //FIX THIS ZOOMING PROBLEM
            TransformMatrix = Matrix.CreateTranslation(new Vector3(-camPos.X, -camPos.Y, 0)) * Matrix.CreateScale(new Vector3(1f, 1f, 1));
        }

        public void Follow()
        {
            Vector2 target = GetTarget();
            
            Smoothing(target);

            SetMatrix();

            
            /*var playerPos = Matrix.CreateTranslation(
                -playerRectangle.X -16- (playerRectangle.Width / 2),
                -playerRectangle.Y -16- (playerRectangle.Height / 2),
                0);

            var offset = Matrix.CreateTranslation(
                640 / 2,
                360 / 2,
                0);

            TransformMatrix = playerPos * offset;*/
        }

    }
}
