// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Microsoft.Dotnet.Scripts
{
    public class Config
    {
        public static Config Instance { get; } = new Config();

        private Lazy<string> _userName = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_USER"));
        private Lazy<string> _email = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_EMAIL"));
        private Lazy<string> _password = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_PASSWORD"));

        private Lazy<string> _gitHubUpstreamOwner = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_UPSTREAM_OWNER", "dotnet"));
        private Lazy<string> _gitHubProject = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_PROJECT", "corefxlab"));
        private Lazy<string> _gitHubUpstreamBranch = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_UPSTREAM_BRANCH", "master"));
        private Lazy<string[]> _gitHubPullRequestNotifications = new Lazy<string[]>(() =>
                                                GetEnvironmentVariable("GITHUB_PULL_REQUEST_NOTIFICATIONS", "")
                                                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

        private Config()
        {
        }

        public string UserName => _userName.Value;
        public string Email => _email.Value;
        public string Password => _password.Value;
        public string GitHubUpstreamOwner => _gitHubUpstreamOwner.Value;
        public string GitHubProject => _gitHubProject.Value;
        public string GitHubUpstreamBranch => _gitHubUpstreamBranch.Value;
        public string[] GitHubPullRequestNotifications => _gitHubPullRequestNotifications.Value;
        public string VersionsUrl => $"https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet";
        public string DependencyFilePath = Path.Combine("tools", "dependencies.props");
        public string CLIVersionFilePath = "DotnetCLIVersion.txt";

        private static string GetEnvironmentVariable(string name, string defaultValue = null)
        {
            string value = Environment.GetEnvironmentVariable(name);
            if (value == null)
            {
                value = defaultValue;
            }

            if (value == null)
            {
                throw new InvalidOperationException($"Can't find environment variable '{name}'.");
            }

            return value;
        }
    }
}
