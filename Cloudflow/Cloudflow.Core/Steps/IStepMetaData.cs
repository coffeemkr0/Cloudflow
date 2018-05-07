using System;

namespace Cloudflow.Core.Steps
{
    public interface IStepMetaData
    {
        Type Type { get; }
    }
}