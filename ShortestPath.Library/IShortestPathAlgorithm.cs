using System;

namespace ShortestPath.Library
{

    //Any algorithm must derive IShortestPathAlgorithm
    public interface IShortestPathAlgorithm
    {
        string Name { get; set; }
        void Execute();
        string[] ShortestPath();
        Graph graph{ get; set; }

        event EventHandler Start;
        event EventHandler End;
        event EventHandler<StepChangedEventArgs> StepChanged;
        event EventHandler<NewShortPathFoundEventArgs> NewShortPathFound;
    }
}