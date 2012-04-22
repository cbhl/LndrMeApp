using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LndrMeApp
{
    public class AppSettings
    {
        IsolatedStorageSettings settings;

        // key names
        const string FirstNameSettingKeyName = "FirstNameSetting";
        const string EmailAddressSettingKeyName = "EmailAddressSetting";
        const string SendEmailSettingKeyName = "SendEmailSetting";
        const string APIServerSettingKeyName = "APIServerSetting";
        const string APIKeySettingKeyName = "APIKeySetting";

        // defaults
        const string FirstNameSettingDefault = "";
        const string EmailAddressSettingDefault = "";
        const bool SendEmailSettingDefault = false;
        
        // production
        // const string APIServerSettingDefault = "http://lndr.me/";
        // const string APIKeySettingDefault = "y9dghu2e9hdgw9g2iowdghh89982gijb";
        // staging
        const string APIServerSettingDefault = "http://lndrme.iterate.ca/";
        const string APIKeySettingDefault = "ydnjbkfavphbgeheyk0zeof6lr0biu";

        public AppSettings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }

        public string FirstNameSetting
        {
            get
            {
                return GetValueOrDefault<string>(FirstNameSettingKeyName, FirstNameSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(FirstNameSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        public string EmailAddressSetting
        {
            get
            {
                return GetValueOrDefault<string>(EmailAddressSettingKeyName, EmailAddressSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(EmailAddressSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        public bool SendEmailSetting
        {
            get
            {
                return GetValueOrDefault<bool>(SendEmailSettingKeyName, SendEmailSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(SendEmailSettingKeyName, value)) {
                    Save();
                }
            }
        }

        public string APIServerSetting
        {
            get
            {
                return GetValueOrDefault<string>(APIServerSettingKeyName, APIServerSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(APIServerSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        public string APIKeySetting
        {
            get
            {
                return GetValueOrDefault<string>(APIKeySettingKeyName, APIKeySettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(APIKeySettingKeyName, value))
                {
                    Save();
                }
            }
        }

    }
}
