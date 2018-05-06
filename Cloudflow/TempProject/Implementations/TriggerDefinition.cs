﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Implementations
{
    public class TriggerDefinition
    {
        public string Name { get; set; }

        public string AssemblyPath { get; set; }

        public Guid ExtensionId { get; set; }

        public Guid ConfigurationExtensionId { get; set; }

        public string Configuration { get; set; }
    }
}