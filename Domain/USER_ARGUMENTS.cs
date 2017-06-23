using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class USER_ARGUMENTS : Entity
    {
        public string OBJECT_NAME { get; set; }
        public string PACKAGE_NAME { get; set; }
        public string ARGUMENT_NAME { get; set; }
        public int POSITION { get; set; } //0 IS RETURN VALUE
        public string DATA_TYPE { get; set; }
        public string DEFAULT_VALUE { get; set; }
        public string IN_OUT { get; set; }   
    }
}
