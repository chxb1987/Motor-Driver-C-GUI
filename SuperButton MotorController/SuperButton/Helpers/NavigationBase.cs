using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Abt.Controls.SciChart.Example.Helpers
{
    class NavigationBase
    {
        public enum NavigationMode
        {
            Back,
            Forward,
            Page
        }

        private Frame _rootFrame;

        public Frame Frame
        {
            get { return _rootFrame; }
            set { _rootFrame = value; }
        }

        public Action BeforeNavigation { get; set; }

        public Action<Exception, UserControl> AfterNavigation { get; set; }


        public NavigationBase(Frame navigationFrame)
        {
            _rootFrame = navigationFrame;
        }

        public void Navigate(NavigationMode navigationMode, Uri uri)
        {
            if (_rootFrame != null)
            {
                AddHandler();

                BeforeNavigation();

                switch (navigationMode)
                {
                    case NavigationMode.Page:
                        _rootFrame.Navigate(uri);
                        break;
                    case NavigationMode.Back:
                        _rootFrame.GoBack();
                        break;
                    case NavigationMode.Forward:
                        _rootFrame.GoForward();
                        break;
                }
            }
        }

        private void AddHandler()
        {
            NavigatedEventHandler successHandler = null;
            NavigationFailedEventHandler failureHandler = null;

            successHandler = (s, e) =>
            {
                _rootFrame.Navigated -= successHandler;
                _rootFrame.NavigationFailed -= failureHandler;

                AfterNavigation(null, e.Content as UserControl);
            };

            failureHandler = (s, e) =>
            {
                _rootFrame.Navigated -= successHandler;
                _rootFrame.NavigationFailed -= failureHandler;

                AfterNavigation(e.Exception, null);
            };

            _rootFrame.Navigated += successHandler;
            _rootFrame.NavigationFailed += failureHandler;
        }

        public bool CanGoBack()
        {
            return _rootFrame != null && _rootFrame.CanGoBack;
        }

        public bool CanGoForward()
        {
            return _rootFrame != null && _rootFrame.CanGoForward;
        }

        public Uri GetCurrentSource()
        {
            Uri uri = null;

            if (_rootFrame != null)
            {
                uri = _rootFrame.CurrentSource;
            }

            return uri;
        }

        public bool CanNavigateTo(string uri)
        {
            var isCurrent = IsCurrentSource(uri);
            return _rootFrame != null && !isCurrent;
        }

        private bool IsCurrentSource(string uri)
        {
            return _rootFrame != null &&
                _rootFrame.CurrentSource != null &&
                _rootFrame.CurrentSource.OriginalString == uri;
        }
    }
}
