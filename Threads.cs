/**************************************************************************
 * Galen Cochrane 11/20/2014
 * Threads.cs
 * Keeps track of all Tasks or Threads that are used by other classes. 
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Asteroids2._0
{
    /**********************************************************************
     * Currently there can be 24 or more tasks (threads) running
     * in parallel at a time.  There are probably many ways to
     * optimize this.
     *********************************************************************/
    static class Threads
    {
        public static Task Physics_Engine;      //   \
        public static Task Collisions_Engine;   //    |>    These are the five main engines that run loops in MainGame.
        public static Task Graphics_Engine;     //    /
        public static Task Alien_Engine;        //   /      Right now, Alien_Task is only controlling spawning, not behavior
        public static Task Game_Engine;         //  /

        public static Task[] physTasks;         // will contain up to 4 Tasks at a time currently
        public static Task[] collTasks;         // will contain up to 6 Tasks at a time currently
        public static Task[] cleanTasks;        // will contain up to 4 Tasks at a time currently

        public static List<Task> InternalCollTasks;     // used for meteor spawning and such

        public static Task[] SFX;                       // right now only runs four parallel Tasks.
        public static Task scrollingStoryTask;          // only runs on form load... not a big deal.

        /// <summary>
        /// Instantiates new instances of the above tasks to be used
        /// as engines and background threads.  Called anew for each level.
        /// </summary>
        public static void initializeTasks()
        {
            if (physTasks != null)
            {
                Task.WaitAll(physTasks);
                Task.WaitAll(collTasks);
                Task.WaitAll(cleanTasks);
                Task.WaitAll(InternalCollTasks.ToArray());
            }

            SFX = new Task[4];
            physTasks = new Task[4];
            collTasks = new Task[6];
            cleanTasks = new Task[4];
            InternalCollTasks = new List<Task>();
        }
    }
}
