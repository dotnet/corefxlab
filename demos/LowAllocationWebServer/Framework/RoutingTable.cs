using System;
using System.Text.Http;
using System.Text.Utf8;

namespace Microsoft.Net.Http
{
    public class ApiRoutingTable<T>
    {
        const int tablecapacity = 20;
        Utf8String[] Uris = new Utf8String[tablecapacity];
        T[] Apis = new T[tablecapacity];
        HttpMethod[] Verbs = new HttpMethod[tablecapacity];
        int count;

        public T Map(HttpRequestLine requestLine)
        {
            for(int i=0; i<count; i++)
            {
                if (requestLine.RequestUri.Equals(Uris[i]) && requestLine.Method == Verbs[i]) return Apis[i];
            }
            return default(T);
        }

        public void Add(T request, HttpMethod method, Utf8String requestUri)
        {
            if (count == tablecapacity) throw new NotImplementedException("ApiReoutingTable does not resize yet.");
            Uris[count] = requestUri;
            Apis[count] = request;
            Verbs[count] = method;
            count++;
        }

        public void Add(T request, HttpMethod verb, string requestUri)
        {
            Add(request, verb, new Utf8String(requestUri));
        }
    }
}
