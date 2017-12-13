using System;

namespace ShortestPath.Library
{

    //Any algorithm must derive IShortestPathAlgorithm
    public interface IShortestPathAlgorithm
    {
        string Name { get; }
        void Execute();
        string[] ShortestPath(long from, long to);
        Graph Graph{ get; }

        event EventHandler Start;
        event EventHandler End;
        event EventHandler<StepChangedEventArgs> StepChanged;
        event EventHandler<NewShortPathFoundEventArgs> NewShortPathFound;
    }
}