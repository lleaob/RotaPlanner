namespace RotaPlanner.Services;

using System;
using System.Collections.Generic;
using RotaPlanner.Models;

public class RotaEngine
{
    public List<ShiftDay> GenerateSchedule(DateOnly startDate, RotaPattern pattern, int daysToGenerate)
    {
        List<ShiftDay> schedule = new();

        if (pattern == null || pattern.Sequence == null || pattern.Sequence.Count == 0 || daysToGenerate <= 0)
        {
            return schedule;
        }

        // Flatten the pattern into a simple day-by-day timeline of booleans
        // E.g., [4, -2, 5, -2] -> 4 on, 2 off, 5 on, 2 off
        List<bool> timeline = new();
        foreach (int count in pattern.Sequence)
        {
            bool isWork = count > 0;
            int absoluteCount = Math.Abs(count);
            for (int i = 0; i < absoluteCount; i++)
            {
                timeline.Add(isWork);
            }
        }

        if (timeline.Count == 0)
        {
            return schedule;
        }

        int timeIndex = 0;
        DateOnly currentDate = startDate;

        for (int i = 0; i < daysToGenerate; i++)
        {
            // Determine if it's a work day based on the pattern loop
            bool isWorkDay = timeline[i % timeline.Count];
            TimeOnly? startTime = null;

            if (isWorkDay)
            {
                // If start times are provided, cycle through them only on work days
                if (pattern.StartTimesPool != null && pattern.StartTimesPool.Count > 0)
                {
                    int hour = pattern.StartTimesPool[timeIndex % pattern.StartTimesPool.Count];
                    startTime = new TimeOnly(hour, 0);
                    timeIndex++;
                }
            }

            schedule.Add(new ShiftDay
            {
                Date = currentDate,
                IsWorkDay = isWorkDay,
                StartTime = startTime
            });

            currentDate = currentDate.AddDays(1);
        }

        return schedule;
    }
}
