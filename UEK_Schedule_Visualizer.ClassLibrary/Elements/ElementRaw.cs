using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_Schedule_Visualizer_ClassLibrary.Elements
{
    class ElementRaw
    {

        #region Properties

        ///<value><c>dataZajec</c> is the date of the course.</value>
        public string dataZajec { get; set; }

        ///<value><c>godzinaOd</c> is the starting time of the course.</value>
        public string godzinaOd { get; set; }

        ///<value><c>godzinaDo</c> is the ending time of the course.</value>
        public string godzinaDo { get; set; }

        ///<value><c>przedmiot</c> is the subject of the course.</value>
        public string przedmiot { get; set; }

        ///<value><c>dydaktyk</c> is the name of the teacher.</value>
        public string dydaktyk { get; set; }

        ///<value><c>nazwaSali</c> is the room designation.</value>
        public string nazwaSali { get; set; }

        ///<value><c>lokalizacja</c> is the building designation.</value>
        public string lokalizacja { get; set; }

        #endregion

    }
}
