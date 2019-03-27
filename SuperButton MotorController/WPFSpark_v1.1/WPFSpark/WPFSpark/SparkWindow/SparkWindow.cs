#region File Header

// -------------------------------------------------------------------------------
// 
// This file is part of the WPFSpark project: http://wpfspark.codeplex.com/
//
// Author: Ratish Philip
// 
// WPFSpark v1.1
//
// -------------------------------------------------------------------------------

#endregion

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace WPFSpark
{
    /// <summary>
    /// Class which provides the implementation of a custom window
    /// </summary>
    [TemplatePart(Name = "PART_About", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Minimize", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Close", Type = typeof(Button))]
    public class SparkWindow : Window
    {
        #region Fields

        Button aboutButton = null;
        Button minimizeButton = null;
        Button maximizeButton = null;
        Button closeButton = null;
        Border titleBar = null;

        #endregion

        #region Dependency Properties

        #region TitleMargin

        /// <summary>
        /// TitleMargin Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleMarginProperty =
            DependencyProperty.Register("TitleMargin", typeof(Thickness), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTitleMarginChanged)));

        /// <summary>
        /// Gets or sets the TitleMargin property. This dependency property 
        /// indicates the margin of the title bar.
        /// </summary>
        public Thickness TitleMargin
        {
            get { return (Thickness)GetValue(TitleMarginProperty); }
            set { SetValue(TitleMarginProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TitleMargin property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTitleMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Thickness oldTitleMargin = (Thickness)e.OldValue;
            Thickness newTitleMargin = win.TitleMargin;
            win.OnTitleMarginChanged(oldTitleMargin, newTitleMargin);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TitleMargin property.
        /// </summary>
        /// <param name="oldTitleMargin">Old Value</param>
        /// <param name="margin">New Value</param>
        protected virtual void OnTitleMarginChanged(Thickness oldTitleMargin, Thickness newTitleMargin)
        {
            TextBlock tb = this.GetChildControl<TextBlock>("PART_TitleText");
            if (tb == null)
                return;

            UpdateTriggerMargin(tb, newTitleMargin);
        }

        #endregion

        #region TitleEffect

        /// <summary>
        /// TitleEffect Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleEffectProperty =
            DependencyProperty.Register("TitleEffect", typeof(Effect), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTitleEffectChanged)));

        /// <summary>
        /// Gets or sets the TitleEffect property. This dependency property 
        /// indicates the Effect to be applied on the Title TextBlock.
        /// </summary>
        public Effect TitleEffect
        {
            get { return (Effect)GetValue(TitleEffectProperty); }
            set { SetValue(TitleEffectProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TitleEffect property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTitleEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Effect oldTitleEffect = (Effect)e.OldValue;
            Effect newTitleEffect = win.TitleEffect;
            win.OnTitleEffectChanged(oldTitleEffect, newTitleEffect);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TitleEffect property.
        /// </summary>
        /// <param name="oldTitleEffect">Old Value</param>
        /// <param name="newTitleEffect">New Value</param>
        protected void OnTitleEffectChanged(Effect oldTitleEffect, Effect newTitleEffect)
        {

        }

        #endregion

        #region WindowFrameMode

        /// <summary>
        /// WindowFrameMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty WindowFrameModeProperty =
            DependencyProperty.Register("WindowFrameMode", typeof(WindowMode), typeof(SparkWindow),
                new FrameworkPropertyMetadata(WindowMode.Pane,
                    new PropertyChangedCallback(OnWindowFrameModeChanged)));

        /// <summary>
        /// Gets or sets the WindowFrameMode property. This dependency property 
        /// indicates the mode of the window frame.
        /// </summary>
        public WindowMode WindowFrameMode
        {
            get { return (WindowMode)GetValue(WindowFrameModeProperty); }
            set { SetValue(WindowFrameModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the WindowFrameMode property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnWindowFrameModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            WindowMode oldWindowFrameMode = (WindowMode)e.OldValue;
            WindowMode newWindowFrameMode = win.WindowFrameMode;
            win.OnWindowFrameModeChanged(oldWindowFrameMode, newWindowFrameMode);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the WindowFrameMode property.
        /// </summary>
        /// <param name="oldWindowFrameMode">Old Value</param>
        /// <param name="newWindowFrameMode">New Value</param>
        protected void OnWindowFrameModeChanged(WindowMode oldWindowFrameMode, WindowMode newWindowFrameMode)
        {
            UpdateWindowFrame(newWindowFrameMode);
        }

        #endregion

        #region IsAboutEnabled

        /// <summary>
        /// IsAboutEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsAboutEnabledProperty =
            DependencyProperty.Register("IsAboutEnabled", typeof(bool), typeof(SparkWindow),
                new FrameworkPropertyMetadata(true,
                    new PropertyChangedCallback(OnIsAboutEnabledChanged)));

        /// <summary>
        /// Gets or sets the IsAboutEnabled property. This dependency property 
        /// indicates whether the About button should be displayed or not.
        /// </summary>
        public bool IsAboutEnabled
        {
            get { return (bool)GetValue(IsAboutEnabledProperty); }
            set { SetValue(IsAboutEnabledProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsAboutEnabled property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnIsAboutEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            bool oldIsAboutEnabled = (bool)e.OldValue;
            bool newIsAboutEnabled = win.IsAboutEnabled;
            win.OnIsAboutEnabledChanged(oldIsAboutEnabled, newIsAboutEnabled);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsAboutEnabled property.
        /// </summary>
        /// <param name="oldIsAboutEnabled">Old Value</param>
        /// <param name="newIsAboutEnabled">New Value</param>
        protected void OnIsAboutEnabledChanged(bool oldIsAboutEnabled, bool newIsAboutEnabled)
        {
            UpdateAboutButton(newIsAboutEnabled);
        }

        #endregion

        #region OuterBorderThickness

        /// <summary>
        /// OuterBorderThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty OuterBorderThicknessProperty =
            DependencyProperty.Register("OuterBorderThickness", typeof(Thickness), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new Thickness(),
                    new PropertyChangedCallback(OnOuterBorderThicknessChanged)));

        /// <summary>
        /// Gets or sets the OuterBorderThickness property. This dependency property 
        /// indicates the thickness of the outer border.
        /// </summary>
        public Thickness OuterBorderThickness
        {
            get { return (Thickness)GetValue(OuterBorderThicknessProperty); }
            set { SetValue(OuterBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the OuterBorderThickness property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnOuterBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Thickness oldOuterBorderThickness = (Thickness)e.OldValue;
            Thickness newOuterBorderThickness = win.OuterBorderThickness;
            win.OnOuterBorderThicknessChanged(oldOuterBorderThickness, newOuterBorderThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the OuterBorderThickness property.
        /// </summary>
        /// <param name="oldOuterBorderThickness">Old Value</param>
        /// <param name="newOuterBorderThickness">New Value</param>
        protected void OnOuterBorderThicknessChanged(Thickness oldOuterBorderThickness, Thickness newOuterBorderThickness)
        {

        }

        #endregion

        #region OuterBorderBrush

        /// <summary>
        /// OuterBorderBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty OuterBorderBrushProperty =
            DependencyProperty.Register("OuterBorderBrush", typeof(Brush), typeof(SparkWindow),
                new FrameworkPropertyMetadata(Brushes.Black,
                    new PropertyChangedCallback(OnOuterBorderBrushChanged)));

        /// <summary>
        /// Gets or sets the OuterBorderBrush property. This dependency property 
        /// indicates the color of the outer border.
        /// </summary>
        public Brush OuterBorderBrush
        {
            get { return (Brush)GetValue(OuterBorderBrushProperty); }
            set { SetValue(OuterBorderBrushProperty, value); }
        }

        /// <summary>
        /// Handles changes to the OuterBorderBrush property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnOuterBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Brush oldOuterBorderBrush = (Brush)e.OldValue;
            Brush newOuterBorderBrush = win.OuterBorderBrush;
            win.OnOuterBorderBrushChanged(oldOuterBorderBrush, newOuterBorderBrush);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the OuterBorderBrush property.
        /// </summary>
        /// <param name="oldOuterBorderBrush">Old Value</param>
        /// <param name="newOuterBorderBrush">New Value</param>
        protected void OnOuterBorderBrushChanged(Brush oldOuterBorderBrush, Brush newOuterBorderBrush)
        {

        }

        #endregion

        #region OuterBorderCornerRadius

        /// <summary>
        /// OuterBorderCornerRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty OuterBorderCornerRadiusProperty =
            DependencyProperty.Register("OuterBorderCornerRadius", typeof(CornerRadius), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new CornerRadius(),
                    new PropertyChangedCallback(OnOuterBorderCornerRadiusChanged)));

        /// <summary>
        /// Gets or sets the OuterBorderCornerRadius property. This dependency property 
        /// indicates the corner radius of the outer border.
        /// </summary>
        public CornerRadius OuterBorderCornerRadius
        {
            get { return (CornerRadius)GetValue(OuterBorderCornerRadiusProperty); }
            set { SetValue(OuterBorderCornerRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the OuterBorderCornerRadius property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnOuterBorderCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            CornerRadius oldOuterBorderCornerRadius = (CornerRadius)e.OldValue;
            CornerRadius newOuterBorderCornerRadius = win.OuterBorderCornerRadius;
            win.OnOuterBorderCornerRadiusChanged(oldOuterBorderCornerRadius, newOuterBorderCornerRadius);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the OuterBorderCornerRadius property.
        /// </summary>
        /// <param name="oldOuterBorderCornerRadius">Old Value</param>
        /// <param name="newOuterBorderCornerRadius">New Value</param>
        protected void OnOuterBorderCornerRadiusChanged(CornerRadius oldOuterBorderCornerRadius, CornerRadius newOuterBorderCornerRadius)
        {

        }

        #endregion

        #region InnerBorderThickness

        /// <summary>
        /// InnerBorderThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty InnerBorderThicknessProperty =
            DependencyProperty.Register("InnerBorderThickness", typeof(Thickness), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new Thickness(),
                    new PropertyChangedCallback(OnInnerBorderThicknessChanged)));

        /// <summary>
        /// Gets or sets the InnerBorderThickness property. This dependency property 
        /// indicates the thickness of the inner border.
        /// </summary>
        public Thickness InnerBorderThickness
        {
            get { return (Thickness)GetValue(InnerBorderThicknessProperty); }
            set { SetValue(InnerBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the InnerBorderThickness property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnInnerBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Thickness oldInnerBorderThickness = (Thickness)e.OldValue;
            Thickness newInnerBorderThickness = win.InnerBorderThickness;
            win.OnInnerBorderThicknessChanged(oldInnerBorderThickness, newInnerBorderThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the InnerBorderThickness property.
        /// </summary>
        /// <param name="oldInnerBorderThickness">Old Value</param>
        /// <param name="newInnerBorderThickness">New Value</param>
        protected void OnInnerBorderThicknessChanged(Thickness oldInnerBorderThickness, Thickness newInnerBorderThickness)
        {

        }

        #endregion

        #region InnerBorderBrush

        /// <summary>
        /// InnerBorderBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty InnerBorderBrushProperty =
            DependencyProperty.Register("InnerBorderBrush", typeof(Brush), typeof(SparkWindow),
                new FrameworkPropertyMetadata(Brushes.White,
                    new PropertyChangedCallback(OnInnerBorderBrushChanged)));

        /// <summary>
        /// Gets or sets the InnerBorderBrush property. This dependency property 
        /// indicates the color of the inner border.
        /// </summary>
        public Brush InnerBorderBrush
        {
            get { return (Brush)GetValue(InnerBorderBrushProperty); }
            set { SetValue(InnerBorderBrushProperty, value); }
        }

        /// <summary>
        /// Handles changes to the InnerBorderBrush property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnInnerBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            Brush oldInnerBorderBrush = (Brush)e.OldValue;
            Brush newInnerBorderBrush = win.InnerBorderBrush;
            win.OnInnerBorderBrushChanged(oldInnerBorderBrush, newInnerBorderBrush);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the InnerBorderBrush property.
        /// </summary>
        /// <param name="oldInnerBorderBrush">Old Value</param>
        /// <param name="newInnerBorderBrush">New Value</param>
        protected void OnInnerBorderBrushChanged(Brush oldInnerBorderBrush, Brush newInnerBorderBrush)
        {

        }

        #endregion

        #region InnerBorderCornerRadius

        /// <summary>
        /// InnerBorderCornerRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty InnerBorderCornerRadiusProperty =
            DependencyProperty.Register("InnerBorderCornerRadius", typeof(CornerRadius), typeof(SparkWindow),
                new FrameworkPropertyMetadata(new CornerRadius(),
                    new PropertyChangedCallback(OnInnerBorderCornerRadiusChanged)));

        /// <summary>
        /// Gets or sets the InnerBorderCornerRadius property. This dependency property 
        /// indicates the corner radius of the inner border.
        /// </summary>
        public CornerRadius InnerBorderCornerRadius
        {
            get { return (CornerRadius)GetValue(InnerBorderCornerRadiusProperty); }
            set { SetValue(InnerBorderCornerRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the InnerBorderCornerRadius property.
        /// </summary>
        /// <param name="d">SparkWindow</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnInnerBorderCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkWindow win = (SparkWindow)d;
            CornerRadius oldInnerBorderCornerRadius = (CornerRadius)e.OldValue;
            CornerRadius newInnerBorderCornerRadius = win.InnerBorderCornerRadius;
            win.OnInnerBorderCornerRadiusChanged(oldInnerBorderCornerRadius, newInnerBorderCornerRadius);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the InnerBorderCornerRadius property.
        /// </summary>
        /// <param name="oldInnerBorderCornerRadius">Old Value</param>
        /// <param name="newInnerBorderCornerRadius">New Value</param>
        protected void OnInnerBorderCornerRadiusChanged(CornerRadius oldInnerBorderCornerRadius, CornerRadius newInnerBorderCornerRadius)
        {

        }

        #endregion

        #endregion

        #region Construction / Initialization

        /// <summary>
        /// Static ctor
        /// </summary>
        static SparkWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SparkWindow), new FrameworkPropertyMetadata(typeof(SparkWindow)));
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public SparkWindow()
        {
            // Default WindowFrameMode is Pane
            this.WindowFrameMode = WindowMode.Pane;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Override which is called when the template is applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Detach previously attached event handlers, if any
            Unsubscribe();

            // Get all the controls in the template
            GetTemplateParts();
        }

        /// <summary>
        /// Handles the closing event
        /// </summary>
        /// <param name="e">CancelEventArgs</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            // Unsubscribe to events
            if (aboutButton != null)
                aboutButton.Click -= OnAbout;

            if (minimizeButton != null)
                minimizeButton.Click -= OnMinimize;

            if (maximizeButton != null)
                maximizeButton.Click -= OnMaximize;

            if (closeButton != null)
                closeButton.Click -= OnClose;

            if (titleBar != null)
                titleBar.MouseLeftButtonDown -= OnTitleBarMouseDown;

            base.OnClosing(e);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Detach previously attached event handlers, if any
        /// </summary>
        private void Unsubscribe()
        {
            // PART_About
            if (aboutButton != null)
            {
                aboutButton.Click -= OnAbout;
            }

            // PART_Minimize
            if (minimizeButton != null)
            {
                minimizeButton.Click -= OnMinimize;
            }

            // PART_Maximize
            if (maximizeButton != null)
            {
                maximizeButton.Click -= OnMaximize;
            }

            // PART_Close
            if (closeButton != null)
            {
                closeButton.Click -= OnClose;
            }

            // PART_TitleBar
            if (titleBar != null)
            {
                titleBar.MouseLeftButtonDown -= OnTitleBarMouseDown;
            }
        }

        /// <summary>
        /// Gets the required controls in the template
        /// </summary>
        protected void GetTemplateParts()
        {
            // PART_About
            aboutButton = GetChildControl<Button>("PART_About");
            if (aboutButton != null)
            {
                aboutButton.Click += new RoutedEventHandler(OnAbout);
            }

            // PART_Minimize
            minimizeButton = GetChildControl<Button>("PART_Minimize");
            if (minimizeButton != null)
            {
                minimizeButton.Click += new RoutedEventHandler(OnMinimize);
            }

            // PART_Maximize
            maximizeButton = GetChildControl<Button>("PART_Maximize");
            if (maximizeButton != null)
            {
                maximizeButton.Click += new RoutedEventHandler(OnMaximize);
            }

            // PART_Close
            closeButton = GetChildControl<Button>("PART_Close");
            if (closeButton != null)
            {
                closeButton.Click += new RoutedEventHandler(OnClose);
            }

            // PART_TitleBar
            titleBar = GetChildControl<Border>("PART_TitleBar");
            if (titleBar != null)
            {
                titleBar.MouseLeftButtonDown += new MouseButtonEventHandler(OnTitleBarMouseDown);
            }

            // PART_TitleText
            TextBlock tb = this.GetChildControl<TextBlock>("PART_TitleText");
            if (tb == null)
                return;

            // Update the margin for the TitleText trigger
            UpdateTriggerMargin(tb, TitleMargin);
            // Update the system control buttons in the window frame
            UpdateWindowFrame(WindowFrameMode);
            // Update the location of the About button
            UpdateAboutButton(IsAboutEnabled);
        }

        /// <summary>
        /// Update the system control buttons in the window frame
        /// </summary>
        /// <param name="winMode">Window mode</param>
        private void UpdateWindowFrame(WindowMode winMode)
        {
            switch (winMode)
            {
                // Only close button should be visible if the mode is CanClose/PaneCanClose
                case WindowMode.CanClose:
                case WindowMode.PaneCanClose:
                    if (minimizeButton != null)
                        minimizeButton.Visibility = Visibility.Collapsed;
                    if (maximizeButton != null)
                        maximizeButton.Visibility = Visibility.Collapsed;
                    break;

                // Only minimize and close buttons should be visible if the mode is Pane/CanMinimize
                case WindowMode.Pane:
                case WindowMode.CanMinimize:
                default:
                    if (minimizeButton != null)
                    {
                        minimizeButton.Visibility = Visibility.Visible;
                        Grid.SetColumn(minimizeButton, 3);
                    }
                    if (maximizeButton != null)
                        maximizeButton.Visibility = Visibility.Collapsed;
                    break;

                // All buttons - minimize, maximize and close will be visible if the mode is CanMaximize
                case WindowMode.CanMaximize:
                    if (minimizeButton != null)
                    {
                        minimizeButton.Visibility = Visibility.Visible;
                        Grid.SetColumn(minimizeButton, 2);
                    }
                    if (maximizeButton != null)
                    {
                        maximizeButton.Visibility = Visibility.Visible;
                    }
                    break;
            }

            // If the mode is Pane/PaneCanClose then the window should be in maximized state
            if ((WindowFrameMode == WindowMode.Pane) || (WindowFrameMode == WindowMode.PaneCanClose))
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// Updates the location and visibility of the About button
        /// </summary>
        /// <param name="isEnabled"></param>
        private void UpdateAboutButton(bool isEnabled)
        {
            if (aboutButton == null)
                return;

            if (IsAboutEnabled)
            {
                // Show the About button
                aboutButton.Visibility = Visibility.Visible;

                // Set the location of the about button based 
                // on the visibility of the minimize and maximize buttons
                switch (WindowFrameMode)
                {
                    case WindowMode.CanClose:
                    case WindowMode.PaneCanClose:
                        Grid.SetColumn(aboutButton, 3);
                        break;
                    case WindowMode.Pane:
                    case WindowMode.CanMinimize:
                        Grid.SetColumn(aboutButton, 2);
                        break;
                    case WindowMode.CanMaximize:
                    default:
                        Grid.SetColumn(aboutButton, 1);
                        break;
                }
            }
            else
            {
                // Hide the About button
                aboutButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Generic method to get a control from the template
        /// </summary>
        /// <typeparam name="T">Type of the control</typeparam>
        /// <param name="ctrlName">Name of the control in the template</param>
        /// <returns>Control</returns>
        protected T GetChildControl<T>(string ctrlName) where T : DependencyObject
        {
            T ctrl = GetTemplateChild(ctrlName) as T;
            return ctrl;
        }

        /// <summary>
        /// Updates the margin used for animating the Window Title when the window loads.
        /// </summary>
        /// <param name="tb">TextBlock which displays the window title</param>
        /// <param name="margin">New Margin</param>
        private void UpdateTriggerMargin(TextBlock tb, Thickness margin)
        {
            if ((tb.Triggers == null) || (tb.Triggers.Count == 0))
                return;

            foreach (EventTrigger trigger in tb.Triggers)
            {
                if (trigger.RoutedEvent.Name == "TargetUpdated")
                {
                    if ((trigger.Actions != null) && (trigger.Actions.Count > 0))
                    {
                        BeginStoryboard bsb = trigger.Actions[0] as BeginStoryboard;
                        if (bsb != null)
                        {
                            foreach (Timeline timeLine in bsb.Storyboard.Children)
                            {
                                ThicknessAnimation anim = timeLine as ThicknessAnimation;
                                if (anim != null)
                                {
                                    Thickness startThickness = new Thickness(margin.Left + 200, margin.Top, margin.Right, margin.Bottom);
                                    anim.SetValue(ThicknessAnimation.FromProperty, startThickness);
                                    anim.SetValue(ThicknessAnimation.ToProperty, margin);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the state of the window to maximized if the state is normal else vice-versa
        /// </summary>
        private void ToggleMaximize()
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the MouseDown event on the title bar.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">MouseButtonEventArgs</param>
        void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            // If the user has clicked on the title bar twice then toggle the 
            // state of the window (if window is maximizable)
            if (WindowFrameMode == WindowMode.CanMaximize && e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            // Allow the user to drag the window to a new location
            this.DragMove();
        }

        /// <summary>
        /// Overridable event handler for the event raised when About button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnAbout(object sender, RoutedEventArgs e)
        {
            // Do nothing here
            // Derived classes should override this method and handle it themselves.
        }

        /// <summary>
        /// Overridable event handler for the event raised when Minimize button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Overridable event handler for the event raised when Maximize button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnMaximize(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        /// <summary>
        /// Overridable event handler for the event raised when Close button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}
