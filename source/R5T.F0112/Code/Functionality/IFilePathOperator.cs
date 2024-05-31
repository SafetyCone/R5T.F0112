using System;

using R5T.T0132;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IFilePathOperator : IFunctionalityMarker
    {
        /// <summary>
        /// Gets the publish directory path for the project file path
        /// (which is complicated),
        /// then calls <see cref="Get_BuildJsonFilePath_FromPublishDirectory(string)"/>, which:
        /// <para><inheritdoc cref="Get_BuildJsonFilePath_FromPublishDirectory(string)" path="/summary"/></para>
        /// </summary>
        public string Get_BuildJsonFilePath(string projectFilePath)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var buildJsonFilePath = this.Get_BuildJsonFilePath_FromPublishDirectory(publishDirectoryPath);
            return buildJsonFilePath;
        }

        /// <summary>
        /// Appends the <see cref="IFileNames.BuildJsonFileName"/> (<inheritdoc cref="IFileNames.BuildJsonFileName" path="descendant::value"/>) to the publish directory path.
        /// </summary>
        public string Get_BuildJsonFilePath_FromPublishDirectory(string publishDirectoryPath)
        {
            var buildJsonFilePath = Instances.PathOperator.Get_FilePath(
                publishDirectoryPath,
                Instances.FileNames.BuildJsonFileName);

            return buildJsonFilePath;
        }

        public string Get_PublishDirectoryOutputAssemblyFilePath(
            string projectFilePath)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var projectName = Instances.ProjectPathsOperator.GetProjectName(projectFilePath);

            var outputAssemblyFileName = Instances.FileNameOperator.Get_FileName(
                projectName,
                Instances.FileExtensions.dll);

            var assemblyFilePath = Instances.PathOperator.Get_FilePath(
                publishDirectoryPath,
                outputAssemblyFileName);

            return assemblyFilePath;
        }

        public string Get_PublishWwwRootFrameworkDirectoryOutputAssemblyFilePath(
            string projectFilePath)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var wwwRootDirectoryPath = Instances.PathOperator.Get_DirectoryPath(
                publishDirectoryPath,
                Instances.DirectoryNames.WwwRoot);

            var frameworkDirectoryPath = Instances.PathOperator.Get_DirectoryPath(
                wwwRootDirectoryPath,
                Instances.DirectoryNames.Framework);

            var projectName = Instances.ProjectPathsOperator.GetProjectName(projectFilePath);

            var outputAssemblyFileName = Instances.FileNameOperator.Get_FileName(
                projectName,
                Instances.FileExtensions.dll);

            var assemblyFilePath = Instances.PathOperator.Get_FilePath(
                frameworkDirectoryPath,
                outputAssemblyFileName);

            return assemblyFilePath;
        }
    }
}
