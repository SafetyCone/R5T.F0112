using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using R5T.T0132;
using R5T.T0159;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IOperations : IFunctionalityMarker
    {
        private static Internal.IOperations Internal => F0112.Internal.Operations.Instance;


        /// <summary>
        /// Builds the project with the win-x64 runtime argument to get a self-contained executable.
        /// (To allow full assembly reflection.)
        /// </summary>
        public async Task BuildProject(
            string projectFilePath,
            ITextOutput textOutput)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            Instances.FileSystemOperator.ClearDirectory_Synchronous(publishDirectoryPath);

            // Publishing as a self-contained application solves the issue with framework assemblies not being available
            // (for example, ASP.NET Core framework assemblies not being available from .NET Core).
            // But! At the cost of tremendous hard disk space (~100MB for each target).
            // Instances.DotnetPublishOperator.Publish_WithRuntimeArgument(
            //    projectFilePath,
            //    publishDirectoryPath);

            // Instead, use the regular publish operation for HD space.
            // Not sure what to do in the reflection step. I have tried:
            // 1. Adding a framework reference in the CSPROJ file of the executable doing the reflection.
            await Instances.PublishOperator.Publish(
                projectFilePath,
                publishDirectoryPath);

            textOutput.WriteInformation("Built project.");
        }

        public async Task BuildProject(
            string projectFilePath,
            ITextOutput textOutput,
            Dictionary<string, string> buildProblemTextsByProjectFilePath)
        {
            try
            {
                await this.BuildProject(
                    projectFilePath,
                    textOutput);
            }
            catch (AggregateException aggregateException)
            {
                textOutput.WriteWarning_WithPause($"Failed to build project:\n\t{projectFilePath}");

                // Combine aggregate exception messages to text.
                var buildProblemText = Instances.TextOperator.Join_Lines(
                    aggregateException.InnerExceptions
                        .Select(exception => exception.Message));

                buildProblemTextsByProjectFilePath.Add(
                    projectFilePath,
                    buildProblemText);
            }

            // Output an S0041-specific file (R5T.S0041.Build.json) containing build time to publish directory.
            // Output this regardless of build success so that projects are not rebuilt in either case until project files change.
            var buildTime = Instances.NowOperator.Get_Now_Local();

            var buildResult = new BuildResult
            {
                Timestamp = buildTime,
            };

            var buildJsonFilePath = Instances.FilePathOperator.Get_BuildJsonFilePath(projectFilePath);

            Instances.JsonOperator.Serialize_Synchronous(
                buildJsonFilePath,
                buildResult);
        }

        public async Task<Dictionary<string, string>> BuildProjectFilePaths(
            bool rebuildFailedBuildsToCollectErrors,
            IList<string> projectFilePaths,
            string buildProblemsFilePath,
            string buildProblemProjectsFilePath,
            ITextOutput textOutput)
        {
            var buildProblemTextsByProjectFilePath = new Dictionary<string, string>();

            var projectCounter = 1; // Start at 1.
            var projectCount = projectFilePaths.Count;

            foreach (var projectFilePath in projectFilePaths)
            {
                textOutput.WriteInformation("Building project ({projectCounter} / {projectCount}):\n\t{projectFilePath}",
                    projectCounter++,
                    projectCount,
                    projectFilePath);

                var shouldBuildProject = Internal.ShouldBuildProject(
                    projectFilePath,
                    rebuildFailedBuildsToCollectErrors,
                    textOutput);

                if (shouldBuildProject)
                {
                    // Clear the publish directory and publish (build), and not any problems.
                    await this.BuildProject(
                        projectFilePath,
                        textOutput,
                        buildProblemTextsByProjectFilePath);
                }
                else
                {
                    // See if a prior attempt to build the project failed, and not the failure.
                    var hasOutputAssembly = Instances.FileSystemOperator.Has_OutputAssembly(projectFilePath);
                    if (!hasOutputAssembly)
                    {
                        var buildProblemText = "Prior builds failed, and option to rebuild prior failed builds to collect errors was not true.";

                        buildProblemTextsByProjectFilePath.Add(
                            projectFilePath,
                            buildProblemText);
                    }
                }
            }

            // Write build problems file.
            Internal.WriteProblemProjectsFile(
                buildProblemsFilePath,
                buildProblemTextsByProjectFilePath);

            // Write build problem projects file.
            await Instances.FileOperator.Write_Lines(
                buildProblemProjectsFilePath,
                buildProblemTextsByProjectFilePath.Keys
                    .OrderAlphabetically());

            return buildProblemTextsByProjectFilePath;
        }
    }
}
