using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;

namespace WPFSparkClient
{
    /// <summary>
    /// Interaction logic for FluidWrapPanelDemo.xaml
    /// </summary>
    public partial class FluidWrapPanelDemo : Window
    {
        #region Images

        /// <summary>
        /// Images Dependency Property
        /// </summary>
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(List<string>), typeof(FluidWrapPanelDemo),
                new FrameworkPropertyMetadata((List<string>)null));

        public List<string> Images
        {
            get { return (List<string>)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }

        #endregion

        ObservableCollection<UIElement> source1 = new ObservableCollection<UIElement>();

        public FluidWrapPanelDemo()
        {
            InitializeComponent();

            //this.DataContext = this;

            //List<string> localImages = new List<string>();
            //localImages.Add("/Resources/Images/Icons/Applications.png");
            //localImages.Add("/Resources/Images/Icons/browser.png");
            //localImages.Add("/Resources/Images/Icons/cal.png");
            //localImages.Add("/Resources/Images/Icons/calc.png");
            //localImages.Add("/Resources/Images/Icons/clock.png");
            //localImages.Add("/Resources/Images/Icons/Downloads.png");
            //localImages.Add("/Resources/Images/Icons/Facebook.png");
            //localImages.Add("/Resources/Images/Icons/graph.png");
            //localImages.Add("/Resources/Images/Icons/ipod.png");
            //localImages.Add("/Resources/Images/Icons/mail.png");
            //localImages.Add("/Resources/Images/Icons/map.png");
            //localImages.Add("/Resources/Images/Icons/notes.png");
            //localImages.Add("/Resources/Images/Icons/phone.png");
            //localImages.Add("/Resources/Images/Icons/photo.png");
            //localImages.Add("/Resources/Images/Icons/Skype.png");
            //localImages.Add("/Resources/Images/Icons/SMS.png");
            //localImages.Add("/Resources/Images/Icons/Terminal.png");
            //localImages.Add("/Resources/Images/Icons/tools.png");
            //localImages.Add("/Resources/Images/Icons/twitter.png");
            //localImages.Add("/Resources/Images/Icons/wallpaper.png");
            //localImages.Add("/Resources/Images/Icons/weather.png");
            //this.Images = localImages;


            this.Loaded += new RoutedEventHandler(FluidWrapPanelDemo_Loaded);
        }

        void FluidWrapPanelDemo_Loaded(object sender, RoutedEventArgs e)
        {
            ImageIcon imgIcon;
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/Applications.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/browser.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/cal.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/calc.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/clock.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/Downloads.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/Facebook.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/graph.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/ipod.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/mail.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/map.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/notes.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/phone.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/photo.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/Skype.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/SMS.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/Terminal.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/tools.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/twitter.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/wallpaper.png" };
            source1.Add(imgIcon);
            imgIcon = new ImageIcon() { Width = 65, Height = 65, ImagePath = "/Resources/Images/Icons/weather.png" };
            source1.Add(imgIcon);

            fluidWrapPanel.ItemsSource = source1;
        }
    }
}
