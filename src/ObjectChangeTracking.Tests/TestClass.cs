using System;
using System.ComponentModel;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using ObjectChangeTracking.Abstractions;

namespace ObjectChangeTracking.Tests
{
    public class TestClass
    {
        [Fact]
        public void TestA()
        {
            Item item = new Item();

            Item proxy = item.AsTrackable();

            proxy.Name = "123";

            ITrackableObject tracking = proxy.CastToTrackable();
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
            Assert.True(proxy.ChangedProperties.Count() == 1);
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
            Assert.True(listTracked.Added.Count() == 1);

        }

        [Fact]
        public void CollectionChangesV2()
        {
            Item item = new Item();
            item.Childs.Add(new Item() { Name = "Test" });

            var trackableItem =  item.AsTrackable();
            
            Assert.True(trackableItem.Childs.Count() == 1);

        }

        [Fact]
        public void CollectionChildProxy()
        {
            Item item = new Item();
            item.Childs.Add(new Item() { Name = "123" });

            Item proxy = item.AsTrackable();

            proxy.Childs.Add(new Item() { Name = "test" });

            ITrackableCollection<Item> collectionProxy = proxy.Childs.CastToTrackableCollection();
            
            Assert.True(collectionProxy.Added.Count() == 1);
            Assert.True(item.Childs.Count == 2);
            Assert.True(proxy.Childs.Count == 2);
        }

        [Fact]
        public void CollectionIItem()
        {
            IList<Item> list = new List<Item>().AsTrackableCollection();
            list.Add(new Item() { Name = "Test" });

            Item item = list[0];

            Assert.True(item is ITrackableObject);
        }
    }
}
