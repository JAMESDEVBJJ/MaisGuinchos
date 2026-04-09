using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow
{
    public class AcceptTowRequestResponseDto
    {
        public Guid TowRequestId { get; set; }
        public Guid TowTravelId { get; set; }
        public TowRequestStatus TowRequestStatus { get; set; }
    }
}
