using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpeezleGame.Physics
{
    //currently not in use
    public class Raycast2D
    {
        
        public Vector2 Direction;

        public Vector2 Origin;


        public Raycast2D(Vector2 position, Vector2 direction)
        {
            Origin = position;
            Direction = direction;
        }

        public bool Equals(Raycast2D other)
        {
            if (Origin.Equals(other.Origin))
            {
                return Direction.Equals(other.Direction);
            }

            return false;
        }
        

        public float? Intersects(Rectangle box)
        {
            float? num = null;
            float? num2 = null;
            if (Math.Abs(Direction.X) < 1E-06f)
            {
                if (Origin.X < box.Left || Origin.X > box.Right)
                {
                    return null;
                }
            }
            else
            {
                num = (box.Left - Origin.X) / Direction.X;
                num2 = (box.Right - Origin.X) / Direction.X;
                if (num > num2)
                {
                    float? num3 = num;
                    num = num2;
                    num2 = num3;
                }
            }

            if (Math.Abs(Direction.Y) < 1E-06f)
            {
                if (Origin.Y < box.Bottom || Origin.Y > box.Top)
                {
                    return null;
                }
            }
            else
            {
                float num4 = (box.Bottom - Origin.Y) / Direction.Y;
                float num5 = (box.Top - Origin.Y) / Direction.Y;
                if (num4 > num5)
                {
                    float num6 = num4;
                    num4 = num5;
                    num5 = num6;
                }

                if ((num.HasValue && num > num5) || (num2.HasValue && num4 > num2))
                {
                    return null;
                }

                if (!num.HasValue || num4 > num)
                {
                    num = num4;
                }

                if (!num2.HasValue || num5 < num2)
                {
                    num2 = num5;
                }
            }

            

            return num;
        }
    }
}
