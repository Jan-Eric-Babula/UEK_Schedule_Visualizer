using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_Schedule_Visualizer_ClassLibrary
{
    class Config
    {

        #region Properties

        public static Config Instance
        {
            get
            {
                return Config.instance;
            }
        }

        public static DateTime LastUpdated
        {
            get;
            set;
        }

        public static string LastUpdatedString
        {
            get
            {
                //TODO Replace with library/helper method
                return Config.LastUpdated.ToString("yyyy-MM-dd");
            }
            set
            {
                Config.LastUpdated = DateTime.Parse(value);
            }
        }

        //TODO References to subject objects (directly)
        public static List<object> SubjectList
        {
            get;
            private set;
        }

        #endregion

        #region Instances

        private static readonly Config instance = new Config();

        #endregion

        #region Constructors

        private Config()
        {
            Config.LastUpdated = new DateTime(0);
            Config.SubjectList = null;
        }

        #endregion

        #region Functions

        public static bool IsConfigLoaded()
        {
            return Config.SubjectList != null;
        }

        public static void LoadConfigFromFile()
        {
            if (!Config.IsConfigLoaded())
            {

            }
            else
            {
                throw new Exception("Config already initialized!");
            }
        }

        public static void SaveConfigToFile() { }

        #endregion

        #region Nested Class

        /// <summary>
        /// Object for serializing and deserialing json config file from and into.
        /// </summary>
        private class InternalConfig
        {
            public string LastUpdated;
            public List<string> SubjectList;
        }

        #endregion

    }

}
