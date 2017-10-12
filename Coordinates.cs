/**************************************************************************
 * Galen Cochrane 11/20/2014
 * Coordinates.cs
 * Provides a base for all physics simulated objects in the game.
 * Is not inherited by GameObject, but rather GameObject has three of
 * these - one for position, one for velocity, and one for dimensions.
 * 
 * Previous "Location" and "Movement" classes were effectively
 * replaced by this class.
 * 
 * Stores 2D coordinates as x,y (cartesian)
 * and provides methods to get and set these coordinates with
 * polar (vector) notation.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class Coordinates
    {
        float y;
        float x;
        public Coordinates() { x = 0; y = 0; }
        public Coordinates(float x, float y) { this.x = x; this.y = y; }
        public Coordinates(Coordinates vec) { x = vec.X; y = vec.Y; }
        public float Y { get { return y; } set { y = value; } }
        public float X { get { return x; } set { x = value; } }
        public PointF getPointF() { return new PointF((float)x, (float)y); }
        public float getMagnitude() { return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); }
        public float getDirection()
        {
            if (x == 0)
                if (y < 0)
                    return -(float)Math.PI / 2;
                else if (y == 0)
                    return 0;
                else
                    return (float)Math.PI / 2;
            return x < 0 ? (float)Math.Atan(y / x) + (float)Math.PI : (float)Math.Atan(y / x);    
        }
        public Coordinates getVector() { return new Coordinates(x, y); }
        public void setVector(float mag, float dir) { x = mag * (float)Math.Cos(dir); y = mag * (float)Math.Sin(dir); }
        public void setVector(Coordinates vec) { x = vec.X; y = vec.Y; }
        public void addVector(float mag, float dir) { x += mag * (float)Math.Cos(dir); y += mag * (float)Math.Sin(dir); }
        public void addVector(Coordinates vec) { x += vec.X; y += vec.Y; }
        public void subtractVector(float mag, float dir) { x -= mag * (float)Math.Cos(dir); y -= mag * (float)Math.Sin(dir); }
        public void subtractVector(Coordinates vec) { x -= vec.X; y -= vec.Y; }
        public float getMagnitudeFrom(float x, float y) { return (float)Math.Sqrt(Math.Pow(this.x - x, 2) + Math.Pow(this.y - y, 2)); }
        public float getDirectionFrom(float x, float y) 
        {
            if ((this.x - x) == 0)
                if ((this.y - y) < 0)
                    return -(float)Math.PI / 2;
                else if ((this.y - y) == 0)
                    return 0;
                else
                    return (float)Math.PI / 2;
            return (this.x - x) < 0 ? (float)Math.Atan((this.y - y) / (this.x - x)) + (float)Math.PI : (float)Math.Atan((this.y - y) / (this.x - x));
        }
        public float getMagnitudeFrom(Coordinates vec) { return getMagnitudeFrom(vec.X, vec.Y); }
        public float getDirectionFrom(Coordinates vec) { return getDirectionFrom(vec.X, vec.Y); }
        public float getMagnitudeTo(float x, float y) { return (float)Math.Sqrt(Math.Pow(x - this.x, 2) + Math.Pow(y - this.y, 2)); }
        public float getDirectionTo(float x, float y)
        {
            if ((x - this.x) == 0)
                if ((y - this.y) < 0)
                    return -(float)Math.PI / 2;
                else if ((y - this.y) == 0)
                    return 0;
                else
                    return (float)Math.PI / 2;
            return (x - this.x) < 0 ? (float)Math.Atan((y - this.y) / (x - this.x)) + (float)Math.PI : (float)Math.Atan((y - this.y) / (x - this.x));
        }
        public float getMagnitudeTo(Coordinates vec) { return getMagnitudeTo(vec.X, vec.Y); }
        public float getDirectionTo(Coordinates vec) { return getDirectionTo(vec.X, vec.Y); }
        static public Coordinates operator + (Coordinates lhs, Coordinates rhs)
        {
            return new Coordinates(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        static public Coordinates operator - (Coordinates lhs, Coordinates rhs)
        {
            return new Coordinates(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
    }
}
