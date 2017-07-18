using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiStaff.Domain
{
    public class CO_SWIPE_DATA : Entity
    {
        public decimal? ID { get; set; }
        public string EMPLOYEEID { get; set; }
        public DateTime? WORKINGDAY { get; set; }
        public DateTime? VALIN1 { get; set; }
        public DateTime? VALIN2 { get; set; }
        public DateTime? VALIN3 { get; set; }
        public DateTime? VALIN4 { get; set; }
        public DateTime? VALOUT1 { get; set; }
        public DateTime? VALOUT2 { get; set; }
        public DateTime? VALOUT3 { get; set; }
        public DateTime? VALOUT4 { get; set; }
    }
}
