#nullable disable

namespace TodoApi.Modelo
{
    public class M_O_MAIL_OBJECTS
    {
        public string MAILITM_PID { get; set; }
        public string MAILITM_FID { get; set; }
        public string EventType { get; set; }
        public string EventDate { get; set; }
        public string Office { get; set; }
        public string Scanned { get; set; }
        public string Workstation { get; set; }
        public string Condition { get; set; }
        public string NextOffice { get; set; }

        public M_O_MAIL_OBJECTS()
        {
            MAILITM_PID = string.Empty;
        }
    }
}
