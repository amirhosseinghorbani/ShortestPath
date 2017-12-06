namespace ShortestPath.Library
{
    //An edge is a pointer to a vertex
    public struct Edge
    {
        //To property store the name of the vertex which it's pointing to
        public string To { get; set; } 
        //Weight property store the weight of the edge.
        public long Weight { get; set; }
    }
}
