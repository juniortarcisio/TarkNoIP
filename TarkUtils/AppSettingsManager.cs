using System;
using System.Configuration;

namespace TarkUtils
{
    public class AppSettingsManager
    {
        /// <summary>
        /// Returns a converted value from AppSettings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key value</param>
        /// <returns>Value converted</returns>
        public static T Get<T>(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                throw new SettingsPropertyNotFoundException(key);

            var value = ConfigurationManager.AppSettings[key];

            T convertedValue;

            try
            {
                convertedValue = (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                throw new SettingsPropertyWrongTypeException(key);
            }

            return convertedValue;
        }
        
        /// <summary>
        /// Returns a converted value from AppSettings, in case it isn't found, it will return a default value        /// 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key value</param>
        /// <param name="defaultValue">Uses a default value in the case the key doesn't exists</param>
        /// <returns>Value converted or default</returns>
        public static T Get<T>(string key, T defaultValue)
        {
            try
            {
                return Get<T>(key);
            }
            catch (SettingsPropertyNotFoundException)
            {
                return defaultValue;
            }
        }
    }
}
