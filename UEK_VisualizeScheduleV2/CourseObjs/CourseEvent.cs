using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEK_VisualizeScheduleV2
{
    //TODO Add hash creation override
    class CourseEvent: IComparable
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public string StartS
        {
            get
            {
                return Library.FormatDTTime(this.Start);
            }
        }
        public string EndS
        {
            get
            {
                return Library.FormatDTTime(this.End);
            }
        }
        public DateTime Date
        {
            get
            {
                return new DateTime(this.Start.Year, this.Start.Month, this.Start.Day);
            }
        }
        public string DateS
        {
            get
            {
                return Library.FormatDTDate(this.Start);
            }
        }

        public CourseMain Course { get; }

        public string Room { get; }
        public string Building { get; }
        public string Location
        {
            get
            {
                return $"{this.Room} {this.Building}";
            }
        }

        public LectureType Type { get; }
        public string Hash { get; }

        public string Teacher { get; }

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

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return this.Hash == ((CourseEvent)obj).Hash;
            }
            return false;
        }

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
    }
}
