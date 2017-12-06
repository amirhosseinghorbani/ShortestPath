namespace ShortestPath.Library
{
    /*
     * Edges class manage the edges of a vertex.
     * It stores and reterives the edges of a vertex
     */
    public class Edges
    {
        private long eSize;
        private Edge[] edges; //
        public long ESize { get => eSize; set => eSize = value; } //it hold the number of edges.
        public Edge this[long index] //an indexer which let the vertex and algorithm to manipulate the edges.
        {
            get => edges[index];
            set => edges[index] = value;
        }

        //Get the size of edges and make them.
        public Edges(long Size)
        {
            ESize = Size; // get the number of edges.
            edges = new Edge[ESize]; // make an array of edges.
        }

    }
}
