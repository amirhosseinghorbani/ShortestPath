using System;

namespace ShortestPath.Library
{
    /*
     * It will be triggerd when algorithm finds a short path.
     * the argument is the new path.
     */
    public class NewShortPathFoundEventArgs : EventArgs 
    {
        public string newShortPath;
        public NewShortPathFoundEventArgs(string newShortPathFound)
        {
            newShortPath = newShortPathFound;
        }
    }
}
