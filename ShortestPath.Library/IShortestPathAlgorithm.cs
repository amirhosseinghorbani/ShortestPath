namespace ShortestPath.Library
{
    public interface IShortestPathAlgorithm
    {
        void Execute();
        string[] ShortestPath();
        long GraphSize { get; set; }
        Graph graph{ get; set; }
    }
}
