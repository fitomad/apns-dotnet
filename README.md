# APNS
A nuget package to work with the Apple Push Notification Service.


## Enumeration Class implementation

I adopt an enumeration class implementation approach instead of the common enumeration type as proposed in the *".NET Microservices: Architecture for Containerized .NET Applications"* book available at dotnet Microsoft website.

The base class for this enumeration classes in the `ApnsEnumeration` abstract class that defines a Key-Value based enumeration cases definition. 

It also implements the `IApnsRepresentable` protocol, that defines a way to share enumetation values as APNS service expects and the `IEquatable` and `IComparable` interfaces.


