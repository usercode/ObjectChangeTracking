using System;
using System.ComponentModel;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using ObjectChangeTracking.Abstractions;
using ObjectChangeTracking.Changes;
using Xunit.Abstractions;

namespace ObjectChangeTracking.Tests;

public class TestClass
{
    [Fact]
    public void SimplePropertyChange()
    {
        Item item = new Item();
        item.Name = "old";

        item = item.AsTrackable();
        item.Name = "new";

        ITrackableObject tracking = item.CastToTrackable();
        
        Assert.NotNull(tracking);
        Assert.Single(tracking.ChangedProperties);
        
        SimplePropertyChange c = tracking.ChangedProperties.Cast<SimplePropertyChange>().First();
        
        Assert.Equal(nameof(Item.Name), c.Name);
        Assert.Equal("old", c.OldValue);
        Assert.Equal("new", c.CurrentValue);
    }

    [Fact]
    public void NotifyPropertyChanged()
    {
        Item item = new Item() { Name = "abc" }.AsTrackable();

        INotifyPropertyChanged proxy = (INotifyPropertyChanged)item;

        bool raised = false;

        proxy.PropertyChanged += (obj, args) =>
        {
            if (item.Name == "123")
            {
                raised = true;
            }
        };

        item.Name = "123";

        Assert.True(raised);
    }

    [Fact]
    public void NotifyPropertyChanging()
    {
        Item item = new Item() { Name = "abc" }.AsTrackable();

        INotifyPropertyChanging proxy = (INotifyPropertyChanging)item;

        bool raised = false;

        proxy.PropertyChanging += (obj, args) =>
        {
            if (item.Name == "abc")
            {
                raised = true;
            }
        };

        item.Name = "123";

        Assert.True(raised);
    }

    [Fact]
    public void ChangeProperties()
    {
        Item item = new Item() { Name = "abc" }.AsTrackable();            
        item.Name = "123";

        ITrackableObject proxy = item.CastToTrackable();

        Assert.True(proxy.IsChanged);
        Assert.Single(proxy.ChangedProperties);
        Assert.True(proxy.ChangedProperties.First().Name == nameof(Item.Name));
    }

    [Fact]
    public void CollectionChanges()
    {
        IList<Item> list = new List<Item>().AsTrackableCollection();

        bool result = false;

        INotifyCollectionChanged notifyList = list as INotifyCollectionChanged;

        notifyList.CollectionChanged += (a, args) =>
        {
            result = true;
        };

        list.Add(new Item() { Name = "test" });

        ITrackableCollection<Item> listTracked = list.CastToTrackableCollection();

        Assert.True(result);
        Assert.Single(listTracked.Added);
        Assert.Empty(listTracked.Removed);

    }

    [Fact]
    public void CollectionChangesV2()
    {
        Item item = new Item();
        item.Childs.Add(new Item() { Name = "Test" });

        item =  item.AsTrackable();
        item.Childs.Add(new Item() { Name = "new" });

        ITrackableObject trackableObject = item.CastToTrackable();

        Assert.Equal(2, item.Childs.Count);
        Assert.Single(trackableObject.ChangedProperties);
        Assert.Single(trackableObject.ChangedProperties.Cast<CollectionPropertyChange>().First().Added);
        Assert.Empty(trackableObject.ChangedProperties.Cast<CollectionPropertyChange>().First().Removed);
    }

    [Fact]
    public void CollectionChildProxy()
    {
        Item item = new Item();
        item.Childs.Add(new Item() { Name = "123" });

        Item proxy = item.AsTrackable();

        proxy.Childs.Add(new Item() { Name = "test" });

        ITrackableCollection<Item> collectionProxy = proxy.Childs.CastToTrackableCollection();
        
        Assert.Single(collectionProxy.Added);
        Assert.Equal(2, item.Childs.Count);
        Assert.Equal(2, proxy.Childs.Count);
    }

    [Fact]
    public void CollectionItem()
    {
        IList<Item> list = new List<Item>().AsTrackableCollection();
        list.Add(new Item() { Name = "Test" });

        Item item = list[0];

        Assert.True(item is ITrackableObject);
    }
}
