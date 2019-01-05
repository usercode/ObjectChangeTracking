# ObjectChangeTracking
It's a simple library to track property and collection changes in your objects.

https://www.nuget.org/packages/ObjectChangeTracking

Status: Pre-alpha

## How to use it ##

```csharp
var customer = new Customer();
customer.Lastname = "Doe";

customer = customer.AsTrackable();
customer.Firstname = "John";

var trackableCustomer = (ITrackableObject)customer;

bool isChanged = trackableCustomer.IsChanged; // -> true

foreach(IPropertyChange changedProperty in trackableCustomer.ChangedProperties)
{
    Console.WriteLine(changedProperty.Name); // -> "Firstname"
}
```
