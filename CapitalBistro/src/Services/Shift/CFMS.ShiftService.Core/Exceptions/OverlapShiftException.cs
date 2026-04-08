using System;

namespace CFMS.ShiftService.Core.Exceptions
{
    public class OverlapShiftException : Exception
    {
        public OverlapShiftException(string message) : base(message)
        {
        }
    }
}
