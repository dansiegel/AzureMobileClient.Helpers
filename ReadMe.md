# AzureMobileClient.Helpers

AzureMobileClient.Helpers is a lightweight toolkit for using the Microsoft Azure Mobile Client. It provides a set of abstractions and base classes that are based originally on the Samples from [Adrian Hall](https://adrianhall.github.io/develop-mobile-apps-with-csharp-and-azure/), along with a few tweaks to follow best practices with an interface based design.

Note that this library has been aligned with the Microsoft.Azure.Mobile.Client and is offered using NetStandard1.4 and as such is not compatible with traditional PCL projects. For this reason, it is recommended that you check out the [Prism Templates](https://github.com/dansiegel/Prism-Templates) I have available for `dotnet new` which use a NetStandard1.4 common library for the shared code.

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
    Container.Register<ICloudService, AzureCloudService>(Reuse.Singleton)
    );
}
```

```cs
public class AwesomeAppCloudServiceOptions : IAzureCloudServiceOptions
{
    public string AppServiceEndpoint => "https://yourappname.azurewebsites.net";
}

public class AwesomeAppCustomerAppContext
{
    public MyAppClient(ICloudSyncTable<Customer> customers, ICloudSyncTable<Invoice> invoices,
                       ICloudSyncTable<InvoiceItem> invoiceItems, ICloudTable<Feedback> feedback)
    {
        Customers = customers;
        Invoices = invoices;
        InvoiceItems = invoiceItems;
        Feedback = feedback;
    }

    public ICloudSyncTable<Customer> Customers { get; }
    public ICloudSyncTable<Invoice> Invoices { get; }
    public ICloudSyncTable<InvoiceItem> InvoiceItems { get; }
    public ICloudTable<Feedback> Feedback { get; }

}

public class Customer : TableData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class Invoice : TableData
{
    public string CustomerId { get; set; }
}

public class InvoiceItem : TableData
{
    public string InvoiceId { get; set; }
    public string ItemId { get; set; }
    public int Quantity { get; set; }
}

public class Feedback : TableData
{
    public string Message { get; set; }
    public string Status { get; set; }
}
```


