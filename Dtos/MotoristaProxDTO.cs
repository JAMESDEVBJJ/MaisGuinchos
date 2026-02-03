namespace MaisGuinchos.Dtos
{
    public class MotoristaProxDTO
    {
        public MotoristaComLoc? Motorista { get; set; }

        public double DistanceKm { get; set; }
    }   

    public class MotoristaComLoc
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }


        public double Lat { get; set; }

        public double Lon { get; set; }
    }
}
