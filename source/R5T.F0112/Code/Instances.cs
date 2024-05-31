using System;


namespace R5T.F0112
{
    public static class Instances
    {
        public static Z0012.IDirectoryNameOperator DirectoryNameOperator => Z0012.DirectoryNameOperator.Instance;
        public static IDirectoryNames DirectoryNames => F0112.DirectoryNames.Instance;
        public static IDirectoryPathOperator DirectoryPathOperator => F0112.DirectoryPathOperator.Instance;
        public static F0000.IEnumerableOperator EnumerableOperator => F0000.EnumerableOperator.Instance;
        public static Z0072.Z000.IFileExtensions FileExtensions => Z0072.Z000.FileExtensions.Instance;
        public static L0066.IFileNameOperator FileNameOperator => L0066.FileNameOperator.Instance;
        public static IFileNames FileNames => F0112.FileNames.Instance;
        public static IFileSystemOperator FileSystemOperator => F0112.FileSystemOperator.Instance;
        public static F0000.IFileOperator FileOperator => F0000.FileOperator.Instance;
        public static IFilePathOperator FilePathOperator => F0112.FilePathOperator.Instance;
        public static F0032.IJsonOperator JsonOperator => F0032.JsonOperator.Instance;
        public static F0000.INowOperator NowOperator => F0000.NowOperator.Instance;
        public static F0002.IPathOperator PathOperator => F0002.PathOperator.Instance;
        public static F0052.IProjectPathsOperator ProjectPathsOperator => F0052.ProjectPathsOperator.Instance;
        public static F0077.IPublishOperator PublishOperator => F0077.PublishOperator.Instance;
        public static F0000.ITextOperator TextOperator => F0000.TextOperator.Instance;
    }
}