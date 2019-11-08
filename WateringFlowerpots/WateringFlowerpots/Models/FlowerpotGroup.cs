using System;
using System.Collections.Generic;

namespace WateringFlowerpots.Models
{
    public class FlowerpotGroup : List<Flowerpot>
    {
        public string Name { get; private set; }

        public FlowerpotGroup(string name, List<Flowerpot> flowerpots) : base(flowerpots)
        {
            Name = name;
        }
    }
}
