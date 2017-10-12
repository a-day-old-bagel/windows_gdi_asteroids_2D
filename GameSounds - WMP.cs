/**************************************************************************
 * Galen Cochrane 11/20/2014
 * GameSounds.cs
 * Instantiates several copies of the Windows Media Player to play
 * music and sound effects in parallel (on top of each other).
 * previously used SoundPlayer, which made parallel sounds impossible.
 * Inspired by: (This has been completely redone)
 * 
 * Ted Delezene 11/14/2012
 * Sounds.cs 
 * This class contains the methods to play sounds for various events.
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Asteroids2._0
{
    static class GameSounds
    {
        #region Fields

        static public WMPLib.WindowsMediaPlayer musicPlayer;
        static bool noMoreSounds;
        static string[] queue = { null, null, null, null };
        static int nextInQueue = 0;

        #endregion

        #region Methods

        /// <summary>
        /// instantiates the music player
        /// </summary>
        static public void initializeMusic()
        {
            musicPlayer = new WMPLib.WindowsMediaPlayer();
            noMoreSounds = false;
        }        
        /// <summary>
        /// starts the background music playing
        /// </summary>
        static public void backGroundLoop()
        {
            if (!noMoreSounds)
            {
                musicPlayer.URL = GameMedia.getDir["backgroundSND"];
                musicPlayer.settings.setMode("loop", true);
            }
        }
        /// <summary>
        /// plays the happy win music
        /// </summary>
        static public void credits()
        {
            if (!noMoreSounds)
            {
                musicPlayer.URL = GameMedia.getDir["winSongSND"];
            }
        }
        /// <summary>
        /// plays the anxious lose music
        /// </summary>
        static public void loser()
        {
            if (!noMoreSounds)
            {
                musicPlayer.URL = GameMedia.getDir["loseSongSND"];
            }
        }
        /// <summary>
        /// plays a sound effect
        /// </summary>
        /// <param name="name">key to the gameMedia file path dictionary</param>
        static public void effect(string name)
        {
            if (Settings.soundIsOn)
            {
                if (!noMoreSounds)
                {
                    queue[nextInQueue] = name;
                    nextInQueue++;
                    nextInQueue %= Settings.numSFXthreads;
                }
            }
        }
        /// <summary>
        /// closes the SFX waiting loops
        /// </summary>
        static public void CloseSFX()
        {
            noMoreSounds = true;
        }
        /// <summary>
        /// closes the music player
        /// </summary>
        static public void CloseMusic()
        {
            musicPlayer.close();
        }
        /// <summary>
        /// begins the SFX waiting loops
        /// </summary>
        static public void InitializeSFX()
        {
            noMoreSounds = false;
            Threads.SFX[0] = Task.Run(() => soundWaitLoop0());
            Threads.SFX[1] = Task.Run(() => soundWaitLoop1());
            Threads.SFX[2] = Task.Run(() => soundWaitLoop2());
            Threads.SFX[3] = Task.Run(() => soundWaitLoop3());
        }

        #endregion

        #region SFX loops

        /// <summary>
        /// The four sound loops that wait for queue to change (waiting for sound effects to play)
        /// </summary>
            static void soundWaitLoop0()
            {
                WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                while (true)
                    if (queue[0] != null)
                    {
                        if (noMoreSounds)
                        {
                            player.close();
                            return;
                        }
                        else
                            try
                            {
                                player.URL = queue[0];
                                queue[0] = null;
                            }
                            catch { }
                    }
            }
            static void soundWaitLoop1()
            {
                WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                while (true)
                    if (queue[1] != null)
                    {
                        if (noMoreSounds)
                        {
                            player.close();
                            return;
                        }
                        else
                            try
                            {
                                player.URL = queue[1];
                                queue[1] = null;
                            }
                            catch { }
                    }
            }
            static void soundWaitLoop2()
            {
                WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                while (true)
                    if (queue[2] != null)
                    {
                        if (noMoreSounds)
                        {
                            player.close();
                            return;
                        }
                        else
                            try
                            {
                                player.URL = queue[2];
                                queue[2] = null;
                            }
                            catch { }
                    }
            }
            static void soundWaitLoop3()
        {
            WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            while (true)
                if (queue[3] != null)
                {
                    if (noMoreSounds)
                    {
                        player.close();
                        return;
                    }
                    else
                        try
                        {
                            player.URL = queue[3];
                            queue[3] = null;
                        }
                        catch { }
                }
        }

        #endregion
    }
}
