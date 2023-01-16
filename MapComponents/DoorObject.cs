﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeezleGame.MapComponents
{
    public class DoorObject : MapObject
    {
        public bool IsOpen
        {
            get { return _isOpen; }
        }
        private bool _isOpen;

        
        public DoorObject(int objectId, Rectangle bounds) : base(objectId, bounds)
        {
            tileCoordinates = new List<Vector2>();
            float rectActualX = (float)Math.Round((decimal)bounds.X / 16) * 16;
            float rectActualY = (float)Math.Round((decimal)bounds.Y / 16) * 16;
            float rectActualWidth = (float)Math.Round((decimal)bounds.Width / 16) * 16;
            float rectActualHeight = (float)Math.Round((decimal)bounds.Height / 16) * 16;

            for (int i = 0; i < (int)(rectActualWidth / 16); i++)
            {
                Debug.WriteLine("i is: " + i);
                for (int j = 0; j < (int)(rectActualHeight / 16); j++)
                {
                    Debug.WriteLine("i is: " + i + " and j is: " + j);
                    tileCoordinates.Add(new Vector2(rectActualX + (16 * i), rectActualY + (16 * j)));
                }
            }
        }
        
        public void ChangeDoorState()
        {
            _isOpen = !_isOpen;
        }
        

    }
}