using Microsoft.Xna.Framework;
using SpeezleGame.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.Core
{
    public class Camera
    {
        Player playerToFollow;
        public Matrix TransformMatrix { get; private set; }

        public Camera(Player targetPlayer)
        {
            playerToFollow = targetPlayer;
        }

        public void Follow()
        {
            var playerRectangle = playerToFollow.playerBounds;
            var playerPos = Matrix.CreateTranslation(
                -playerRectangle.X - (playerRectangle.Width / 2),
                -playerRectangle.Y - (playerRectangle.Height / 2),
                0);

            var offset = Matrix.CreateTranslation(
                640 / 2,
                360 / 2,
                0);

            TransformMatrix = playerPos * offset;
        }

    }
}
