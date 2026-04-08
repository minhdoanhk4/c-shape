using System;
using CFMS.FranchiseService.Core.Enums;

namespace CFMS.FranchiseService.API.DTOs
{
    public class CreateFranchiseRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string OpenTime { get; set; } // "HH:mm"
        public string CloseTime { get; set; } // "HH:mm"
        public FranchiseType Type { get; set; }
    }

    public class UpdateFranchiseRequest : CreateFranchiseRequest
    {
        public FranchiseStatus Status { get; set; }
    }

    public class FranchiseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public FranchiseType Type { get; set; }
        public FranchiseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
