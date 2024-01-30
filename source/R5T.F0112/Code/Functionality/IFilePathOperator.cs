using System;

using R5T.T0132;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IFilePathOperator : IFunctionalityMarker
    {
        public string Get_BuildJsonFilePath(string projectFilePath)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var buildJsonFilePath = this.Get_BuildJsonFilePath_FromPublishDirectory(publishDirectoryPath);
            return buildJsonFilePath;
        }

        public string Get_BuildJsonFilePath_FromPublishDirectory(string publishDirectoryPath)
        {
            var buildJsonFilePath = Instances.PathOperator.Get_FilePath(
                publishDirectoryPath,
                FileNames.Instance.BuildJsonFileName);

            return buildJsonFilePath;
        }

        public string Get_PublishDirectoryOutputAssemblyFilePath(
            string projectFilePath)
        {
            var publishDirectoryPath = Instances.DirectoryPathOperator.GetPublishDirectoryPath_ForProjectFilePath(projectFilePath);

            var projectName = Instances.ProjectPathsOperator.GetProjectName(projectFilePath);

            var outputAssemblyFileName = F0000.FileNameOperator.Instance.Get_FileName(
                projectName,
                Instances.FileExtensions.dll.Value);

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

            var outputAssemblyFileName = F0000.FileNameOperator.Instance.Get_FileName(
                projectName,
                Instances.FileExtensions.dll.Value);

            var assemblyFilePath = Instances.PathOperator.Get_FilePath(
                frameworkDirectoryPath,
                outputAssemblyFileName);

            return assemblyFilePath;
        }
    }
}
