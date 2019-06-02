using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_Schedule_Visualizer_ClassLibrary.Elements
{
    class ElementInstancesRaw
    {

        public List<ElementRaw> AllElementsRaw
        {
            get;
            private set;
        }

        private static readonly ElementInstancesRaw instance = new ElementInstancesRaw();

        public static ElementInstancesRaw Instance
        {
            get
            {
                return ElementInstancesRaw.instance;
            }
        }

        private ElementInstancesRaw()
        {
            this.AllElementsRaw = null;
        }

    }
}
