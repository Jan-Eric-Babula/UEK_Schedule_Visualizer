using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_VisualizeScheduleV2
{
    //TODO Add hash creation override
    class LectureType
    {
        public string Name { get; }
        public string RawName { get; }
        public string RawNameL { get; }
        public string Hash { get; }

        public LectureType (string rawname)
        {
            this.RawName = rawname.Trim();
            this.RawNameL = this.RawName.ToLower();
            this.Name = LectureType.ResolveRawName(this.RawName);
            this.Hash = Library.GetHashString(this.RawNameL);
        }

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

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this.Hash == ((LectureType)obj).Hash;
            }
            return false;
        }
    }
}
