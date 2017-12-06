namespace ShortestPath.Library
{
    /*
     *Vertex class define and manage the requirements of a vertex
     *any vertex has a name and some edges. 
     */
    public struct Vertex
    {
        private string _name;
        private long _deg;
        private Edges _edges;
        public string Name { get => _name; } //return a name for vertex instance.
        public long Degree { get => _deg; } //return the degree of the vertex.
        public Edges Edges { get => _edges; } //return the edges.

        public Vertex(string name, long edgesSize)
        {
            _name = name; // get the name
            _deg = edgesSize; //get the degree of the vertex.
            _edges = new Edges(_deg); //allocate the memory and make an array of edges for this vertex
        }

        public Vertex(string name, long edgesSize, Edges edges) : this(name, edgesSize)
        {
            _edges = edges; //get the edges
        }

        public void SetEdge(long index, Edge obj) //get an edge and set it to edges
        {
            if (index >= 0 && index <= Degree)
                this._edges[index] = obj;
        }

        public override bool Equals(object obj) //checking an object if it's the same object, return true
        {
            if (obj.GetHashCode().Equals(this.GetHashCode()))
                return true;
            return false;
        }

        public override string ToString() //overriding the ToString method and return useful
        {
            return $"Vertex {Name} with {Degree} edges.";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
