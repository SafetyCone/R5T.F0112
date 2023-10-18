using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using R5T.T0132;
using R5T.T0159;


namespace R5T.F0112.Internal
{
    [FunctionalityMarker]
    public partial interface IOperations : IFunctionalityMarker
    {
        public bool ShouldBuildProject(
            string projectFilePath,
            bool rebuildFailedBuildsToCollectErrors,
            ITextOutput textOutput)
        {
            textOutput.WriteInformation("Determining whether the project should be built:\n\t{projectFilePath}", projectFilePath);

            var neverBuiltBefore = this.ShouldBuildProject_NeverBuiltBefore(
                projectFilePath,
                textOutput);

            if (neverBuiltBefore)
            {
                textOutput.WriteInformation("Build project: never built (as part of this process).");

                return true;
            }

            var anyChangesSinceLastBuild = this.ShouldBuildProject_AnyChangesSinceLastBuild(
                projectFilePath,
                textOutput.Logger);

            if (anyChangesSinceLastBuild)
            {
                textOutput.WriteInformation("Build project: changes found since last build.");

                return true;
            }

            // At this point, we know *an attempt* to build project has been tried before, and that there were no changes since the last attempt.

            // If the output assembly was not found, we should re-build the project.
            var outputAssemblyNotFound = this.ShouldBuildProject_OutputAssemblyNotFound(
                projectFilePath,
                textOutput);

            // But only if we want to wait to rebuild projects for which prior build attempts have failed.
            var rebuildProjectAfterPriorFailedBuilds = outputAssemblyNotFound && rebuildFailedBuildsToCollectErrors;

            if (rebuildProjectAfterPriorFailedBuilds)
            {
                textOutput.WriteInformation("Build project: rebuild project after prior failure.");

                return true;
            }

            textOutput.WriteInformation("Do not build project.");

            return false;
        }

        /// <summary>
        /// If the project has never been built before (as part of this process), it should be built.
        /// </summary>
        public bool ShouldBuildProject_NeverBuiltBefore(
            string projectFilePath,
            ITextOutput textOutput)
        {
            // Determine whether the project has been built before as part of this process by testing for the existence of the output build file specific to this process.
            var hasBuildResultFile = Instances.FileSystemOperator.Has_BuildResultFile(projectFilePath);

            if (hasBuildResultFile)
            {
                textOutput.WriteInformation("Should not build: already built (as part of this process).");
                return false;
            }
            else
            {
                textOutput.WriteInformation("Should build: never built (as part of this process).");
                return true;
            }
        }

        /// <summary>
        /// If a project has not changed since the last time it was built, then it should not be built (re-built).
        /// For the specific application of determining instances within a project, we only need to rebuild a project if files within that project have changed.
        /// Note: but for the general case of determining whether a project should be rebuilt, an examination of all files in the full recursive project references hierarchy should be performed (even including NuGet package reference update rules evaluation).
        /// </summary>
        public bool ShouldBuildProject_AnyChangesSinceLastBuild(
            string projectFilePath,
            ILogger logger)
        {
            // Assume that a project should be built.
            var shouldBuildProject = true;

            // Check latest file write time in project directory against build time in publish directory (R5T.S0041.Build.json).
            var projectDirectoryPath = Instances.ProjectPathsOperator.GetProjectDirectoryPath(projectFilePath);

            var projectFilesLastModifiedTime = F0000.FileSystemOperator.Instance.GetLastModifiedTime_ForDirectory_Local(
                projectDirectoryPath,
                Instances.DirectoryNameOperator.IsNotBinariesOrObjectsDirectory);

            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var buildJsonFilePath = Instances.FilePathOperator.Get_BuildJsonFilePath_FromPublishDirectory(publishDirectoryPath);

            var buildJsonFileExists = Instances.FileSystemOperator.Exists_File(buildJsonFilePath);
            if (buildJsonFileExists)
            {
                var buildResult = Instances.JsonOperator.Deserialize_Synchronous<BuildResult>(
                    buildJsonFilePath);

                var lastBuildTime = buildResult.Timestamp;

                // If the last build time is greater than latest file write time, skip building project.
                var skipRepublishProject = lastBuildTime > projectFilesLastModifiedTime;
                if (skipRepublishProject)
                {
                    logger.LogInformation("Skip building project. (Project files last modified time: {projectFilesLastModifiedTime}, last build time: {lastBuildTime}", projectFilesLastModifiedTime, lastBuildTime);

                    shouldBuildProject = false;
                }
            }

            return shouldBuildProject;
        }

        /// <summary>
        /// If a project has not been built (built during a prior run of this process, then it should be built).
        /// </summary>
        public bool ShouldBuildProject_OutputAssemblyNotFound(
            string projectFilePath,
            ITextOutput textOutput)
        {
            var outputAssemblyExists = Instances.FileSystemOperator.Has_OutputAssembly(projectFilePath);
            if (outputAssemblyExists)
            {
                textOutput.WriteInformation("Should not build: output assembly already exists.");
                return false;
            }
            else
            {
                textOutput.WriteInformation("Should build: output assembly does not exist.");
                return true;
            }
        }

        public void WriteProblemProjectsFile(
            string problemProjectsFilePath,
            IDictionary<string, string> problemProjects)
        {
            Instances.FileOperator.WriteAllLines_Synchronous(
                problemProjectsFilePath,
                Instances.EnumerableOperator.From($"Problem Projects, Count: {problemProjects.Count}\n\n")
                    .Append(problemProjects
                        .OrderAlphabetically(pair => pair.Key)
                        .SelectMany(pair => Instances.EnumerableOperator.From($"{pair.Key}:")
                            .Append(pair.Value)
                            .Append("***\n"))));
        }
    }
}
