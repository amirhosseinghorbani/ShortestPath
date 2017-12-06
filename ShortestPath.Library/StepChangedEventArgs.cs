using System;

namespace ShortestPath.Library
{
    //this will be triggered when a step of algorithm is completed and it's going to start the new step.
    public class StepChangedEventArgs : EventArgs
    {
        public long step;
        public StepChangedEventArgs(long newstep)
        {
            step = newstep;
        }
    }
}
