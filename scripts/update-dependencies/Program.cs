// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.VersionTools;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Dotnet.Scripts
{
    public static class Program
    {
        private static readonly Config config = Config.Instance;

        public static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            if (!ParseArgs(args, out bool updateOnly))
                return;

            IEnumerable<IDependencyUpdater> updaters = GetUpdaters();
            IEnumerable<DependencyBuildInfo> buildInfoDependencies = GetBuildInfoDependencies();

            DependencyUpdateResults updateResults = DependencyUpdateUtils.Update(updaters, buildInfoDependencies);

            if (!updateOnly && updateResults.ChangesDetected())
            {
                CreatePullRequest(updateResults);
            }
        }

        private static bool ParseArgs(string[] args, out bool updateOnly)
        {
            updateOnly = false;

            foreach (string arg in args)
            {
                if (string.Equals(arg, "--update", StringComparison.OrdinalIgnoreCase))
                {
                    updateOnly = true;
                }
                else
                {
                    int idx = arg.IndexOf('=');
                    if (idx < 0)
                    {
                        Console.Error.WriteLine($"Unrecognized argument '{arg}'");
                        return false;
                    }

                    string name = arg.Substring(0, idx);
                    string value = arg.Substring(idx + 1);
                    Environment.SetEnvironmentVariable(name, value);
                }
            }

            return true;
        }

        private static void CreatePullRequest(DependencyUpdateResults updateResults)
        {
            GitHubAuth gitHubAuth = new GitHubAuth(config.Password, config.UserName, config.Email);
            GitHubProject origin = new GitHubProject(config.GitHubProject, config.UserName);
            GitHubBranch upstreamBranch = new GitHubBranch(config.GitHubUpstreamBranch, new GitHubProject(config.GitHubProject, config.GitHubUpstreamOwner));

            string commitMessage = updateResults.GetSuggestedCommitMessage();
            string body = string.Empty;
            if (config.GitHubPullRequestNotifications.Any())
            {
                body += PullRequestCreator.NotificationString(config.GitHubPullRequestNotifications);
            }

            new PullRequestCreator(gitHubAuth, origin, upstreamBranch)
                .CreateOrUpdateAsync(commitMessage, commitMessage + $" ({upstreamBranch.Name})", body)
                .Wait();
        }

        private static IEnumerable<DependencyBuildInfo> GetBuildInfoDependencies()
        {
            yield return GetBuildInfoDependency("Cli", "cli/master");
            yield return GetBuildInfoDependency("CoreFx", "corefx/master");
            yield return GetBuildInfoDependency("CoreSetup", "core-setup/master");
        }

        private static DependencyBuildInfo GetBuildInfoDependency(string target, string fragment)
        {
            BuildInfo buildInfo = BuildInfo.Get(target, $"{config.VersionsUrl}/{fragment}", false);
            return new DependencyBuildInfo(buildInfo, true, Enumerable.Empty<string>());
        }

        private static IEnumerable<IDependencyUpdater> GetUpdaters()
        {
            yield return CreateRegexPropertyUpdater(config.DependencyFilePath, "SystemCompilerServicesUnsafeVersion", "System.Runtime.CompilerServices.Unsafe");
            yield return CreateRegexPropertyUpdater(config.DependencyFilePath, "SystemMemoryVersion", "System.Memory");
            yield return CreateRegexPropertyUpdater(config.DependencyFilePath, "SystemNumericsVectorsVersion", "System.Numerics.Vectors");
            yield return CreateFileUpdater(config.CLIVersionFilePath, "Microsoft.DotNet.Cli.Utils");
        }

        private static IDependencyUpdater CreateFileUpdater(string path, string packageId)
        {
            return new DotNet.VersionTools.Dependencies.FilePackageUpdater()
            {
                Path = path,
                PackageId = packageId
            };
        }

        private static IDependencyUpdater CreateRegexPropertyUpdater(string path, string propertyName, string packageId)
        {
            return new FileRegexPackageUpdater()
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), path),
                PackageId = packageId,
                Regex = new Regex($@"<{propertyName}>(?<version>.*)</{propertyName}>"),
                VersionGroupName = "version"
            };
        }
    }
}
