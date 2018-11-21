using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Abstractions
{
    public interface IChangedProperty
    {
        String Name { get; }
    }
}
