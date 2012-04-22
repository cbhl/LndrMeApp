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
using Microsoft.Phone.Shell;
using LndrMeApp;

namespace LndrMeApp
{
    public partial class AboutPage : PhoneApplicationPage
    {
        AppSettings appSettings;

        int selectedServerIndex;
        string initialCustomServerUrl;
        string initialCustomServerKey;

        public AboutPage()
        {
            // Save server settings for when the back button is pressed.
            appSettings = new AppSettings();
            initialCustomServerUrl = appSettings.CustomAPIServerSetting;
            initialCustomServerKey = appSettings.CustomAPIKeySetting;
            selectedServerIndex = appSettings.APIServerSetting;

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

            // FIXME: Hack to get ListBox to show the correct item even though the items are populated after SelectedIndex is set.
            ApiServerListBox.SelectedIndex = 0;
            ApiServerListBox.SelectedIndex = selectedServerIndex;

            // Add an Application Bar that has a 'done' confirmation button and 
            // a 'cancel' button
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = 1.0;

            ApplicationBarIconButton doneButton = new ApplicationBarIconButton(new Uri("/icons/appbar.check.rest.png", UriKind.Relative));
            doneButton.Text = "done";
            doneButton.Click += new EventHandler(doneButton_Click);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("/icons/appbar.cancel.rest.png", UriKind.Relative));
            cancelButton.Text = "cancel";
            cancelButton.Click += new EventHandler(cancelButton_Click);

            ApplicationBar.Buttons.Add(doneButton);
            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void ValidateApiServerControls()
        {
            if ((CustomApiServerTextBox != null) && (CustomApiKeyTextBox != null))
            {
                // update UI
                switch (appSettings.APIServerSetting)
                {
                    case 0:
                    case 1:
                        CustomApiServerTextBox.Text = appSettings.CurrentAPIServer;
                        CustomApiKeyTextBox.Text = appSettings.CurrentAPIKey;
                        CustomApiServerTextBox.IsEnabled = false;
                        CustomApiKeyTextBox.IsEnabled = false;
                        CustomApiKeyTextBox.Visibility = CustomApiKeyTextBlock.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    default:
                        CustomApiServerTextBox.Text = appSettings.CurrentAPIServer;
                        CustomApiKeyTextBox.Text = appSettings.CurrentAPIKey;
                        CustomApiServerTextBox.IsEnabled = true;
                        CustomApiKeyTextBox.IsEnabled = true;
                        CustomApiKeyTextBox.Visibility = CustomApiKeyTextBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                }
            }
        }

        private void ApiServerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApiServerListBox != null)
            {
                // update UI
                switch (ApiServerListBox.SelectedIndex)
                {
                    case 0:
                    case 1:
                        appSettings.APIServerSetting = ApiServerListBox.SelectedIndex;
                        break;
                    default:
                        appSettings.APIServerSetting = 2;
                        break;
                }
            }

            ValidateApiServerControls();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            // save settings
            appSettings.FirstNameSetting = FirstNameTextBox.Text;
            appSettings.EmailAddressSetting = EmailAddressTextBox.Text;
            appSettings.SendEmailSetting = (bool) SendEmailCheckBox.IsChecked;
            appSettings.APIServerSetting = selectedServerIndex = ApiServerListBox.SelectedIndex;
            switch (ApiServerListBox.SelectedIndex)
            {
                case 0:
                case 1:
                    break;
                default:
                    appSettings.CustomAPIServerSetting = initialCustomServerUrl = CustomApiServerTextBox.Text;
                    appSettings.CustomAPIKeySetting = initialCustomServerKey = CustomApiKeyTextBox.Text;
                    break;
            }

            NavigationService.GoBack();
        }

        private void RestoreSettings()
        {
            // (FIXME: may result in double-write when saving)
            appSettings.APIServerSetting = selectedServerIndex;
            switch (ApiServerListBox.SelectedIndex)
            {
                case 0:
                case 1:
                    break;
                default:
                    appSettings.CustomAPIServerSetting = initialCustomServerUrl;
                    appSettings.CustomAPIKeySetting = initialCustomServerKey;
                    break;
            }

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            RestoreSettings();
            base.OnBackKeyPress(e);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            RestoreSettings();
            NavigationService.GoBack();
        }
    }
}
