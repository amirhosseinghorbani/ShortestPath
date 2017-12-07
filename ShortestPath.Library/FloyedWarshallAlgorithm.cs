using System;
using System.Collections.Generic;
using System.Text;

namespace ShortestPath.Library
{
    public class FloyedWarshallAlgorithm : IShortestPathAlgorithm
    {
        private const long Z = 0;
        private const long INF = int.MaxValue;
        private Graph _graph;
        public string Name
        {
            get => "Floyed Warshal Algorithm";
        }
        public Graph Graph { get => _graph; }

        public string[,] Paths;

        public event EventHandler Start;
        public event EventHandler End;
        public event EventHandler<StepChangedEventArgs> StepChanged;
        public event EventHandler<NewShortPathFoundEventArgs> NewShortPathFound;

        public FloyedWarshallAlgorithm(Graph graph)
        {
            _graph = graph;
            Paths = new string[Graph.Size, Graph.Size];
        }

        //convert the graph into a matrix
        public long[,] ValuesToArray()
        {
            long[,] arr = new long[Graph.Size, Graph.Size];
            for (long i = 0; i < Graph.Size; i++)
                for (long j = 0; j < Graph.Size; j++)
                    arr[i, j] = Graph[i].Edges[j].Weight;
            return arr;
        }

        //every vertex has 0 step to itself.
        private void Initializing()
        {
            for (long i = 0; i < Graph.Size; i++)
                for (long j = 0; j < Graph.Size; j++)
                {
                    if (i == j)
                        Graph[i].SetEdge(j, new Edge() { To = Graph[i].Name, Weight = Z });
                    else
                        Graph[i].SetEdge(j, new Edge() { To = Graph[i].Name });
                }
        }

        public void Execute()
        {
            Initializing();
            OnStart(); //trigger the Start event
            for (long k = 0; k < Graph.Size; k++)
            {
                for (long i = 0; i < Graph.Size; i++)
                {
                    for (long j = 0; j < Graph.Size; j++)
                    {
                        if (Graph[i].Edges[k].Weight + Graph[k].Edges[j].Weight < Graph[i].Edges[j].Weight)
                        {
                            Graph[i].SetEdge(j, new Edge() //Change the old path(longer path) to new path(shorter path)
                            {
                                Weight = Graph[i].Edges[k].Weight + Graph[k].Edges[j].Weight, // calculate the shorter path
                                To = Graph[i].Name //Save the name of next path
                            });
                            OnNewShortPathFound(new NewShortPathFoundEventArgs($"{Graph[i].Edges[k].To} => {Graph[i].Edges[j].To}"));
                        }
                    }
                }
                OnStepChanged(new StepChangedEventArgs(k)); //trigger the StepChanged event
            }
            OnEnd(); //trigger the End event
        }
        protected virtual void OnStart() => Start?.Invoke(this, EventArgs.Empty);
        protected virtual void OnEnd() => End?.Invoke(this, EventArgs.Empty);
        protected virtual void OnStepChanged(StepChangedEventArgs ev) => StepChanged?.Invoke(this, ev);
        protected virtual void OnNewShortPathFound(NewShortPathFoundEventArgs ev) => NewShortPathFound?.Invoke(this, ev);

        public string[] ShortestPath()
        {
            throw new NotImplementedException();
        }
    }
}
