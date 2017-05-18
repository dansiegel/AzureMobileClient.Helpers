# AzureMobileClient.Helpers

AzureMobileClient.Helpers is a lightweight toolkit for using the Microsoft Azure Mobile Client. It provides a set of abstractions and base classes that are based originally on the Samples from [Adrian Hall](https://adrianhall.github.io/develop-mobile-apps-with-csharp-and-azure/), along with a few tweaks to follow best practices with an interface based design.

Note that this library has been aligned with the Microsoft.Azure.Mobile.Client and is offered using NetStandard1.4 and as such is not compatible with traditional PCL projects. For this reason, it is recommended that you check out the [Prism Templates](https://github.com/dansiegel/Prism-Templates) I have available for `dotnet new` which use a NetStandard1.4 common library for the shared code.

| Package | Version |
|---------|---------|
| [AzureMobileClient.Helpers][11] | [![21]][11] |
| [AzureMobileClient.Helpers.DryIoc][12] | [![22]][12] |

## Setting up the library for Dependency Injection

The following examples are based on using DryIoc in a Prism Application:

```cs
protected override void RegisterTypes()
{
    Container.Register(typeof(ICloudTable<>), typeof(AzureCloudTable<>);
    Container.Register(typeof(ICloudSyncTable<>), typeof(AzureCloudSyncTable<>));
    Container.Register<AwesomeAppCustomerAppContext>(Reuse.Singleton);

    // If you are not using Authentication
    Container.RegisterInstance<IMobileServiceClient>(new MobileServiceClient(AppSettings.ApiEndpoint));

    // If you are using Authentication
    // If using Facebook or some other 3rd Party OAuth provider be sure to register ILoginProvider
    // in IPlatformServices in your Platform Project. If you are using a custom auth provider, you may
    // be able to author an ILoginProvider from shared code.
    Container.Register<IAzureCloudServiceOptions, AwesomeAppCloudServiceOptions>(Reuse.Singleton);
    Container.Register<ICloudService, AzureCloudService>(Reuse.Singleton);
}
```

```cs
public class AwesomeAppCloudServiceOptions : IAzureCloudServiceOptions
{
    public string AppServiceEndpoint => "https://yourappname.azurewebsites.net";
    public string AlternateLoginHost => string.Empty;
    public string LoginUriPrefix => string.Empty;
}

public class AwesomeAppCustomerAppContext : DryIocCloudAppContext
{
    public MyAppClient(IContainer container)
        // We can optionally pass in a database name
        : base(container, "myDatabaseName.db")
    {

    }

    public ICloudSyncTable<Customer> Customers => SyncTable<Customer>();
    public ICloudSyncTable<Invoice> Invoices => SyncTable<Invoice>();
    public ICloudSyncTable<InvoiceItem> InvoiceItems => SyncTable<InvoiceItem>();
    public ICloudTable<Feedback> Feedback => Table<Feedback>();

}

public class Customer : EntityData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class Invoice : EntityData
{
    public string CustomerId { get; set; }
}

public class InvoiceItem : EntityData
{
    public string InvoiceId { get; set; }
    public string ItemId { get; set; }
    public int Quantity { get; set; }
}

public class Feedback : EntityData
{
    public string Message { get; set; }
    public string Status { get; set; }
}
```


[11]: https://www.nuget.org/packages/AzureMobileClient.Helpers
[12]: https://www.nuget.org/packages/AzureMobileClient.Helpers.DryIoc

[21]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.svg
[22]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.DryIoc.svg