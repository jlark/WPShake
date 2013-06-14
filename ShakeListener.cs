using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace capulcuTencere
{


    class ShakeListener
    {
        //Minimum force to responde to 
        private int MIN_FORCE = 2;

        private Stopwatch stopWatch = new Stopwatch();
        /**
         * Minimum times in a shake gesture that the direction of movement needs to
         * change.
         */
        private static int MIN_DIRECTION_CHANGE = 3;

        /** Maximum pause between movements. */
        private static int MAX_PAUSE_BETHWEEN_DIRECTION_CHANGE = 200;

        /** Maximum allowed time for shake gesture. */
        private static int MAX_TOTAL_DURATION_OF_SHAKE = 400;

        /** Time when the gesture started. */
        private double mFirstDirectionChangeTime = 0;

        /** Time when the last movement started. */
        private double mLastDirectionChangeTime;

        /** How many movements are considered so far. */
        private int mDirectionChangeCount = 0;

        /** The last x position. */
        private double lastX = 0;

        /** The last y position. */
        private double lastY = 0;

        /** The last z position. */
        private double lastZ = 0;

        public ShakeListener()
        {
            this.stopWatch.Start();
        }
        /// <summary>
        /// Set the threshold of what is considered a shake.Higher values will make it more tolerant
        /// </summary>
        /// <param name="threshold"></param>
        public void setThreshold(int threshold)
        {
           this.MIN_FORCE = threshold;
        }
        
        /// <summary>
        /// Provides check to see if phone has been shaken
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool Shaken(double x, double y, double z)
        {
            try
            {


                // calculate movement
                double totalMovement = Math.Abs(x + y + z - lastX - lastY - lastZ);

                Debug.WriteLine("accelreation " + totalMovement);

                if (totalMovement > MIN_FORCE)
                {

                    // get time
                    double now = stopWatch.ElapsedMilliseconds;

                    // store first movement time
                    if (mFirstDirectionChangeTime == 0)
                    {
                        mFirstDirectionChangeTime = now;
                        mLastDirectionChangeTime = now;
                    }

                    // check if the last movement was not long ago
                    double lastChangeWasAgo = now - mLastDirectionChangeTime;
                    if (lastChangeWasAgo < MAX_PAUSE_BETHWEEN_DIRECTION_CHANGE)
                    {

                        // store movement data
                        mLastDirectionChangeTime = now;
                        mDirectionChangeCount++;

                        // store last sensor data 
                        lastX = x;
                        lastY = y;
                        lastZ = z;

                        // check how many movements are so far
                        if (mDirectionChangeCount >= MIN_DIRECTION_CHANGE)
                        {

                            // check total duration
                            double totalDuration = now - mFirstDirectionChangeTime;
                            if (totalDuration < MAX_TOTAL_DURATION_OF_SHAKE)
                            {
                                return true;
                            }
                        }

                    }
                    else
                    {
                        Debug.WriteLine("resetting params acc");
                        resetShakeParameters();
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Caught exception in thread " + ex);

            }
            return false;

            }

            /// <summary>
            /// 
            ///reset values for shake
            /// </summary>
            private void resetShakeParameters()
            {
                    this.mFirstDirectionChangeTime = 0;
                    this.mDirectionChangeCount = 0;
                    this.mLastDirectionChangeTime = 0;
                    this.lastX = 0;
                    this.lastY = 0;
                    this.lastZ = 0;
            }
    }


}
