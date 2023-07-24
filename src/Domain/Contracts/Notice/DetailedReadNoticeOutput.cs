﻿namespace Domain.Contracts.Notice
{
    public class DetailedReadNoticeOutput : BaseNoticeContract
    {
        public Guid? Id { get; set; }
        public string? DocUrl { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}