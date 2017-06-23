using System;

namespace Domain
{
    [Serializable]
    public class Organization
    {
        public int? org_id { get; set; }
        public string org_name { get; set; }
        public int? parent_id { get; set; }
        public string org_level { get; set; }
        public DateTime created_date { get; set; }

        public string address{ get; set; }
    
     
    }
}