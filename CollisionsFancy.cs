/**************************************************************************
 * Galen Cochrane and Nathan Bitikofer 12/08/2014
 * CollisionsFancy.cs
 * This is an SAT based algorithm for detecting a collision between any two
 * convex polygons.
 * 
 * HISTORY:
 * It is a vast improvement over the crude axis-aligned
 * bounding box collision detection that was used in the original game.
 * Nathan and I worked on this together, initially because he needed it
 * to simulate collisions with his "Titanic," which was not anywhere near
 * square, and so could not be well described by an AABB.
 * It uses the Separating Axis Theorem (SAT) to detect collisions.
 * I use this opdated and condensed version to be able to have long skinny
 * meteors and long skinny projectiles in my game - something that was
 * impossible to do realistically with only an axis aligned bounding box.
 * 
 * It's slower than bounding boxes, so it can be turned off in the menu.
 *************************************************************************/
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids2._0
{
    static class FancyCollisions
    {
        /// <summary>
        /// Detect collision between any two convex polygons (as defined by an array of points)
        /// </summary>
        /// <param name="objectA">first gameObject</param>
        /// <param name="objectB">second gameObject</param>
        /// <returns></returns>
        public static Boolean isColliding(GameObject objectA, GameObject objectB)
        {
            // find object's corners, accounting for position and rotation
            CalculateCollisionCorners(objectA);
            CalculateCollisionCorners(objectB);
            
            // detect unanimous overlap of lines representing the objects onto all of each others' side normals
            return (projectAndDetect(objectA, objectB) && projectAndDetect (objectB, objectA));
        }
        /// <summary>
        /// Uses the Separating Axis Theorem - projects 'shadows' of each object onto a line
        /// parallel to each of each objects' sides to see if they all overlap.
        /// </summary>
        /// <param name="objectA">first gameObject</param>
        /// <param name="objectB">secont gameObject</param>
        /// <returns></returns>
        private static bool projectAndDetect(GameObject objectA, GameObject objectB)
        {
            // make an array of all of object A's sides' normals, onto each of which we will project two lines representing the two objects
            Coordinates[] projectionLines = new Coordinates[objectA.numberOfSignificantSides];
            for (int i = 0; i < objectA.numberOfSignificantSides; i++)
                projectionLines[i] = new Coordinates(objectA.collisionCorners[i].Y - objectA.collisionCorners[(i + 1) % (objectA.numberOfSignificantSides)].Y, -(objectA.collisionCorners[i].X - objectA.collisionCorners[(i + 1) % (objectA.numberOfSignificantSides)].X));  // % (objectA.numSides + 1)
            // For each of object A's sides' normals on which to project, do this:
            for (int j = 0; j < objectA.numberOfSignificantSides; j++)
            {
                // Make the projected line for object B
                float[] bPoints = new float[objectB.collisionCorners.Length];
                for (int i = 0; i < objectB.collisionCorners.Length; i++)
                {
                    bPoints[i] = getProjectedPoint(objectB.collisionCorners[i], projectionLines[j]);
                }
                float bSmall = bPoints[0];
                float bLarge = bPoints[0];
                for (int i = 0; i < objectB.collisionCorners.Length; i++)
                {
                    if (bPoints[i] < bSmall) bSmall = bPoints[i];
                    if (bPoints[i] > bLarge) bLarge = bPoints[i];
                }
                // Make the projected line for object A
                float[] aPoints = new float[objectA.collisionCorners.Length];
                for (int i = 0; i < objectA.collisionCorners.Length; i++)
                {
                    aPoints[i] = getProjectedPoint(objectA.collisionCorners[i], projectionLines[j]);
                }
                float aSmall = aPoints[0];
                float aLarge = aPoints[0];
                for (int i = 0; i < objectA.collisionCorners.Length; i++)
                {
                    if (aPoints[i] < aSmall) aSmall = aPoints[i];
                    if (aPoints[i] > aLarge) aLarge = aPoints[i];
                }
                // See if the two lines overlap
                if (aLarge <= bSmall || bLarge <= aSmall)
                    return false;                           // if any of the overlap tests fail, the objects are not colliding.
            }

            return true; // if all of the overlap tests showed overlap, return true;
        }
        /// <summary>
        /// Given the set of object-relative verteces stored in a gameObject, calculates the
        /// verteces after rotation
        /// </summary>
        /// <param name="gameObj">gameObject whose verteces you wish to rotate</param>
        public static void CalculateCollisionCorners(GameObject gameObj)
        {
            for (int i = 0; i < gameObj.collisionCorners.Length; i++)
            {
                gameObj.collisionCorners[i].setVector(gameObj.ObjectRelativeCorners[i]);
                rotatePointAboutOrigin(ref gameObj.collisionCorners[i], gameObj.RadRotation);
                gameObj.collisionCorners[i] += gameObj.Position;
            }            
        }
        /// <summary>
        /// Geven a vector projectionLine onto which to project, returns the projection of CoordinatesToProject
        /// onto projectionLine
        /// </summary>
        /// <param name="CoordinatesToProject"></param>
        /// <param name="projectionLine"></param>
        /// <returns></returns>
        private static float getProjectedPoint(Coordinates CoordinatesToProject, Coordinates projectionLine)
        {
            return ((projectionLine.X * CoordinatesToProject.X) + (projectionLine.Y * CoordinatesToProject.Y))
                            /
                           ((projectionLine.X * projectionLine.X) + (projectionLine.Y * projectionLine.Y));
        }
        /// <summary>
        /// given a point and a rotation, returns the rotated point
        /// </summary>
        /// <param name="centerToCorner">original vector</param>
        /// <param name="gameObjectRotation">angle of rotation</param>
        public static void rotatePointAboutOrigin(ref Coordinates centerToCorner, float gameObjectRotation)
        {
            centerToCorner.setVector(centerToCorner.getMagnitude(), centerToCorner.getDirection() + gameObjectRotation);
        }
    }
}

