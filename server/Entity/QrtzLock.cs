﻿using System;
using System.Collections.Generic;

namespace server.Entity
{
    public partial class QrtzLock
    {
        public string SchedName { get; set; } = null!;
        public string LockName { get; set; } = null!;
    }
}
