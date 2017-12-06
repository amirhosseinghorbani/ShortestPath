namespace ShortestPath.Library
{
    /*
     * Graph class contains vertices of a graph and every vertex has some edges.
     * graph class manage the vertices and edges.
     */
    public class Graph
    {
        private long _size;
        private Vertex[] _vertices;
        public long Size { get => _size; } //return the number of vertices
        public Vertex[] Vertices { get => _vertices; }

        public Vertex this[long index] //indexer to let algorithm manipulate the graph
        {
            get => _vertices[index];
            set => _vertices[index] = value;
        }

        public Graph(long size)
        {
            this._size = size;
            _vertices = new Vertex[Size];
        }
        public Graph(Vertex[] vertices) //get a vertex array and use it as the graph
        {
            _vertices = vertices;
        }
        public long VertexIndex(Vertex vertex) //return the index of vertex in the graph array.
        {
            for (int i = 0; i < Size; i++)
                if (this[i].Equals(vertex))
                    return i;
            return -1;
        }
    }
}
