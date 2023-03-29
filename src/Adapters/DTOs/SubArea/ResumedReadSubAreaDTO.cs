﻿using Adapters.DTOs.Base;

namespace Adapters.DTOs.SubArea
{
    public class ResumedReadSubAreaDTO : ResponseDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}