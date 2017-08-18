namespace WebServerComponent.Filter
{
    public interface IServiceInvokerProvider
    {
        IServiceInvoker ResolveServiceInvoker(string id);
    }
}
