using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LndrMeApp
{
    [JsonObject]
    public class ApplianceViewModel : INotifyPropertyChanged
    {
        public enum ApplianceType { WASHER, DRYER };

        public string Status
        {
            get
            {
                if (Busy)
                {
                    return string.Format(Resources.WillBeAvailableIn, DateHelper.DistanceOfTimeInWords(DateTime.Now.ToUniversalTime(), FreeAt));
                }
                else
                {
                    return Resources.Available;
                }
            }
        }

        //private ApplianceType _appliance;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public ApplianceType Appliance
        {
            get
            {
                //return _appliance;
                return (Id <= 2 ? ApplianceViewModel.ApplianceType.WASHER : ApplianceViewModel.ApplianceType.DRYER);
            }
            //set
            //{
            //    if (value != _appliance)
            //    {
            //        _appliance = value;
            //        NotifyPropertyChanged("Appliance");
            //    }
            //}
        }

        private int _id;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("id")]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                    NotifyPropertyChanged("Appliance");
                }
            }
        }

        private string _name;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private bool _busy;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("busy")]
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                if (value != _busy)
                {
                    _busy = value;
                    NotifyPropertyChanged("Busy");
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private DateTime _freeAt;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        [JsonProperty("free_at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime FreeAt
        {
            get
            {
                return _freeAt;
            }
            set
            {
                if (value != _freeAt)
                {
                    _freeAt = value;
                    NotifyPropertyChanged("FreeAt");
                }
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
}