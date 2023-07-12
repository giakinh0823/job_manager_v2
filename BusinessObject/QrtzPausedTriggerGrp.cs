using System;
using System.Collections.Generic;

namespace prn_job_manager.Models
{
    public partial class QrtzPausedTriggerGrp
    {
        public string SchedName { get; set; } = null!;
        public string TriggerGroup { get; set; } = null!;
    }
}
