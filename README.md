WPShake
=======
I was suprised that a built in shake event handler didn't exist. We can see through MSDN that they put it for Windows8 but this feature doesn't work for windows phone.

Windows Phone Shake Listener:
Simple to use shake listener simply bind to an existing accelerometer event listener.

//initialize listener
        private ShakeListener mShakeListener = new ShakeListener();

//bind it to ReadingChanged

            var accelerometer = Accelerometer.GetDefault();
            if (accelerometer != null)
            {

                accelerometer.ReadingChanged += Shaken; 
            }

// check that it it's good to go.
        private void Shaken(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
             AccelerometerReading red =  sender.GetCurrentReading();

             double x = red.AccelerationX;
             double y = red.AccelerationY;
             double z = red.AccelerationZ;
             Debug.WriteLine("shaken x : " + x);
             if (this.mShakeListener.Shaken(x, y, z))
             {
                 Debug.WriteLine("we have shaken");
             }
           