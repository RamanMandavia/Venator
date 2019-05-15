using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public struct Circle
    {

        /*
         *              ***WARNING***
         * This class is not made by Dead Fish Studio
         * 
         * This class is made by craftworkgames
         * Source: https://stackoverflow.com/a/24563800
         * 
         * Use this class to facilitate creating circular hitboxes for players, enemies, etc
         * 
         * */

        //Tiny modifications made by Alex Sarnese. (like bug fix a bug from original code lol. Check your code before posting it online folks!)


        public Circle(int x, int y, int radius)
            : this()
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        //changed these to be public to allow us to change the circle's location and radius without making a new circle
        public int Radius { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool Intersects(Rectangle rectangle)
        {
            // the first thing we want to know is if any of the corners intersect
            var corners = new[]
            {
            new Point(rectangle.Left, rectangle.Top),
            new Point(rectangle.Left, rectangle.Bottom),
            new Point(rectangle.Right, rectangle.Top),
            new Point(rectangle.Right, rectangle.Bottom)
        };

            foreach (var corner in corners)
            {
                if (ContainsPoint(corner))
                    return true;
            }

            // next we want to know if the left, top, right or bottom edges overlap
            if (X - Radius > rectangle.Right || X + Radius < rectangle.Left)
                return false;

            if (Y - Radius > rectangle.Bottom || Y + Radius < rectangle.Top)
                return false;

            return true;
        }

        public bool Intersects(Circle circle)
        {
            // put simply, if the distance between the circle centre's is less than
            // their combined radius
            var centre0 = new Vector2(circle.X, circle.Y);
            var centre1 = new Vector2(X, Y);
            return Vector2.Distance(centre0, centre1) < Radius + circle.Radius;
        }

        public bool ContainsPoint(Point point)
        {
            var vector2 = new Vector2(point.X - X, point.Y - Y);
            return vector2.Length() <= Radius;
        }
    }
}
