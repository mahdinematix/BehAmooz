﻿namespace StudyManagement.Application.Contracts.Class;

public class ClassSearchModel
{
    public string Code { get; set; }
    public string StartTime { get; set; }
    public bool IsActive { get; set; }
    public int DayId { get; set; }

}