using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Command;

namespace LndrMeApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.AllAppliances = new ObservableCollection<ApplianceViewModel>();
            this.Washers = new ObservableCollection<ApplianceViewModel>();
            this.Dryers = new ObservableCollection<ApplianceViewModel>();
        }

        
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ApplianceViewModel> AllAppliances { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ApplianceViewModel> Washers { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ApplianceViewModel> Dryers { get; private set; }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get {
                return _isDataLoaded;
            }
            private set
            {
                if (value != _isDataLoaded)
                {
                    _isDataLoaded = value;
                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }

        private void OnClaim(ApplianceViewModel avm)
        {
            UriBuilder fullUri = new UriBuilder("http://lndr.me/receive.json");
            fullUri.Query = String.Format("key={0}&id={1}&email={2}",Resources.ServerKey,avm.Id,Resources.DefaultEMail);

            // initialize a new WebRequest
            HttpWebRequest lndrRequest = (HttpWebRequest)WebRequest.Create(fullUri.Uri);

            // set up the state object for the async request
            LndrUpdateState lndrUpdateState = new LndrUpdateState();
            lndrUpdateState.AsyncRequest = lndrRequest;

            // start the asynchronous request
            lndrRequest.BeginGetResponse(new AsyncCallback(HandleUpdateResponse),
                lndrUpdateState);
        }

        private void AddAppliance(ApplianceViewModel avm)
        {
            avm.ClaimCommand = new RelayCommand(() =>
                OnClaim(avm));
            this.AllAppliances.Add(avm);
            switch (avm.Appliance)
            {
                case ApplianceViewModel.ApplianceType.DRYER:
                    this.Dryers.Add(avm);
                    break;
                case ApplianceViewModel.ApplianceType.WASHER:
                    this.Washers.Add(avm);
                    break;
            }
        }

        public void Clear()
        {
            this.AllAppliances.Clear();
            this.Dryers.Clear();
            this.Washers.Clear();
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            //AddAppliance(new ApplianceViewModel() { Appliance = ApplianceViewModel.ApplianceType.WASHER, Id = 1, Name = "Washer 1", Busy = false, FreeAt = DateTime.Now });
            //AddAppliance(new ApplianceViewModel() { Appliance = ApplianceViewModel.ApplianceType.WASHER, Id = 2, Name = "Washer 2", Busy = true, FreeAt = DateTime.Now.AddHours(1.2) });
            //AddAppliance(new ApplianceViewModel() { Appliance = ApplianceViewModel.ApplianceType.DRYER, Id = 3, Name = "Dryer 1", Busy = true, FreeAt = DateTime.Now.AddMinutes(42) });
            //AddAppliance(new ApplianceViewModel() { Appliance = ApplianceViewModel.ApplianceType.DRYER, Id = 4, Name = "Dryer 2", Busy = false, FreeAt = DateTime.Now });

            this.IsDataLoaded = false;
            Clear();

            UriBuilder fullUri = new UriBuilder(Resources.ServerBaseURI + Resources.ServerStatusEndpoint);
            fullUri.Query = String.Format("key={0}", Resources.ServerKey);

            // initialize a new WebRequest
            HttpWebRequest lndrRequest = (HttpWebRequest)WebRequest.Create(fullUri.Uri);

            // set up the state object for the async request
            LndrUpdateState lndrUpdateState = new LndrUpdateState();
            lndrUpdateState.AsyncRequest = lndrRequest;

            // start the asynchronous request
            lndrRequest.BeginGetResponse(new AsyncCallback(HandleIndexResponse),
                lndrUpdateState);
        }

        /// <summary>
        /// Handle the information returned from the async request
        /// </summary>
        /// <param name="asyncResult"></param>
        private void HandleUpdateResponse(IAsyncResult asyncResult)
        {
            LndrUpdateState lndrUpdateState = (LndrUpdateState)asyncResult.AsyncState;
            HttpWebRequest lndrRequest = (HttpWebRequest)lndrUpdateState.AsyncRequest;

            lndrUpdateState.AsyncResponse = (HttpWebResponse)lndrRequest.EndGetResponse(asyncResult);

            try {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LoadData();
                }
                );

            }
            catch (FormatException)
            {
                // there was some kind of error processing the response from the web
                // additional error handling would normally be added here
                return;
            }
        }

        /// <summary>
        /// Handle the information returned from the async request
        /// </summary>
        /// <param name="asyncResult"></param>
        private void HandleIndexResponse(IAsyncResult asyncResult)
        {
            LndrUpdateState lndrUpdateState = (LndrUpdateState)asyncResult.AsyncState;
            HttpWebRequest lndrRequest = (HttpWebRequest)lndrUpdateState.AsyncRequest;

            lndrUpdateState.AsyncResponse = (HttpWebResponse)lndrRequest.EndGetResponse(asyncResult);

            try
            {
                Stream streamResult = lndrUpdateState.AsyncResponse.GetResponseStream();
                StreamReader sr = new StreamReader(streamResult);
                string json = sr.ReadToEnd();
                IList<ApplianceViewModel> appliances = JsonConvert.DeserializeObject<IList<ApplianceViewModel>>(json);
                //foreach( ApplianceViewModel appliance in appliances ) {
                //    appliance.Appliance = (appliance.Id <= 2 ? ApplianceViewModel.ApplianceType.WASHER : ApplianceViewModel.ApplianceType.DRYER);
                //}

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Clear();
                        foreach (ApplianceViewModel appliance in appliances)
                        {
                            AddAppliance(appliance);
                        }
                        this.IsDataLoaded = true;
                    }
                );

            }
            catch (FormatException)
            {
                // there was some kind of error processing the response from the web
                // additional error handling would normally be added here
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// State information for our BeginGetResponse async call
    /// </summary>
    public class LndrUpdateState
    {
        public HttpWebRequest AsyncRequest { get; set; }
        public HttpWebResponse AsyncResponse { get; set; }
    }
}