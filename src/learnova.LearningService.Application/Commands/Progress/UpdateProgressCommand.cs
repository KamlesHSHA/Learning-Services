using System;

namespace learnova.LearningService.Application.Commands.Progress
{
    public class UpdateProgressCommand
    {
        public Guid ProgressId { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
