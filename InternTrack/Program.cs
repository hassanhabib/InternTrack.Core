// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace InternTrack.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "InternTrackCoreApi",
                OnEvents = new Events
                {
                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.Windows2022,

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Checking Out Code"
                            },
                            new SetupDotNetTaskV1
                            {
                                Name = "Installing .NET",
                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.203",
                                    IncludePrerelease = true,
                                }
                            },
                            new RestoreTask
                            {
                                Name = "Restoring Nuget Packages"
                            },
                            new DotNetBuildTask
                            {
                                Name = "Building Project"
                            },
                            new TestTask
                            {
                                Name = "Running Tests"
                            }
                        },
                    }
                }
            };

            var client = new ADotNetClient();

            client.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: "../.internTrackCoreApi/dotnet.yml"
            );
        }
    }
}
