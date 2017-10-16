# AzureMobileClient.Helpers

AzureMobileClient.Helpers is a lightweight toolkit for using the Microsoft Azure Mobile Client. It provides a set of abstractions and base classes that are based originally on the Samples from [Adrian Hall](https://adrianhall.github.io/develop-mobile-apps-with-csharp-and-azure/), along with a few tweaks to follow best practices with an interface based design.

Note that this library has been aligned with the Microsoft.Azure.Mobile.Client and is offered using NetStandard1.4 and as such is not compatible with traditional PCL projects. For this reason, it is recommended that you check out the [Prism Templates](https://github.com/dansiegel/Prism-Templates) I have available for `dotnet new` which use a NetStandard1.4 common library for the shared code.

| Package | Version | MyGet |
|---------|---------|-------|
| [AzureMobileClient.Helpers][HelpersNuGet] | [![HelpersShield]][HelpersNuGet] | [![HelpersMyGetShield]][HelpersMyGet] |
| [AzureMobileClient.Helpers.Autofac][HelpersAutofacNuGet] | [![HelpersAutofacShield]][HelpersAutofacNuGet] | [![HelpersAutofacMyGetShield]][HelpersAutofacMyGet] |
| [AzureMobileClient.Helpers.DryIoc][HelpersDryIocNuGet] | [![HelpersDryIocShield]][HelpersDryIocNuGet] | [![HelpersDryIocMyGetShield]][HelpersDryIocMyGet] |
| [AzureMobileClient.Helpers.SimpleInjector][HelpersSimpleInjectorNuGet] | [![HelpersSimpleInjectorShield]][HelpersSimpleInjectorNuGet] | [![HelpersSimpleInjectorMyGetShield]][HelpersSimpleInjectorMyGet] |
| [AzureMobileClient.Helpers.Unity][HelpersUnityNuGet] | [![HelpersUnityShield]][HelpersUnityNuGet] | [![HelpersUnityMyGetShield]][HelpersUnityMyGet] |
| [AzureMobileClient.Helpers.AzureActiveDirectory][HelpersAADNuGet] | [![HelpersAADShield]][HelpersAADNuGet] | [![HelpersAADMyGetShield]][HelpersAADMyGet] |

## Support

If this project helped you reduce time to develop and made your app better, please help support this project.

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.me/dansiegel)

## Resources

- [Getting Started Tutorial](https://dansiegel.net/post/2017/05/23/azure-mobile-client-helpers)
- [Todo Demo](https://github.com/dansiegel/TodoDemo)

## Setting up the library for Dependency Injection

The following examples are based on using DryIoc in a Prism Application:

```cs
protected override void RegisterTypes()
{
    // ICloudTable is only needed for Online Only data
    Container.Register(typeof(ICloudTable<>), typeof(AzureCloudTable<>), Reuse.Singleton);
    Container.Register(typeof(ICloudSyncTable<>), typeof(AzureCloudSyncTable<>), Reuse.Singleton);

    Container.UseInstance<IPublicClientApplication>(new PublicClientApplication(Secrets.AuthClientId, AppConstants.Authority)
    {
        RedirectUri = AppConstants.RedirectUri
    });

    Container.RegisterMany<AADOptions>(reuse: Reuse.Singleton,
                                       serviceTypeCondition: type =>
                                                type == typeof(IAADOptions) ||
                                                type == typeof(IAADLoginProviderOptions));

    Container.Register<IAzureCloudServiceOptions, AppServiceContextOptions>(Reuse.Singleton);
    Container.RegisterMany<AppDataContext>(reuse: Reuse.Singleton,
                                           serviceTypeCondition: type => 
                                                type == typeof(IAppDataContext) ||
                                                type == typeof(ICloudService));
    Container.RegisterDelegate<IMobileServiceClient>(factoryDelegate: r => r.Resolve<ICloudService>().Client,
                                                     reuse: Reuse.Singleton,
                                                     setup: Setup.With(allowDisposableTransient: true));
    Container.Register<ILoginProvider<AADAccount>,LoginProvider>(Reuse.Singleton);
}
```

```cs
public class AwesomeAppCloudServiceOptions : IAzureCloudServiceOptions
{
    public string AppServiceEndpoint => "https://yourappname.azurewebsites.net";
    public string AlternateLoginHost => string.Empty;
    public string LoginUriPrefix => string.Empty;
    public HttpMessageHandler[] Handlers => new HttpMessageHandler[0];
}

public class AwesomeAppCustomerAppContext : DryIocCloudAppContext
{
    public MyAppClient(IContainer container)
        // We can optionally pass in a database name
        : base(container, "myDatabaseName.db")
    {

    }

    /*
     * NOTE: This is architected to be similar to Entity Framework in that
     * the CloudAppContext will look for properties that are ICloudSyncTable<>
     * so that it can register the Model type with the SQLite Store.
     */
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

[HelpersNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers
[HelpersAutofacNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers.Autofac
[HelpersDryIocNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers.DryIoc
[HelpersSimpleInjectorNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers.SimpleInjector
[HelpersUnityNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers.Unity
[HelpersAADNuGet]: https://www.nuget.org/packages/AzureMobileClient.Helpers.AzureActiveDirectory

[HelpersShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.svg
[HelpersAutofacShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.Autofac.svg
[HelpersDryIocShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.DryIoc.svg
[HelpersSimpleInjectorShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.SimpleInjector.svg
[HelpersUnityShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.Unity.svg
[HelpersAADShield]: https://img.shields.io/nuget/vpre/AzureMobileClient.Helpers.AzureActiveDirectory.svg

[HelpersMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers
[HelpersAutofacMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers.Autofac
[HelpersDryIocMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers.DryIoc
[HelpersSimpleInjectorMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers.SimpleInjector
[HelpersUnityMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers.Unity
[HelpersAADMyGet]: https://www.myget.org/feed/azurehelpers/package/nuget/AzureMobileClient.Helpers.AzureActiveDirectory

[HelpersMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.svg
[HelpersAutofacMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.Autofac.svg
[HelpersDryIocMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.DryIoc.svg
[HelpersSimpleInjectorMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.SimpleInjector.svg
[HelpersUnityMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.Unity.svg
[HelpersAADMyGetShield]: https://img.shields.io/myget/azurehelpers/vpre/AzureMobileClient.Helpers.AzureActiveDirectory.svg