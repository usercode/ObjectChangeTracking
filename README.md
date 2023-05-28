# ObjectChangeTracking
It's a simple library to track property and collection changes in your objects.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/ObjectChangeTracking.svg?style=flat-square)](https://www.nuget.org/packages/ObjectChangeTracking/)

## How to use it ##

```csharp
var customer = new Customer();
customer.Lastname = "Doe";
customer.Firstname = "Jack";

customer = customer.AsTrackable();
customer.Firstname = "John";

var trackableCustomer = (ITrackableObject)customer;

bool isChanged = trackableCustomer.IsChanged; // -> true

foreach(IPropertyChange changedProperty in trackableCustomer.ChangedProperties)
{
    if(changedProperty is SimplePropertyChange simpleProperty)
    {
        Console.WriteLine(simpleProperty.Name); // -> "Firstname"
        Console.WriteLine(simpleProperty.OldValue); // -> "Jack"
        Console.WriteLine(simpleProperty.CurrentValue); // -> "John"        
    }
    else if(changedProperty is CollectionPropertyChange collectionProperty)
    {
        Console.WriteLine(collectionProperty.Name);
        
        foreach(var added in collectionProperty.Added)
        {
             Console.WriteLine("Added: " + added);
        }
        
        foreach(var removed in collectionProperty.Removed)
        {
             Console.WriteLine("Removed: " + removed);
        }
    }
}
```
