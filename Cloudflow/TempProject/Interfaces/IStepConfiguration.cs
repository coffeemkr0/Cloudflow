namespace TempProject.Interfaces
{
    public interface IStepConfiguration
    {
    }

    public static class StepConfigurationExtensions
    {
        public static string GetClassName(this IStepConfiguration step)
        {
            return step.GetType().Name;
        }
    }
}