namespace RotaPlanner.Tests;

using System;
using System.Collections.Generic;
using RotaPlanner.Models;
using RotaPlanner.Services;
using Xunit;

public class RotaEngineTests
{
    private readonly RotaEngine _engine = new();

    [Fact]
    public void GenerateSchedule_AlternatingPattern_GeneratesCorrectWorkAndOffDays()
    {
        // Arrange: 4 on, 2 off, 5 on, 2 off (13 day cycle)
        DateOnly startDate = new(2026, 9, 1);
        RotaPattern pattern = new()
        {
            Sequence = new List<int> { 4, -2, 5, -2 }
        };

        // Act
        List<ShiftDay> schedule = _engine.GenerateSchedule(startDate, pattern, 14);

        // Assert
        Assert.Equal(14, schedule.Count);

        // Days 1-4: Work (Sep 1 to Sep 4)
        Assert.True(schedule[0].IsWorkDay);
        Assert.Equal(new DateOnly(2026, 9, 1), schedule[0].Date);
        Assert.True(schedule[1].IsWorkDay);
        Assert.True(schedule[2].IsWorkDay);
        Assert.True(schedule[3].IsWorkDay);

        // Days 5-6: Off (Sep 5 to Sep 6)
        Assert.False(schedule[4].IsWorkDay);
        Assert.False(schedule[5].IsWorkDay);

        // Days 7-11: Work (Sep 7 to Sep 11)
        Assert.True(schedule[6].IsWorkDay);
        Assert.True(schedule[7].IsWorkDay);
        Assert.True(schedule[8].IsWorkDay);
        Assert.True(schedule[9].IsWorkDay);
        Assert.True(schedule[10].IsWorkDay);

        // Days 12-13: Off (Sep 12 to Sep 13)
        Assert.False(schedule[11].IsWorkDay);
        Assert.False(schedule[12].IsWorkDay);

        // Day 14: Next cycle start -> Work (Sep 14)
        Assert.True(schedule[13].IsWorkDay);
        Assert.Equal(new DateOnly(2026, 9, 14), schedule[13].Date);
    }

    [Fact]
    public void GenerateSchedule_WithStartTimesPool_CyclesStartTimesOnlyOnWorkDays()
    {
        // Arrange
        DateOnly startDate = new(2026, 9, 1);
        RotaPattern pattern = new()
        {
            Sequence = new List<int> { 3, -2 },
            StartTimesPool = new List<int> { 7, 8, 9 }
        };

        // Act
        List<ShiftDay> schedule = _engine.GenerateSchedule(startDate, pattern, 8);

        // Assert
        // Day 0: Work, 7:00
        Assert.True(schedule[0].IsWorkDay);
        Assert.Equal(new TimeOnly(7, 0), schedule[0].StartTime);

        // Day 1: Work, 8:00
        Assert.True(schedule[1].IsWorkDay);
        Assert.Equal(new TimeOnly(8, 0), schedule[1].StartTime);

        // Day 2: Work, 9:00
        Assert.True(schedule[2].IsWorkDay);
        Assert.Equal(new TimeOnly(9, 0), schedule[2].StartTime);

        // Day 3: Off, StartTime is null
        Assert.False(schedule[3].IsWorkDay);
        Assert.Null(schedule[3].StartTime);

        // Day 4: Off, StartTime is null
        Assert.False(schedule[4].IsWorkDay);
        Assert.Null(schedule[4].StartTime);

        // Day 5: Work, StartTime cycles back to 7:00
        Assert.True(schedule[5].IsWorkDay);
        Assert.Equal(new TimeOnly(7, 0), schedule[5].StartTime);

        // Day 6: Work, 8:00
        Assert.True(schedule[6].IsWorkDay);
        Assert.Equal(new TimeOnly(8, 0), schedule[6].StartTime);

        // Day 7: Work, 9:00
        Assert.True(schedule[7].IsWorkDay);
        Assert.Equal(new TimeOnly(9, 0), schedule[7].StartTime);
    }

    [Fact]
    public void GenerateSchedule_EmptyOrNullInputs_ReturnsEmptyList()
    {
        DateOnly startDate = new(2026, 9, 1);

        Assert.Empty(_engine.GenerateSchedule(startDate, null!, 10));
        Assert.Empty(_engine.GenerateSchedule(startDate, new RotaPattern(), 10));
        Assert.Empty(_engine.GenerateSchedule(startDate, new RotaPattern { Sequence = new List<int>() }, 10));
        Assert.Empty(_engine.GenerateSchedule(startDate, new RotaPattern { Sequence = new List<int> { 4, -2 } }, 0));
    }
}
