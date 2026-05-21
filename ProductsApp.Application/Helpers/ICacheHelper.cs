namespace ProductsApp.Application.Helpers
{
    public interface ICacheHelper
    {
        T Get<T>(string cacheKey);
        void Set<T>(string cacheKey, T data);
        void Clear(string cacheKey);
    }
}
