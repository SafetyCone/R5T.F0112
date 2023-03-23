using System;

using R5T.T0132;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IDirectoryPathOperator : IFunctionalityMarker
    {
        public string GetPublishDirectoryPath_ForProjectFilePath(string projectFilePath)
        {
            var projectDirectoryPath = Instances.ProjectPathsOperator.GetProjectDirectoryPath(projectFilePath);

            var publishDirectoryPath = Instances.PathOperator.GetDirectoryPath(
                projectDirectoryPath,
                Instances.DirectoryNames.bin,
                Instances.DirectoryNames.Publish);

            return publishDirectoryPath;
        }
    }
}
