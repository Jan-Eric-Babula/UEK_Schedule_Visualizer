using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_Schedule_Visualizer
{
    //TODO Add hash creation override
    /// <summary>
    /// Class to represent the possible types of lectures.
    /// </summary>
    class LectureType
    {
        #region Properties

        ///<value><c>Name</c> is the display name of the lecture type.</value>
        public string Name { get; }

        ///<value><c>RawName</c> is the given/polish designation of the lecture type.</value>
        public string RawName { get; }

        ///<value><c>RawNameL</c> is the given/polish designation of the lecture type with lowercase letters.</value>
        public string RawNameL { get; }

        ///<value><c>Hash</c> is the individual hash of the instance.</value>
        public string Hash { get; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of <c>LectureType</c>.
        /// </summary>
        /// <param name="rawname">The given/polish designation of the lecture type.</param>
        public LectureType (string rawname)
        {
            this.RawName = rawname.Trim();
            this.RawNameL = this.RawName.ToLower();
            this.Name = LectureType.ResolveRawName(this.RawName);
            this.Hash = Library.GetHashString(this.RawNameL);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Resolves and streamlines designations of given/polish lecture types.
        /// </summary>
        /// <param name="rawName">Given/polish designation of the lecture type.</param>
        /// <returns>English and streamlined lecture type designations.</returns>
        private static string ResolveRawName(string rawName)
        {
            switch (rawName.Trim())
            {
                case "wykład z egzaminem": return "Lecture";
                case "wykład z zaliczeniem": return "Lecture";
                case "Wykład": return "Lecture";
                case "lab": return "Laboratory";
                case "Laboratoria": return "Laboratory";
                case "Ćwiczenia": return "Class/Tutorial";
                case "język polski": return "Polish Language Course";
                default: return "#Undefined#";
            }
        }

        #region Methods

        /// <summary>
        /// Evaluates if instance and a given object are equal.
        /// Overrides original function to use internal <c>Hash</c> property.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>If the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this.Hash == ((LectureType)obj).Hash;
            }
            return false;
        }

        #endregion

        #endregion
    }
}
