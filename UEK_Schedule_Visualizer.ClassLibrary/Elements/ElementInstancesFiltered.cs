using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_Schedule_Visualizer_ClassLibrary.Elements
{
    class ElementInstancesFiltered
    {

        public List<ElementSubject> AllSubjects
        {
            get;
            private set;
        }

        public List<ElementClass> AllClasses
        {
            get;
            private set;
        }

        public List<ElementType> AllTypes
        {
            get;
            private set;
        }

        private static readonly ElementInstancesFiltered instance = new ElementInstancesFiltered();

        public static ElementInstancesFiltered Instance
        {
            get
            {
                return ElementInstancesFiltered.instance;
            }
        }

        private ElementInstancesFiltered()
        {
            this.AllSubjects = null;
            this.AllClasses = null;
            this.AllTypes = null;
        }
    }
}
