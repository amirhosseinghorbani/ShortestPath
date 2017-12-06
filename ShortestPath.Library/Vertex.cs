namespace ShortestPath.Library
{

    public struct Vertex
    {
        private string _name;
        private long _eSize;
        private Edges _edges;
        public string Name { get => _name; set => _name = value; }
        public long ESize { get => _eSize; set => _eSize = value; }
        public Edges Edges { get => _edges; set => _edges = value; }

        public Vertex(string name, long edgesSize)
        {
            _name = name;
            _eSize = edgesSize;
            _edges = new Edges(_eSize);
        }

        public Vertex(string name, long edgesSize, Edges edges) : this(name, edgesSize)
        {
            _edges = edges;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetHashCode().Equals(this.GetHashCode()))
                return true;
            return false;
        }
    }
}
