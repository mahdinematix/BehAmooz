﻿namespace StudyManagement.Application.Contracts.Order
{
    public class OrderSearchModel
    {
        public long AccountId { get; set; }
        public bool IsCanceled { get; set; }
        public string IssueTrackingNo { get; set; }
    }
}
