﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public interface IExtensionMetaData
    {
        string Id { get; }

        Type Type { get; }
    }
}
