// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Azure.Authentication;
using System.Buffers.Cryptography;
using System.Text;
using Xunit;

namespace tests
{
    public class AzureSdk
    {
        DateTime utc = DateTime.UtcNow;
        string fakeKey = "TjW7xr4kKR67qgt2y3fAAMxvC2neMHT6cKawiliGCsDkxSS34V0EnwL8GKrA6ZTIfNrXK91t1Ey3RmEKQLrrCA==";
        string keyType = "master";
        string resourceType = "dbs";
        string version = "1.0";
        string resourceId = "";

        [Fact]
        public void GenerateSignature()
        {
            var keyBytes = Signature.ComputeKeyBytes(fakeKey);
            var sha = Sha256.Create(keyBytes);

            // Generate using non-allocating APIs
            var buffer = new byte[256];
            var bytesWritten = Signature.Generate(buffer, sha, keyType, "GET", resourceId, resourceType, version, utc);
            var signatureAsString = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

            // Generate using existing .NET APIs (sample from Asure documentation)
            var expected = GenerateMasterKeyAuthorizationSignatureMsdn(fakeKey, keyType, "GET", resourceId, resourceType, version, utc);

            Assert.Equal(expected, signatureAsString);
        }

        static string GenerateMasterKeyAuthorizationSignatureMsdn(string key, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc)
        {
            var keyBytes = Convert.FromBase64String(key);
            var hmacSha256 = new System.Security.Cryptography.HMACSHA256 { Key = keyBytes };
            string utc_date = utc.ToString("r");

            string payLoad = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n",
                    verb.ToLowerInvariant(),
                    resourceType.ToLowerInvariant(),
                    resourceId,
                    utc_date.ToLowerInvariant(),
                    ""
            );

            byte[] hashPayLoad = hmacSha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payLoad));
            string signature = Convert.ToBase64String(hashPayLoad);

            var full = String.Format(System.Globalization.CultureInfo.InvariantCulture, "type={0}&ver={1}&sig={2}",
                keyType,
                tokenVersion,
                signature);

            var result = System.Text.Encodings.Web.UrlEncoder.Default.Encode(full);

            return result;
        }
    }
}
