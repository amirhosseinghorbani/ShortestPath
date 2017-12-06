using System;

namespace ShortestPath.Library
{

    //Any algorithm must derive IShortestPathAlgorithm
    public interface IShortestPathAlgorithm
    {
        void Execute();
        string[] ShortestPath();
        long GraphSize { get; set; }
        Graph graph{ get; set; }

        event EventHandler Start;
        event EventHandler End;
        event EventHandler<StepChangedEventArgs> StepChanged;
        event EventHandler<NewShortPathFoundEventArgs> NewShortPathFound;
    }
}