using System;
using System.Collections.Generic;

namespace num1_Project
{
    public class CapsuleInfo
    {
        public int CapsuleId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime OpenDate { get; set; }
        public bool NoticeShown { get; set; }
        public List<SongInfo> Songs { get; set; } = new List<SongInfo>();

        public int DDay
        {
            get
            {
                return (OpenDate.Date - DateTime.Now.Date).Days;
            }
        }

        public bool IsOpenable
        {
            get
            {
                return DateTime.Now.Date >= OpenDate.Date;
            }
        }
    }
}