using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_VisualizeScheduleV2
{
    class UEKVConfig
    {
        
        private DateTime lastUpdated;
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

        public DateTime LastUpdatedRaw()
        {
            return this.lastUpdated;
        }

        private List<string> subjects;
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

        private static UEKVConfig instance;

        public static UEKVConfig InitiateInstance(UEKVConfig c)
        {
            if(UEKVConfig.instance == null)
            {
                UEKVConfig.instance = c;
            }

            return UEKVConfig.GetInstance();

        }

        public static UEKVConfig GetInstance()
        {
            if(UEKVConfig.instance == null)
            {
                UEKVConfig c = new UEKVConfig();
                c.lastUpdated = new DateTime(0);
                c.subjects = new List<string>();
                UEKVConfig.instance = c;
            }
            return UEKVConfig.instance;
        }
        
    }
}
