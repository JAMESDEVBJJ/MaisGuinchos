using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow
{
    public class RejectTowRequestResponseDTO
    {
        public Guid TowRequestId { get; set; }
        public TowRequestStatus TowRequestStatus { get; set; }
    }

    public class CancelTowRequestResponseDTO
    {
        public Guid TowRequestId { get; set; }
        public TowRequestStatus TowRequestStatus { get; set; }
    }
}
