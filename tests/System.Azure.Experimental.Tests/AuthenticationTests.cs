// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Azure.Authentication;
using System.Buffers.Cryptography;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Utf8;
using Xunit;

namespace System.Azure.Tests
{
    public class AzureSdk
    {
        DateTime utc = DateTime.UtcNow;
        string fakeKey = "TjW7xr4kKR67qgt2y3fAAMxvC2neMHT6cKawiliGCsDkxSS34V0EnwL8GKrA6ZTIfNrXK91t1Ey3RmEKQLrrCA==";
        string keyType = "master";
        string resourceType = "dbs";
        string version = "1.0";
        string resourceId = "";
        string canonicalizedResource = "/myaccount /mycontainer\ncomp:metadata\nrestype:container\ntimeout:20";

        [Fact]
        public void CosmosDbAuthenticationHeaderTryWrite()
        {
            var keyBytes = Key.ComputeKeyBytes(fakeKey);
            var sha = Sha256.Create(keyBytes);

            // Generate using non-allocating APIs
            var buffer = new byte[256];
            Assert.True(CosmosDbAuthorizationHeader.TryWrite(buffer, sha, keyType, "GET", resourceId, resourceType, version, utc, out int bytesWritten));
            var signatureAsString = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

            // Generate using existing .NET APIs (sample from Asure documentation)
            var expected = CosmosDbBaselineFromMsdn(fakeKey, keyType, "GET", resourceId, resourceType, version, utc);

            Assert.Equal(expected, signatureAsString);
        }

        [Fact]
        public void CosmosDbAuthenticationHeaderTryWriteUtf8()
        {
            var keyBytes = Key.ComputeKeyBytes(fakeKey);
            var sha = Sha256.Create(keyBytes);

            // Generate using non-allocating APIs
            var buffer = new byte[256];
            Assert.True(CosmosDbAuthorizationHeader.TryWrite(buffer, sha, (Utf8Span)keyType, (Utf8Span)"GET", (Utf8Span)resourceId, (Utf8Span)resourceType, (Utf8Span)version, utc, out int bytesWritten));
            var signatureAsString = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

            // Generate using existing .NET APIs (sample from Asure documentation)
            var expected = CosmosDbBaselineFromMsdn(fakeKey, keyType, "GET", resourceId, resourceType, version, utc);

            Assert.Equal(expected, signatureAsString);
        }

        [Fact]
        public void CosmosDbAuthenticationHeaderTryFormat()
        { 
            var header = new CosmosDbAuthorizationHeader();
            header.Hash = Sha256.Create(Key.ComputeKeyBytes(fakeKey));
            header.KeyType = keyType;
            header.Method = "GET";
            header.ResourceId = resourceId;
            header.ResourceType = resourceType;
            header.Version = version;
            header.Time = utc;

            // Generate using non-allocating APIs
            var buffer = new byte[256];
            Assert.True(header.TryFormat(buffer, out int bytesWritten));

            var signatureAsString = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

            // Generate using existing .NET APIs (sample from Asure documentation)
            var expected = CosmosDbBaselineFromMsdn(fakeKey, keyType, "GET", resourceId, resourceType, version, utc);

            Assert.Equal(expected, signatureAsString);
        }

        [Fact]
        public void StorageSignature()
        {
            var keyBytes = Key.ComputeKeyBytes(fakeKey);
            var sha = Sha256.Create(keyBytes);

            // Generate using non-allocating APIs
            var buffer = new byte[256];
            Assert.True(StorageAccessSignature.TryWrite(buffer, sha, "GET", canonicalizedResource, utc, out int bytesWritten));
            var signatureAsString = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

            // Generate using existing .NET APIs (sample from Asure documentation)
            var expected = StorageBaselineFromMsdn(fakeKey, "GET", canonicalizedResource, utc);

            Assert.Equal(expected, signatureAsString);
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

        static string StorageBaselineFromMsdn(string key, string verb, string canonicalizedResource, DateTime utc)
        {
            string canonicalizedString = verb + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "x-ms-date:" + utc.ToString("r") + "\n"
               + canonicalizedResource;

            using (HMACSHA256 hmacSha256 = new HMACSHA256(Convert.FromBase64String(key)))
            {
                Byte[] dataToHmac = Encoding.UTF8.GetBytes(canonicalizedString);
                return Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }
        }
    }
}
