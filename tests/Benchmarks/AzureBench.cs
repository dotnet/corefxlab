// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xunit.Performance;
using System;
using System.Azure.Authentication;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Buffers.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public class AzureBench
{
    static DateTime utc = DateTime.UtcNow;
    static string fakeKey = "TjW7xr4kKR67qgt2y3fAAMxvC2neMHT6cKawiliGCsDkxSS34V0EnwL8GKrA6ZTIfNrXK91t1Ey3RmEKQLrrCA==";
    static string keyType = "master";
    static string resourceType = "dbs";
    static string version = "1.0";
    static string resourceId = "";
    static byte[] output = new byte[256];
    static Sha256 sha;

    static AzureBench()
    {
        var keyBytes = Key.ComputeKeyBytes(fakeKey);
        sha = Sha256.Create(keyBytes);
    }

    [Benchmark]
    static void Msdn()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement()) {
                CosmosDbBaselineFromMsdn(fakeKey, keyType, "GET", resourceId, resourceType, version, utc);
            }
        }
    }

    [Benchmark]
    static void Raw()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement()) {
                CosmosDbAuthorizationHeader.TryWrite(output, sha, keyType, "GET", resourceId, resourceType, version, utc, out int bytesWritten);
            }
        }
    }

    [Benchmark]
    static void Writer()
    {
        foreach (var iteration in Benchmark.Iterations)
        {
            using (iteration.StartMeasurement()) {
                CosmosDbAuthorizationHeader.TryWrite2(output, sha, keyType, "GET", resourceId, resourceType, version, utc, out int bytesWritten);
            }
        }
    }

    static string CosmosDbBaselineFromMsdn(string key, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc)
    {
        var keyBytes = Convert.FromBase64String(key);
        var hmacSha256 = new HMACSHA256 { Key = keyBytes };
        string utc_date = utc.ToString("r");

        string payLoad = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n",
                verb.ToLowerInvariant(),
                resourceType.ToLowerInvariant(),
                resourceId,
                utc_date.ToLowerInvariant(),
                ""
        );

        byte[] hashPayLoad = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(payLoad));
        string signature = Convert.ToBase64String(hashPayLoad);

        var full = String.Format(CultureInfo.InvariantCulture, "type={0}&ver={1}&sig={2}",
            keyType,
            tokenVersion,
            signature);

        var result = System.Text.Encodings.Web.UrlEncoder.Default.Encode(full);

        return result;
    }
}

