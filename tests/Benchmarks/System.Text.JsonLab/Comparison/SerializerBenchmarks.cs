using System;
using System.Linq;

namespace JsonBenchmarks
{
    internal static class SerializerBenchmarks
    {
        internal static Type[] GetTypes()
            => GetOpenGenericBenchmarks()
                .SelectMany(openGeneric => GetViewModels().Select(viewModel => openGeneric.MakeGenericType(viewModel)))
                .ToArray();

        private static Type[] GetOpenGenericBenchmarks()
            => new[]
            {
                typeof(JsonDeserializerComparison_FromString<>),
                typeof(JsonSerializerComparison_ToString<>),
            };

        private static Type[] GetViewModels()
            => new[]
            {
                typeof(LoginViewModel),
                typeof(Location),
                typeof(IndexViewModel),
                typeof(MyEventsListerViewModel)
            };
    }
}
