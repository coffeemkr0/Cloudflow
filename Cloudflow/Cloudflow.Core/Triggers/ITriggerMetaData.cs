using System;

namespace Cloudflow.Core.Triggers
{
    public interface ITriggerMetaData
    {
        Type Type { get; }
    }
}