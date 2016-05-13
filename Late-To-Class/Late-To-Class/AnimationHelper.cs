using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Late_To_Class
{
    /// <summary>
    /// This holds all information needed to play through a looping animation. Each animation will have one
    /// </summary>
    public class AnimationHelper
    {
        #region Fields
        double fps; //speed the animation runs at
        double spf; // 1 / fps, the time it takes for one frame to pass
        int currentFrame; //the current frame of the animation
        int totalFrames; //the total number of frames in the animation
        #endregion

        #region Properties
        public double FPS { get { return fps; } set { fps = value; } }
        public double SPF { get { return spf; } set { spf = value; } }
        public int CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        public int TotalFrames { get { return totalFrames; } set { totalFrames = value; } }
        #endregion


        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fps">how fast the animation runs in frames per second</param>
        /// <param name="totalFrames">the total number of frames in the animation</param>
        public AnimationHelper(int fps, int totalFrames)
        {
            this.fps = fps;
            this.totalFrames = totalFrames;
            spf = 1.0 / fps;
            currentFrame = 0;
        }

        public void UpdateSPF()
        {
            spf = 1 / fps;
        }

        #endregion
    }
}
