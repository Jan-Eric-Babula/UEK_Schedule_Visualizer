using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UEK_Schedule_Visualizer_ClassLibrary
{
    //TODO Add documentation
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

        public DateTime LastUpdated
        {
            get;
            set;
        }

        public string LastUpdatedString
        {
            get
            {
                //TODO Replace with library/helper method
                return this.LastUpdated.ToString("yyyy-MM-dd");
            }
            set
            {
                this.LastUpdated = DateTime.Parse(value);
            }
        }

        //TODO References to subject objects (directly)
        public List<object> SubjectList
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
            this.LastUpdated = new DateTime(0);
            this.SubjectList = null;
        }

        #endregion

        #region Functions

        public static bool IsConfigLoaded()
        {
            return Config.Instance.SubjectList != null;
        }

        private static void InitConfigToFile()
        {
            InternalConfig emptyConfig = new InternalConfig();
            emptyConfig.LastUpdated = (new DateTime(0)).ToString("yyyy-MM-dd");
            emptyConfig.SubjectList = new List<string>();

            string emptyConfigJson = JsonConvert.SerializeObject(emptyConfig, Formatting.Indented);
            File.WriteAllText("<path>", emptyConfigJson, Encoding.UTF8);
        }

        //TODO Streamline
        public static void LoadConfigFromFile(int recursionIndex = 0)
        {
            if (!Config.IsConfigLoaded())
            {
                if (!File.Exists("<path>"))
                {
                    Config.InitConfigToFile();
                }

                string configJson;
                InternalConfig rawConfig;

                try
                {
                    configJson = File.ReadAllText("<path>", Encoding.UTF8);
                    rawConfig = JsonConvert.DeserializeObject<InternalConfig>(configJson);
                }
                catch (Exception e)
                {
                    Thread.Sleep(250);
                    if(recursionIndex < 4)
                    {
                        Config.LoadConfigFromFile(recursionIndex + 1);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"An error occured loading the configuration file!\n" + 
                            $"{e.Message}\n{e.StackTrace}"
                        );
                        rawConfig = new InternalConfig();
                        rawConfig.LastUpdated = (new DateTime(0)).ToString("yyyy-MM-dd");
                        rawConfig.SubjectList = new List<string>();
                    }
                }
                finally
                {
                    //Config.Instance.SubjectList = rawConfig.SubjectList
                }

            }
            else
            {
                throw new Exception("Config already initialized!");
            }
        }

        public static void SaveConfigToFile() { }

        #endregion

        #region Nested Classes

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
