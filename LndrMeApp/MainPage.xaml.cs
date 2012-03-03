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

namespace LndrMeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);

            if (App.ViewModel.IsDataLoaded)
            {
                MainProgressBar.IsIndeterminate = false;
                MainProgressBar.Visibility = Visibility.Collapsed;
            }else{
                MainProgressBar.IsIndeterminate = true;
                MainProgressBar.Visibility = Visibility.Visible;
                App.ViewModel.LoadData();
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsDataLoaded"))
            {
                MainProgressBar.IsIndeterminate = !App.ViewModel.IsDataLoaded;
                MainProgressBar.Visibility = App.ViewModel.IsDataLoaded ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}