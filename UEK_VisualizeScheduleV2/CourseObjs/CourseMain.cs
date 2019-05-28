using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace UEK_VisualizeScheduleV2
{
    //TODO Add hash creation override
    class CourseMain : IComparable
    {
        public string Subject { get; }
        public string SubjectL { get; }
        public string Hash { get; }

        public CourseMain(string subject)
        {
            this.Subject = subject.Trim();
            this.SubjectL = this.Subject.ToLower();
            this.Hash = Library.GetHashString(this.SubjectL);
        }
        
        public override bool Equals(object obj)
        {
            if(obj.GetType() == this.GetType()){
                return this.Hash == ((CourseMain)obj).Hash;
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            if(obj.GetType() == this.GetType())
            {
                CourseMain cm = (CourseMain)obj;
                if(cm.Subject == this.Subject)
                {
                    return 0;
                }
                else
                {
                    List<string> tmp = new List<string>(new string[] { cm.Subject, this.Subject });
                    tmp.Sort();
                    if(tmp[0] == cm.Subject)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }
    }
}
