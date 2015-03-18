using System.Text.Utf8;

namespace System.Net.Http.Buffered
{
    public class ApiRoutingTable<T>
    {
        const int tablecapacity = 20;
        Utf8String[] Uris = new Utf8String[tablecapacity];
        T[] Apis = new T[tablecapacity];
        int count;

        public T Map(HttpRequestLine requestLine)
        {
            for(int i=0; i<count; i++)
            {
                if (Uris[i].Equals(requestLine.RequestUri)) return Apis[i];
            }
            return default(T);
        }

        public void Add(T request, HttpMethod method, Utf8String requestUri)
        {
            if (count == tablecapacity) throw new NotImplementedException("ApiReoutingTable does not resize yet.");
            if (method != HttpMethod.Get) throw new NotImplementedException("HttpRoutingTable.Add not implemented for verbs other than Get");
            Uris[count] = requestUri;
            Apis[count] = request;
            count++;
        }

        public void Add(T request, HttpMethod verb, string requestUri)
        {
            Add(request, verb, new Utf8String(requestUri));
        }
    }
}
