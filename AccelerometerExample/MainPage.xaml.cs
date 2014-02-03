using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AccelerometerExample.Resources;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;

namespace AccelerometerExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        Accelerometer accelerometer;
        CameraCaptureTask cameraCaptureTask;
        public MainPage()
        {
            InitializeComponent();
            if (!Accelerometer.IsSupported)
            {
                // The device on which the application is running does not support
                // the accelerometer sensor. Alert the user and disable the
                // Start and Stop buttons.
                statusTextBlock.Text = "device does not support accelerometer";
            }
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (accelerometer == null)
            {
                // Instantiate the Accelerometer.
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged +=
                    accelerometer_CurrentValueChanged;
            }
            try
            {
                statusTextBlock.Text = "starting accelerometer.";
                accelerometer.Start();
            }
            catch (InvalidOperationException ex)
            {
                statusTextBlock.Text = "unable to start accelerometer.";
            }
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            // Call UpdateUI on the UI thread and pass the AccelerometerReading.
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
        }

        private void UpdateUI(AccelerometerReading accelerometerReading)
        {
            statusTextBlock.Text = "getting data";

            Vector3 acceleration = accelerometerReading.Acceleration;

            // Show the numeric values.
            xTextBlock.Text = "X: " + acceleration.X.ToString("0.00");
            yTextBlock.Text = "Y: " + acceleration.Y.ToString("0.00");
            zTextBlock.Text = "Z: " + acceleration.Z.ToString("0.00");


            //// Show the values graphically.
            //xLine.X2 = xLine.X1 + acceleration.X * 200;
            //yLine.Y2 = yLine.Y1 - acceleration.Y * 200;
            //zLine.X2 = zLine.X1 - acceleration.Z * 100;
            //zLine.Y2 = zLine.Y1 + acceleration.Z * 100;

            if (acceleration.Y.ToString("0.00") == "-1.00")
            {
                accelerometer.CurrentValueChanged -= accelerometer_CurrentValueChanged;
                cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Show();
                cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            }
            else if (acceleration.Y >= -1.00)
            {
                Map.Visibility = Visibility.Visible;
            }
            else
            {
                Map.Visibility = Visibility.Collapsed;
            }
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show(e.ChosenPhoto.Length.ToString());
                accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;

                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
            }
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