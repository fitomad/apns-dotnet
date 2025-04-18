# APNS NuGet Package

This package simplifies sending notifications via the Apple Push Notification Service (APNS). Designed for ease of use, flexibility, and compatibility with modern C# development practices, this package supports token-based and certificate-based authentication, advanced notification customization, and error handling.

## Features

- **Token-Based and Certificate-Based Authentication**: Configure APNS authentication using either JSON Web Token (JWT) or X509 certificates.
- **Flexible Notification Building**: Customize notifications with alerts, badges, sounds, live activity, relevance scores, interruption levels, and more.
- **Error Handling**: Comprehensive error definitions to handle and respond to APNS errors effectively.
- **Environment Configuration**: Set up easily for development or production environments.

## APNS connection

Please, refer to Apple official documentation to create the certificate or JWT Key used to establish and send a notification request

- [Establishing a connection to Apple Push Notification service (APNs)](https://developer.apple.com/documentation/usernotifications/establishing-a-connection-to-apns)
- [Establishing a token-based connection to APNs](https://developer.apple.com/documentation/usernotifications/establishing-a-token-based-connection-to-apns)
- [Establishing a certificate-based connection to APNs](https://developer.apple.com/documentation/usernotifications/establishing-a-certificate-based-connection-to-apns)

## Installation

To install the package, use:

```bash
dotnet add package Apns
```

## Dependency Injection. Create an `ApnsClient` instance

To create an `ApnsClient` instance, the entry point to the whole Fitomad.Apns framework, developers must use DI.

I provide a helper method registered as an `IServiceCollection` extension named `AddApns` which receives an `ApnsSettings` object as parameter. You can create an ApnsSettings object using the `ApnsSettingsBuilder` object.

This is an example of DI in an Unit Testing (xunit) environment.

```csharp
var testSettings = new ApnsSettingsBuilder()
    .InEnvironment(ApnsEnvironment.Development)
    .SetTopic("com.desappstre.Smarty")
    .WithPathToX509Certificate2(certPath, certPassword)
    .Build();

var services = new ServiceCollection();
services.AddApns(settings: testSettings);
var provider = services.BuildServiceProvider();
```

And now, thanks to the built-on DI container available in .NET we can use the `ApnsClient` registered type.

## Getting Started

### Register APNS Service

Use the `AddApns` method to configure the APNS client. First you have to create the APNS connection settings using the `ApnsSettingsBuilder` class to set values for the following properties:

- APNS environment. Apple defines two different environments, **Production** and **Development**. Use the `InEnvironment()` method to set the desired environment passing a `ApnsEnvironment` value
- APNS topic. It's the `apns-topic` HTTP header and must be the application bundle identifier for the application that will receive our notifications. Use the `SetTopic()` method to pass a `string` value with the bundle id.
- X509 certificate. Use the three different methods to set the X509 certificate used to sign all the APNS notification requests.
  - `WithCertificate(ApnsCertificate certificate)`: Use a custom `ApnsCertificate` structure with the certificate information.
  - `WithX509Certificate2(X509Certificate2 certificate)`: A .NET `X509Certificate2` class
  - `WithPathToX509Certificate2(string pathToCertificate, string password)`: Pass the path to the certificate file and the associated password
- JWT signed. Apple also brings the chance to use a JWT to sign the notification request. If this is your case please use the following methods:
  - `WithJsonToken(ApnsJsonToken jsonToken)`. Use a custom `ApnsJsonToken` structure to set all the information required to apply the JWT sign
  - `WithPathToJsonToken(string pathToJwt, string keyId, string teamId)` Pass the path to the JWT key file and also the key id provided by Apple. The package also needs the Team Identifier available in your Apple Developer Program account.

```csharp
string certPath = Environment.GetEnvironmentVariable("Apns:CertPath");
string certPassword = Environment.GetEnvironmentVariable("Apns:CertPassword");

// Set APNS connection settings
var developmentSettings = new ApnsSettingsBuilder()
    .InEnvironment(ApnsEnvironment.Development)
    .SetTopic("com.desappstre.Smarty")
    .WithPathToX509Certificate2(certPath, certPassword)
    .Build();

var services = new ServiceCollection();
services.AddApns(settings: developmentSettings);

var provider = services.BuildServiceProvider();
```

And now take a look to an example using a JWT key to sign the notification requests.

```csharp
var jwtInformation = new ApnsJsonToken
{
    Content = Environment.GetEnvironmentVariable("Apns:JwtContent"),
    KeyId = Environment.GetEnvironmentVariable("Apns:JwtKey"),
    TeamId = Environment.GetEnvironmentVariable("Apns:TeamId")
};

// Set APNS connection settings
var developmentSettings = new ApnsSettingsBuilder()
    .InEnvironment(ApnsEnvironment.Production)
    .SetTopic("com.desappstre.Smarty")
    .WithJsonToken(jwtInformation)
    .Build();

var services = new ServiceCollection();
services.AddApns(settings: developmentSettings);

var provider = services.BuildServiceProvider();
```

## Build Notifications

Create notifications using the INotificationBuilder interface:

```csharp
var alertContent = new Alert()
{
    Title = "Test Alert",
    Subtitle = "Test Subtitle",
    Body = "Test Body"
};

Notification notification = new NotificationBuilder()
    .WithAlert(alertContent)
    .Build();

ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: _deviceToken);
```

```csharp
var alertContent = new LocalizableAlert()
{
    TitleLocalizationKey = "push_title_arg",
    TitleLocalizationArguments = [ "España" ],
    SubtitleLocalizationKey = "push_subtitle_arg",
    SubtitleLocalizationArguments = [ "Madrid" ],
    BodyLocalizationKey = "push_body_arg",
    BodyLocalizationArguments = [ "❤️" ]
};

Notification notification = new NotificationBuilder()
    .WithAlert(alertContent)
    .Build();

ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: _deviceToken);
```

### Live Activity 

In case you need to send a Live Activity notification you can use the same `NotificationBuilder` object used to create regular notification, but once you set the common notification properties, you must invoke the `UseLiveActivity` method to switch to the Live Activity properties set methods.

You have to set the `PushType` property to `NotificationType.LiveActivity` in the `NotificationSettings` object passed to the `SendAsync` method available in the `ApnsClient` object.

Setting the PushType property also modify the notification topic adding the proper suffix, in this case `.push-type.liveactivity`

```csharp
var notificationSettings = new NotificationSettings
{
    PushType = NotificationType.LiveActivity
};

var alert = new Alert
{
    Title = "Live Activity Title",
    Body = "Live Activity Body"
};

var dataContent = new Dictionary<string, object>
{
    { "name", "@fitomad" }
};

var liveActivityNotification = new NotificationBuilder()
    .WithAlert(alert)
    .UseLiveActivity()
    .SetTimestamp(DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds())
    .SetLiveActivityEvent(LiveActivityEvent.Start)
    .SetLiveActivityAttributeType("MockAttributeType")
    .SetLiveActivityStartAttributes(dataContent)
    .WithLiveActivityContent(dataContent)
    .Build(); 

ApnsResponse response = await _client.SendAsync(liveActivityNotification, 
                                                notificationSettings: notificationSettings, 
                                                deviceToken: _deviceToken);
```

## Notification request response

The response from the APNS service is mapped using the `ApnsResponse` class, that implements a Result pattern, that means that instead of throws and exception in case somethign goes wrong, the `ApnsResponse` class indicates the operation status using the `IsSuccess` property.

There are two additional properties that are fulfill depends on the response.

- **Success**: You can check the `Guid` property that returns an `ApnsGuid` record that contains the notification identifier and the unique identifier provided by Apple.
- **Failure**: Check the `Error` property that implements an `ApnsError` record with the error code and description provided by Apple.

```csharp
ApnsResponse response = await _client.SendAsync(liveActivityNotification, 
                                                notificationSettings: notificationSettings, 
                                                deviceToken: _deviceToken)
                                                
if(apnsResponse is { IsSuccess: true })
{
    // Notification send
}
else
{
    // Failure
}
```

## Testing

You must set some user-secrets to run the unit-test project

```text
Apns:TeamId = TEAM-ID
Apns:KeyId = KEY-ID
Apns:JwtKeyPath = /path/to/jwt-key
Apns:DeviceToken = DEVICE-TOKEN
Apns:CertPath = /path/to/certificate.p12
Apns:CertPassword = PASSWORD
```

## Enumeration Class implementation

I adopt the enumeration class implementation approach instead of the common enumeration type as proposed in the *".NET Microservices: Architecture for Containerized .NET Applications"* book available at dotnet Microsoft website.

The base class for this enumeration classes in the `ApnsEnumeration` abstract class that defines a Key-Value based enumeration cases definition. 

It also implements the `IApnsRepresentable` protocol, that defines a way to share enumetation values as APNS service expects and the `IEquatable` and `IComparable` interfaces.

## Fluent API design for internal rules validation

Fluent API Design is a programming style that prioritizes easy-to-read, chainable method calls to build complex queries, configurations, or behaviors in a more intuitive and elegant manner. The design mimics natural language, allowing developers to interact with APIs in a flow that feels logical and sequential. This is widely used for configurations, querying data, or rule-building frameworks, like the one you've designed.

### Key Highlights Fluent API Design:

- **Ease of Chaining**:
  - Methods like `Where`, `VerifyThat`, `OnFailure`, and `OnSuccess` allow logical operations to be chained fluently. For example, a developer can construct a sequence like:
    
    ```csharp
    rule.Where(someCondition)
        .VerifyThat(anotherCondition)
        .OnFailure(() => HandleError())
        .OnSuccess(() => HandleSuccess());
    ```
    This makes the code expressive and clean.

- **Flexibility with Conditions**:
  - By accepting `Func<bool>` as input parameters in methods like `Where` and `VerifyThat`, the API enables developers to specify custom conditions dynamically.

- **Failure and Success Actions**:
  - The `OnFailure` and `OnSuccess` methods let users attach callbacks or actions, making error handling or success behaviors customizable.

- **Handling Properties**:
  - The `Property<TValue>` method introduces a generic interface for managing values, enforcing constraints like `IEquatable<TValue>` and `IComparable<TValue>`. This ensures that comparisons and validations can be performed effectively.

  - **Extended Rules for Properties**:
    - The `IProperty<TValue>` interface further provides methods to validate properties, such as `IsEqualsTo`, `InRange`, `IsNull`, `IsNotNull`, and pattern matching with `MatchRegularExpression`. This adds flexibility for handling diverse validation needs with succinct chaining:
    
    ```csharp
      property.IsNotNull()
              .InRange(1, 10)
              .MatchRegularExpression(@"\d+");
      ```

### Practical Usage for APNS Rule Validation:

This Fluent API approach is well-suited for APNS rule validation, as it simplifies defining complex rule sets. For example:

```csharp
var rule = new Rule()
            .VerifyThat(() => isDeviceTokenValid)
            .OnFailure(() => LogError("Invalid Token"))
            .OnSuccess(() => SendNotification());
```



