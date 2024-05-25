using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace TagExtractort.Application.JobSchedule
{
    public class SkipWhenPreviousJobIsRunningAttribute : JobFilterAttribute, IClientFilter, IApplyStateFilter
    {
        public void OnCreating(CreatingContext context)
        {
            var connection = context.Connection as JobStorageConnection;

            if (connection == null) return;

            if (!context.Parameters.ContainsKey("RecurringJobId")) return;

            var recurringJobId = context.Parameters["RecurringJobId"] as string;

            if (string.IsNullOrWhiteSpace(recurringJobId)) return;

            var running = connection.GetValueFromHash($"recurring-job:{recurringJobId}", "Running");
            if ("yes".Equals(running, StringComparison.OrdinalIgnoreCase))
            {
                context.Canceled = true;
            }
        }

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is EnqueuedState)
            {
                var recurringJobId = SerializationHelper.Deserialize<string>(context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));
                if (string.IsNullOrWhiteSpace(recurringJobId)) return;

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "yes") });
            }
            else if (context.NewState.IsFinal || context.NewState is FailedState)
            {
                var recurringJobId = SerializationHelper.Deserialize<string>(context.Connection.GetJobParameter(context.BackgroundJob.Id, "RecurringJobId"));
                if (string.IsNullOrWhiteSpace(recurringJobId)) return;

                transaction.SetRangeInHash(
                    $"recurring-job:{recurringJobId}",
                    new[] { new KeyValuePair<string, string>("Running", "no") });
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}