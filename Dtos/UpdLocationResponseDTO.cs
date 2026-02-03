namespace MaisGuinchos.Dtos
{
    public class UpdLocationResponseDTO
    {
        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public string? DisplayName { get; set; }

        public UserSummaryDTO? User { get; set; }
    }

    public class UserSummaryDTO
    {
        public Guid? Id { get; set; }

        public string? UserName { get; set; }
    }
}
