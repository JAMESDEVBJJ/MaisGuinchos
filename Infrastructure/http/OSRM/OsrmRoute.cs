namespace MaisGuinchos.Infrastructure.http.OSRM
{
    public class OsrmRoute
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public OsrmGeometry geometry { get; set; }
    }

    public class OsrmGeometry
    {
        public List<List<double>> coordinates { get; set; }
    }
}
