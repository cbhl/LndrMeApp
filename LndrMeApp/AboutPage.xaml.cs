using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using LndrMeApp;

namespace LndrMeApp
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

            Visibility v = (Visibility)Resources["PhoneLightThemeVisibility"];
            if (v == System.Windows.Visibility.Visible)
            {
                // Light theme
            }
            else
            {
                // Dark theme
                LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x23, 0x39, 0x5B));
            }

            VersionTextBlock.Text = "Version: " + LndrMeApp.Resources.AppVersion;
        }
    }
}