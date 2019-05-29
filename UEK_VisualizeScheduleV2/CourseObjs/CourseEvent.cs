using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_VisualizeScheduleV2
{
    //TODO Add hash creation override
    /// <summary>
    /// A <c>CourseEvent</c> represents a single class happening of a subject.
    /// </summary>
    class CourseEvent: IComparable
    {
        #region Properties

        /// <value><c>Start</c> is the DateTime at which the course starts.</value>
        public DateTime Start { get; }

        /// <value><c>End</c> is the DateTime at which the course ends</value>
        public DateTime End { get; }

        /// <value><c>StartS</c> is the string-formatted time at which the course starts.</value>
        public string StartS
        {
            get
            {
                return Library.FormatDTTime(this.Start);
            }
        }

        /// <value><c>EndS</c> is the string-formatted time at which the course ends.</value>
        public string EndS
        {
            get
            {
                return Library.FormatDTTime(this.End);
            }
        }

        /// <value><c>Date</c> is only the date at which the course takes place.</value>
        public DateTime Date
        {
            get
            {
                return new DateTime(this.Start.Year, this.Start.Month, this.Start.Day);
            }
        }

        /// <value><c>DateS</c> is the string-formatted date at which the course takes place.</value>
        public string DateS
        {
            get
            {
                return Library.FormatDTDate(this.Start);
            }
        }

        /// <value><c>Course</c> refers to the main subject of which <c>CourseEvent</c> is an instance of.</value>
        public CourseMain Course { get; }

        /// <value><c>Room</c> is the room designation.</value>
        public string Room { get; }

        /// <value><c>Building</c> is the Building designation.</value>
        public string Building { get; }

        /// <value><c>Location</c> is the combination of <c>Room</c> and <c>Building</c>.</value>
        public string Location
        {
            get
            {
                return $"{this.Room} {this.Building}";
            }
        }

        /// <value><c>Type</c> is a reference to the type of lecture.</value>
        public LectureType Type { get; }

        /// <value><c>Hash</c> is the individual hash of the class instance.</value>
        public string Hash { get; }

        /// <value><c>Teacher</c> is the name of the teacher.</value>
        public string Teacher { get; }

        #endregion

        #region Constructor(s)
        
        /// <summary>
        /// Creates an instance of <c>CourseEvent</c>.
        /// </summary>
        /// <param name="date">Date of the course.</param>
        /// <param name="start">Starting time of the course.</param>
        /// <param name="end">Ending time of the course.</param>
        /// <param name="room">Room designation of the course.</param>
        /// <param name="building">Building designation of the course.</param>
        /// <param name="teacher">The name of the teacher.</param>
        /// <param name="cm">Reference to the subject.</param>
        /// <param name="lt">Reference to the type of lecture.</param>
        public CourseEvent(string date, string start, string end, string room, string building, string teacher, CourseMain cm, LectureType lt)
        {
            this.Start = DateTime.Parse($"{date.Trim()} {start.Trim()}");
            this.End = DateTime.Parse($"{date.Trim()} {end.Trim()}");
            this.Room = room.Trim();
            this.Building = building.Trim();
            this.Course = cm;
            this.Type = lt;
            this.Teacher = teacher;
            this.Hash = Library.GetHashString($"{this.Start}{this.End}{this.Location}{this.Teacher}{this.Course.Hash}{this.Type.Hash}");
        }

        #endregion

        #region Functions

        #region Methods

        /// <summary>
        /// Compares the instance of the class to a given object.
        /// Overrides original function to use internal <c>Hash</c> property.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Returns <c>true</c> if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this.Hash == ((CourseEvent)obj).Hash;
            }
            return false;
        }

        #region IComparable

        /// <summary>
        /// Compares the instance of the class to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>If the given object is greater, equal or lesser.</returns>
        public int CompareTo(object obj)
        {
            if (obj.GetType() == this.GetType()) {
                CourseEvent ce = (CourseEvent)obj;

                if(ce.Date < this.Date)
                {
                    return 1;
                }else if(ce.Date > this.Date)
                {
                    return -1;
                }
                else
                {
                    if(ce.Start < this.Start)
                    {
                        return 1;
                    }else if(ce.Start > this.Start)
                    {
                        return -1;
                    }
                    else
                    {
                        if(ce.End < this.End)
                        {
                            return 1;
                        }else if(ce.End > this.End)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
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
