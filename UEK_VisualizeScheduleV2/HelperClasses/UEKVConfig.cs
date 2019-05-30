using System;
using System.Collections.Generic;

namespace UEK_Schedule_Visualizer
{
    /// <summary>
    /// Class to represent and hold working data of program config file.
    /// </summary>
    class UEKVConfig
    {
        #region Properties

        /// <summary>
        /// <c>lastUpdated</c> is the DateTime at which the data file was last updated.
        /// </summary>
        private DateTime lastUpdated;

        /// <summary>
        /// <c>subjects</c> is a list containing all the subject names selected.
        /// </summary>
        private List<string> subjects;

        /// <value><c>LastUpdated</c> is the Date at which the data file was last updated.</value>
        public string LastUpdated {
            get
            {
                return this.lastUpdated.ToString("yyyy-MM-dd");
            }
            set
            {
                this.lastUpdated = DateTime.Parse(value);
            }
        }

        /// <value><c>Subjects</c> is the list of selected subjects.</value>
        public List<string> Subjects
        {
            get
            {
                return this.subjects;
            }
            set
            {
                this.subjects = value;
            }
        }
        #endregion

        #region Instances

        /// <summary>
        /// <c>instance</c> holds the main instance of <c>UEKConfig</c>.
        /// </summary>
        private static UEKVConfig instance;

        #endregion

        #region Functions

        /// <summary>
        /// Sets the main instance to a given instance, if no instance exsists yet.
        /// Used to initiate the config instance when loading the config data from file.
        /// </summary>
        /// <param name="c">Instance of <c>UEKVConfig</c> to set the main instance to.</param>
        /// <returns>Main instance of <c>UEKVConfig</c>.</returns>
        public static UEKVConfig InitiateInstance(UEKVConfig c)
        {
            if (UEKVConfig.instance == null)
            {
                UEKVConfig.instance = c;
            }

            return UEKVConfig.GetInstance();

        }

        /// <summary>
        /// Gets the main instance of <c>UEKVConfig</c>.
        /// Creates one, if none exsists yet.
        /// </summary>
        /// <returns>Main instance of <c>UEKVConfig</c>.</returns>
        public static UEKVConfig GetInstance()
        {
            if (UEKVConfig.instance == null)
            {
                UEKVConfig c = new UEKVConfig();
                c.lastUpdated = new DateTime(0);
                c.subjects = new List<string>();
                UEKVConfig.instance = c;
            }
            return UEKVConfig.instance;
        }

        #region Methods

        /// <summary>
        /// Returns the DateTime at which the data was last updated.
        /// </summary>
        /// <returns>DateTime of last data update.</returns>
        public DateTime LastUpdatedRaw()
        {
            return this.lastUpdated;
        }

        #endregion

        #endregion


    }
}
