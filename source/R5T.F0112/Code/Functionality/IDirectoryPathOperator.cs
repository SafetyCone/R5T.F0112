using System;

using R5T.T0132;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IDirectoryPathOperator : IFunctionalityMarker
    {
        /// <summary>
        /// Gets the project directory path for the project file path (which is the parent directory containing the project file),
        /// then appends /<inheritdoc cref="Z0012.IDirectoryNames.bin" path="descendant::value"/>/<inheritdoc cref="Z0012.IDirectoryNames.Publish" path="descendant::value"/>.
        /// <para>{project directory path}/<inheritdoc cref="Z0012.IDirectoryNames.bin" path="descendant::value"/>/<inheritdoc cref="Z0012.IDirectoryNames.Publish" path="descendant::value"/></para>
        /// </summary>
        public string GetPublishDirectoryPath_ForProjectFilePath(string projectFilePath)
        {
            var projectDirectoryPath = Instances.ProjectPathsOperator.GetProjectDirectoryPath(projectFilePath);

            var publishDirectoryPath = Instances.PathOperator.Get_DirectoryPath(
                projectDirectoryPath,
                Instances.DirectoryNames.bin,
                Instances.DirectoryNames.Publish);

            return publishDirectoryPath;
        }
    }
}
