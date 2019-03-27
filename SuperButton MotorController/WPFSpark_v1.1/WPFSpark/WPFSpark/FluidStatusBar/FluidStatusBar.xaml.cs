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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WPFSpark
{
    /// <summary>
    /// Interaction logic for FluidStatusBar.xaml
    /// </summary>
    public partial class FluidStatusBar : UserControl
    {
        #region Fields

        Storyboard fadeInOutSB = null;
        Storyboard fadeInSB = null;

        Storyboard fadeOutLeftSB = null;
        Storyboard fadeOutRightSB = null;
        Storyboard fadeOutUpSB = null;
        Storyboard fadeOutDownSB = null;

        Queue<StatusMessage> messageQueue = null;
        bool isAnimationInProgress = false;

        #endregion

        #region DependencyProperties

        #region FadeOutDirection

        /// <summary>
        /// FadeOutDirection Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeOutDirectionProperty =
            DependencyProperty.Register("FadeOutDirection", typeof(StatusDirection), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(StatusDirection.Left, new PropertyChangedCallback(OnFadeOutDirectionChanged)));

        /// <summary>
        /// Gets or sets the FadeOutDirection property. This dependency property 
        /// indicates the direction in which the old text should fade out.
        /// </summary>
        public StatusDirection FadeOutDirection
        {
            get { return (StatusDirection)GetValue(FadeOutDirectionProperty); }
            set { SetValue(FadeOutDirectionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the FadeOutDirection property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnFadeOutDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            StatusDirection oldFadeOutDirection = (StatusDirection)e.OldValue;
            StatusDirection newFadeOutDirection = fsBar.FadeOutDirection;
            fsBar.OnFadeOutDirectionChanged(oldFadeOutDirection, newFadeOutDirection);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FadeOutDirection property.
        /// </summary>
        /// <param name="oldFadeOutDirection">Old Value</param>
        /// <param name="newFadeOutDirection">New Value</param>
        protected virtual void OnFadeOutDirectionChanged(StatusDirection oldFadeOutDirection, StatusDirection newFadeOutDirection)
        {
            switch (newFadeOutDirection)
            {
                case StatusDirection.Right:
                    fadeInOutSB = fadeOutRightSB;
                    break;
                case StatusDirection.Up:
                    fadeInOutSB = fadeOutUpSB;
                    break;
                case StatusDirection.Down:
                    fadeInOutSB = fadeOutDownSB;
                    break;
                case StatusDirection.Left:
                default:
                    fadeInOutSB = fadeOutLeftSB;
                    break;
            }
        }

        #endregion

        #region FadeOutDistance

        /// <summary>
        /// FadeOutDistance Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeOutDistanceProperty =
            DependencyProperty.Register("FadeOutDistance", typeof(double), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnFadeOutDistanceChanged)));

        /// <summary>
        /// Gets or sets the FadeOutDistance property. This dependency property 
        /// indicates the width of the fade out animation.
        /// </summary>
        public double FadeOutDistance
        {
            get { return (double)GetValue(FadeOutDistanceProperty); }
            set { SetValue(FadeOutDistanceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the FadeOutDistance property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnFadeOutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            double oldFadeOutDistance = (double)e.OldValue;
            double newFadeOutDistance = fsBar.FadeOutDistance;
            fsBar.OnFadeOutDistanceChanged(oldFadeOutDistance, newFadeOutDistance);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FadeOutDistance property.
        /// </summary>
        /// <param name="oldFadeOutDistance">Old Value</param>
        /// <param name="newFadeOutDistance">New Value</param>
        protected virtual void OnFadeOutDistanceChanged(double oldFadeOutDistance, double newFadeOutDistance)
        {
            UpdateFadeOutDistance(fadeOutLeftSB, new Thickness(0, 0, newFadeOutDistance, 0));
            UpdateFadeOutDistance(fadeOutUpSB, new Thickness(0, 0, 0, newFadeOutDistance));
            UpdateFadeOutDistance(fadeOutRightSB, new Thickness(newFadeOutDistance, 0, 0, 0));
            UpdateFadeOutDistance(fadeOutDownSB, new Thickness(0, newFadeOutDistance, 0, 0));
        }

        #endregion

        #region FadeOutDuration

        /// <summary>
        /// FadeOutDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeOutDurationProperty =
            DependencyProperty.Register("FadeOutDuration", typeof(Duration), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(new Duration(), new PropertyChangedCallback(OnFadeOutDurationChanged)));

        /// <summary>
        /// Gets or sets the FadeOutDuration property. This dependency property 
        /// indicates the duration for fading out the text.
        /// </summary>
        public Duration FadeOutDuration
        {
            get { return (Duration)GetValue(FadeOutDurationProperty); }
            set { SetValue(FadeOutDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the FadeOutDuration property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnFadeOutDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            Duration oldFadeOutDuration = (Duration)e.OldValue;
            Duration newFadeOutDuration = fsBar.FadeOutDuration;
            fsBar.OnFadeOutDurationChanged(oldFadeOutDuration, newFadeOutDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FadeOutDuration property.
        /// </summary>
        /// <param name="oldFadeOutDuration">Old Value</param>
        /// <param name="newFadeOutDuration">New Value</param>
        protected virtual void OnFadeOutDurationChanged(Duration oldFadeOutDuration, Duration newFadeOutDuration)
        {
            UpdateFadeOutDuration(fadeOutLeftSB, newFadeOutDuration);
            UpdateFadeOutDuration(fadeOutRightSB, newFadeOutDuration);
            UpdateFadeOutDuration(fadeOutUpSB, newFadeOutDuration);
            UpdateFadeOutDuration(fadeOutDownSB, newFadeOutDuration);
        }

        #endregion

        #region MoveDuration

        /// <summary>
        /// MoveDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty MoveDurationProperty =
            DependencyProperty.Register("MoveDuration", typeof(Duration), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(new Duration(), new PropertyChangedCallback(OnMoveDurationChanged)));

        /// <summary>
        /// Gets or sets the MoveDuration property. This dependency property 
        /// indicates the duration for moving the text while fading out.
        /// </summary>
        public Duration MoveDuration
        {
            get { return (Duration)GetValue(MoveDurationProperty); }
            set { SetValue(MoveDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the MoveDuration property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnMoveDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            Duration oldMoveDuration = (Duration)e.OldValue;
            Duration newMoveDuration = fsBar.MoveDuration;
            fsBar.OnMoveDurationChanged(oldMoveDuration, newMoveDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the MoveDuration property.
        /// </summary>
        /// <param name="oldMoveDuration">Old Value</param>
        /// <param name="newMoveDuration">New Value</param>
        protected virtual void OnMoveDurationChanged(Duration oldMoveDuration, Duration newMoveDuration)
        {
            UpdateMoveDuration(fadeOutLeftSB, newMoveDuration);
            UpdateMoveDuration(fadeOutRightSB, newMoveDuration);
            UpdateMoveDuration(fadeOutUpSB, newMoveDuration);
            UpdateMoveDuration(fadeOutDownSB, newMoveDuration);
        }

        #endregion

        #region TextHorizontalAlignment

        /// <summary>
        /// TextHorizontalAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register("TextHorizontalAlignment", typeof(HorizontalAlignment), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(HorizontalAlignment.Center,
                    new PropertyChangedCallback(OnTextHorizontalAlignmentChanged)));

        /// <summary>
        /// Gets or sets the TextHorizontalAlignment property. This dependency property 
        /// indicates the alignment of the Text in the FluidStatusBar.
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty); }
            set { SetValue(TextHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TextHorizontalAlignment property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTextHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            HorizontalAlignment oldTextHorizontalAlignment = (HorizontalAlignment)e.OldValue;
            HorizontalAlignment newTextHorizontalAlignment = fsBar.TextHorizontalAlignment;
            fsBar.OnTextHorizontalAlignmentChanged(oldTextHorizontalAlignment, newTextHorizontalAlignment);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextHorizontalAlignment property.
        /// </summary>
        /// <param name="oldTextHorizontalAlignment">Old Value</param>
        /// <param name="newTextHorizontalAlignment">New Value</param>
        protected void OnTextHorizontalAlignmentChanged(HorizontalAlignment oldTextHorizontalAlignment, HorizontalAlignment newTextHorizontalAlignment)
        {

        }

        #endregion

        #region TextVerticalAlignment

        /// <summary>
        /// TextVerticalAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextVerticalAlignmentProperty =
            DependencyProperty.Register("TextVerticalAlignment", typeof(VerticalAlignment), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(VerticalAlignment.Center,
                    new PropertyChangedCallback(OnTextVerticalAlignmentChanged)));

        /// <summary>
        /// Gets or sets the TextVerticalAlignment property. This dependency property 
        /// indicates the VerticalAlignment of the status message in the FluidStatusBar.
        /// </summary>
        public VerticalAlignment TextVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(TextVerticalAlignmentProperty); }
            set { SetValue(TextVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TextVerticalAlignment property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnTextVerticalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar fsBar = (FluidStatusBar)d;
            VerticalAlignment oldTextVerticalAlignment = (VerticalAlignment)e.OldValue;
            VerticalAlignment newTextVerticalAlignment = fsBar.TextVerticalAlignment;
            fsBar.OnTextVerticalAlignmentChanged(oldTextVerticalAlignment, newTextVerticalAlignment);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TextVerticalAlignment property.
        /// </summary>
        /// <param name="oldTextVerticalAlignment">Old Value</param>
        /// <param name="newTextVerticalAlignment">New Value</param>
        protected void OnTextVerticalAlignmentChanged(VerticalAlignment oldTextVerticalAlignment, VerticalAlignment newTextVerticalAlignment)
        {

        }

        #endregion
        
        #region Message

        /// <summary>
        /// Message Dependency Property
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(StatusMessage), typeof(FluidStatusBar),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMessageChanged)));

        /// <summary>
        /// Gets or sets the Message property. This dependency property 
        /// indicates the message to be displayed in the FluidStatusBar.
        /// </summary>
        public StatusMessage Message
        {
            get { return (StatusMessage)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Message property.
        /// </summary>
        /// <param name="d">FluidStatusBar</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FluidStatusBar statusBar = (FluidStatusBar)d;
            StatusMessage oldMessage = (StatusMessage)e.OldValue;
            StatusMessage newMessage = statusBar.Message;
            statusBar.OnMessageChanged(oldMessage, newMessage);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Message property.
        /// </summary>
        /// <param name="oldMessage">Old Value</param>
        /// <param name="newMessage">New Value</param>
        protected virtual void OnMessageChanged(StatusMessage oldMessage, StatusMessage newMessage)
        {
            if (newMessage != null)
            {
                SetStatus(newMessage);
            }
        }

        #endregion

        #endregion

        #region Construction / Initialization

        public FluidStatusBar()
        {
            InitializeComponent();

            messageQueue = new Queue<StatusMessage>();

            fadeOutLeftSB = (Storyboard)this.Resources["FadeInOutLeftStoryboard"];
            if (fadeOutLeftSB != null)
            {
                fadeOutLeftSB.Completed += new EventHandler(OnFadeOutAnimationCompleted);
            }

            fadeOutRightSB = (Storyboard)this.Resources["FadeInOutRightStoryboard"];
            if (fadeOutRightSB != null)
            {
                fadeOutRightSB.Completed += new EventHandler(OnFadeOutAnimationCompleted);
            }

            fadeOutUpSB = (Storyboard)this.Resources["FadeInOutUpStoryboard"];
            if (fadeOutUpSB != null)
            {
                fadeOutUpSB.Completed += new EventHandler(OnFadeOutAnimationCompleted);
            }

            fadeOutDownSB = (Storyboard)this.Resources["FadeInOutDownStoryboard"];
            if (fadeOutDownSB != null)
            {
                fadeOutDownSB.Completed += new EventHandler(OnFadeOutAnimationCompleted);
            }

            fadeInSB = (Storyboard)this.Resources["FadeInStoryboard"];
            if (fadeInSB != null)
            {
                fadeInSB.Completed += new EventHandler(OnFadeOutAnimationCompleted);
            }

            fadeInOutSB = fadeOutLeftSB;

            isAnimationInProgress = false;
        }

        #endregion

        #region APIs

        /// <summary>
        /// Sets the new status message in the status bar.
        /// </summary>
        /// <param name="statusMsg">New Status Message</param>
        public void SetStatus(StatusMessage statusMsg)
        {
            if (statusMsg == null)
                return;

            lock (messageQueue)
            {
                messageQueue.Enqueue(statusMsg);
            }

            ProcessAnimationQueue();

            
        }

        /// <summary>
        /// Sets the new status message in the status bar.
        /// </summary>
        /// <param name="message">New message to be displayed</param>
        /// <param name="isAnimated">Flag to indicate whether the old status message 
        /// should be animated when it fades out</param>
        public void SetStatus(string message, bool isAnimated)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(new StatusMessage(message, isAnimated));
            }

            ProcessAnimationQueue();
        }

        #endregion

        #region Helpers

        private void UpdateFadeOutDistance(Storyboard sb, Thickness thickness)
        {
            if (sb != null)
            {
                foreach (Timeline timeline in sb.Children)
                {
                    ThicknessAnimation anim = timeline as ThicknessAnimation;
                    if (anim != null)
                    {
                        anim.SetValue(ThicknessAnimation.ToProperty, thickness);
                    }
                }
            }
        }

        private void UpdateMoveDuration(Storyboard sb, Duration duration)
        {
            if (sb != null)
            {
                foreach (Timeline timeline in sb.Children)
                {
                    ThicknessAnimation anim = timeline as ThicknessAnimation;
                    if (anim != null)
                    {
                        anim.SetValue(ThicknessAnimation.DurationProperty, duration);
                    }
                }
            }
        }

        private void UpdateFadeOutDuration(Storyboard sb, Duration duration)
        {
            if (sb != null)
            {
                foreach (Timeline timeline in sb.Children)
                {
                    DoubleAnimation anim = timeline as DoubleAnimation;
                    if (anim != null)
                    {
                        anim.SetValue(DoubleAnimation.DurationProperty, duration);
                    }
                }
            }
        }

        private void ProcessAnimationQueue()
        {
            if (isAnimationInProgress)
                return;

            lock (messageQueue)
            {
                if (messageQueue.Count > 0)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (messageQueue.Count > 0)
                            {
                                isAnimationInProgress = true;
                                StatusMessage msg = messageQueue.Peek() as StatusMessage;
                                if (msg != null)
                                {
                                    if (msg.IsAnimated)
                                    {
                                        // Copy the text to begin fade out
                                        FadeOutTextBlock.Text = FadeInTextBlock.Text;
                                        FadeInTextBlock.Text = msg.Message;

                                        if (fadeInOutSB != null)
                                            fadeInOutSB.Begin();
                                    }
                                    else
                                    {
                                        FadeInTextBlock.Text = msg.Message;
                                        if (fadeInSB != null)
                                            fadeInSB.Begin();
                                    }
                                }
                            }
                        }));
                }
            }
        }

        #endregion

        #region Event Handlers

        void OnFadeOutAnimationCompleted(object sender, EventArgs e)
        {
            isAnimationInProgress = false;

            lock (messageQueue)
            {
                if (messageQueue.Count > 0)
                    messageQueue.Dequeue();
            }

            ProcessAnimationQueue();
        }

        #endregion
    }
}
