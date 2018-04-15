using System;

namespace Cloudflow.Core.Data.Agent.Models
{
    public class Run
    {
        #region Enums

        public enum RunStatuses
        {
            Queued,
            Running,
            Completed,
            CompletedWithWarnings,
            Canceled,
            Failed
        }

        #endregion

        #region Constructors

        public Run()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string JobName { get; set; }

        public DateTime DateQueued { get; set; }

        public DateTime? DateStarted { get; set; }

        public DateTime? DateEnded { get; set; }

        public RunStatuses Status { get; set; }

        #endregion
    }
}