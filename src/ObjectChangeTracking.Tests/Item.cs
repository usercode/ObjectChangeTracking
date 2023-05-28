using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectChangeTracking.Tests;

public class Item
{
    public Item()
    {
        Childs = new List<Item>();
    }

    public virtual IList<Item> Childs { get; }

    public virtual String Name { get; set; }

    public virtual int Value { get; set; }
}
