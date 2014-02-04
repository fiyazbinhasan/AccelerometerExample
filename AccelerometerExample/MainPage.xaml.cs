using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
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
        private Vector3 _acceleration;
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Accelerometer.IsSupported)
            {
                if (accelerometer == null)
                {
                    accelerometer = new Accelerometer();
                    accelerometer.Start();
                    accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                    accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                }
                else
                {
                    accelerometer.Start();
                    accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                    accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                }
            }

        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {

            // Call UpdateUI on the UI thread and pass the AccelerometerReading.
            if (e.SensorReading.Acceleration.X <= -0.90)
            {
                cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Show();
                accelerometer.Stop();
                cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            }
            if(e.SensorReading.Acceleration.X >= 0.90)
            {
                cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Show();
                accelerometer.Stop();
                cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            }
            Dispatcher.BeginInvoke(() => UpdateTextBoxes(e.SensorReading.Acceleration));
        }

        private void UpdateTextBoxes(Vector3 acceleration)
        {
            TextBlock.Text = acceleration.X.ToString();
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessagePrompt prompt = new MessagePrompt();
                prompt.Message = "Photo saved in " + e.OriginalFileName;
                prompt.HorizontalAlignment = HorizontalAlignment.Center;
                prompt.VerticalAlignment = VerticalAlignment.Center;
                prompt.Show();
            }
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.LandscapeLeft) == PageOrientation.LandscapeLeft)
            {
                accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
            }
            else if ((e.Orientation & PageOrientation.LandscapeRight) == PageOrientation.LandscapeRight)
            {
                accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
            }
            else
            {
                accelerometer.Start();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
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