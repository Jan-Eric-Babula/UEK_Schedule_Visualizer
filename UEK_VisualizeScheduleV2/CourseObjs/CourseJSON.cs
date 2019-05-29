using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_VisualizeScheduleV2
{
    /// <summary>
    /// Class to mirror structure of entries in Library Json file.
    /// </summary>
    class CourseJSON
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

        #region Functions

        #region Methods

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The date of the course.</returns>
        public string GetDate()
        {
            return this.dataZajec;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The starting time of the course.</returns>
        public string GetStart()
        {
            return this.godzinaOd;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The ending time of the course.</returns>
        public string GetEnd()
        {
            return this.godzinaDo;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The subject of the course.</returns>
        public string GetSubject()
        {
            return this.przedmiot;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The teachners name.</returns>
        public string GetTeacher()
        {
            return this.dydaktyk;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The room designation.</returns>
        public string GetRoom()
        {
            return this.nazwaSali;
        }

        /// <summary>
        /// Engish translation of property for easier use.
        /// </summary>
        /// <returns>The building designation.</returns>
        public string GetBuilding()
        {
            return this.lokalizacja;
        }

        #endregion

        #endregion
    }
}
