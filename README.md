# ObjectChangeTracking
It's a simple library to track property and collection changes in your objects.

https://www.nuget.org/packages/ObjectChangeTracking/

## How to use it ##

```csharp
ITrackableObject trackableObject = new Object().AsTrackable();

bool isChanged = trackableObject.IsChanged;
IEnumerable<IChangedProperty> changedProperties = trackableObject.ChangedProperties;
```
