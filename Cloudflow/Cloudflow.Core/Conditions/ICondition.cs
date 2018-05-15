using System;

namespace Cloudflow.Core.Conditions
{
    public interface ICondition : IDisposable
    {
        bool Evaluate();
    }

    public static class ConditionExtensions
    {
        public static string GetClassName(this ICondition condition)
        {
            return condition.GetType().Name;
        }
    }
}