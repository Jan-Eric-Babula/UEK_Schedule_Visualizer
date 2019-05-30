using System;
using System.Collections.Generic;

namespace UEK_Schedule_Visualizer
{
    //TODO Add hash creation override
    /// <summary>
    /// Class that represents an entire subject.
    /// </summary>
    class CourseMain : IComparable
    {
        #region Properties

        ///<value><c>Subject</c> is the subject name.</value>
        public string Subject { get; }

        ///<value><c>SubjectL</c> is the subject name with lowercase letters.</value>
        public string SubjectL { get; }

        ///<value><c>Hash</c> is the individual hash for the instance.</value>
        public string Hash { get; }
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creates an instance of <c>CourseMain</c>.
        /// </summary>
        /// <param name="subject">Subject name.</param>
        public CourseMain(string subject)
        {
            this.Subject = subject.Trim();
            this.SubjectL = this.Subject.ToLower();
            this.Hash = Library.GetHashString(this.SubjectL);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Evaluates if instance and a given object are equal.
        /// Overrides original function to use internal <c>Hash</c> property.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>If the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if(obj.GetType() == this.GetType()){
                return this.Hash == ((CourseMain)obj).Hash;
            }
            return false;
        }

        #region Methods

        #region IComparable

        /// <summary>
        /// Compares the instance to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>If the given object is greater, equal or lesser.</returns>
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

        #endregion

        #endregion

        #endregion
    }
}
