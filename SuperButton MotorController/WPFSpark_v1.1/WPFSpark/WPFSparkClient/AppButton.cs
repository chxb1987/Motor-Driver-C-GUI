using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using WPFSpark;

namespace WPFSparkClient
{
    public class AppButton : Button
    {
        static AppButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppButton), new FrameworkPropertyMetadata(typeof(AppButton)));
        }

        public AppButton()
            : base()
        {
            var behaviors = Interaction.GetBehaviors(this);
            behaviors.Add(new FluidMouseDragBehavior { DragButton = MouseButton.Right });
        }

    }
}
