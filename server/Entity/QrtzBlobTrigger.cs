﻿using System;
using System.Collections.Generic;

namespace server.Entity
{
    public partial class QrtzBlobTrigger
    {
        public string SchedName { get; set; } = null!;
        public string TriggerName { get; set; } = null!;
        public string TriggerGroup { get; set; } = null!;
        public byte[]? BlobData { get; set; }
    }
}
