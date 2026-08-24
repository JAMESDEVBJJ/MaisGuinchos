using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow
{
    public class RejectTowRequestResponseDTO
    {
        public Guid Id { get; set; }
        public TowRequestStatus TowRequestStatus { get; set; }

        public string DriverName { get; set; }
    }

    public class CancelTowRequestResponseDTO
    {
        public Guid Id { get; set; }
        public TowRequestStatus TowRequestStatus { get; set; }
    }
}
