using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public static class TagCollectionExtensions
    {
        public static TagCollection HttpHost(this TagCollection tagCollection, string host)
        {
            return tagCollection.Set("http.host", host);
        }
        
        public static TagCollection HttpPath(this TagCollection tagCollection, string path)
        {
            return tagCollection.Set("http.path", path);
        }
    }
}