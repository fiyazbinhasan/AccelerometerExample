using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MovingBallAccelerometer.Resources;

namespace MovingBallAccelerometer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private Accelerometer _accelerometer;
        public MainPage()
        {
            InitializeComponent();
            if (_accelerometer == null)
            {
                _accelerometer = new Accelerometer();
                _accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                _accelerometer.Start();
            }
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateBallPosition(e.SensorReading));
        }

        private void UpdateBallPosition(AccelerometerReading e)
        {
            double distanceToTravel = 2;
            double accelerationFactor = Math.Abs(e.Acceleration.Z) == 0 ? 0.1 : Math.Abs(e.Acceleration.Z);
            double ballX = (double) Ball.GetValue(Canvas.LeftProperty) +
                           distanceToTravel*e.Acceleration.X/accelerationFactor;
            double ballY = (double) Ball.GetValue(Canvas.TopProperty) -
                           distanceToTravel*e.Acceleration.Y/accelerationFactor;

            if (ballX < 0)
            {
                ballX = 0;
            }
            else if (ballX > ContentGrid.Width)
            {
                ballX = ContentGrid.Width;
            }

            if (ballY < 0)
            {
                ballY = 0;
            }
            else if (ballY > ContentGrid.Height)
            {
                ballY = ContentGrid.Height;
            }

            Ball.SetValue(Canvas.LeftProperty, ballX);
            Ball.SetValue(Canvas.TopProperty, ballY);
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}