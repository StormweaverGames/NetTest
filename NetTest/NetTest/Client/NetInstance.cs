﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTest.Client
{
    public abstract class NetInstance
    {
        public abstract  byte NetworkID { get; }
        public abstract short InstanceID { get; }
    }
}
