using System;

using R5T.T0132;


namespace R5T.F0112
{
    [FunctionalityMarker]
    public partial interface IFileSystemOperator : IFunctionalityMarker,
        F0000.IFileSystemOperator
    {
        public bool Has_BuildResultFile(
            string projectFilePath)
        {
            var buildJsonFilePath = Instances.FilePathOperator.Get_BuildJsonFilePath(projectFilePath);

            var buildJsonFileExists = this.Exists_File(buildJsonFilePath);
            return buildJsonFileExists;
        }

        public bool Has_OutputAssembly(
            string projectFilePath)
        {
            // Test for the default output assembly file path.
            var assemblyFilePath = Instances.FilePathOperator.Get_PublishDirectoryOutputAssemblyFilePath(projectFilePath);

            var outputAssemblyExists = this.Exists_File(assemblyFilePath);
            if (outputAssemblyExists)
            {
                return true;
            }

            // Otherwise, test for the Blazor WebAssembly output file path.
            var blazorWebAssemblyFilePath = Instances.FilePathOperator.Get_PublishWwwRootFrameworkDirectoryOutputAssemblyFilePath(projectFilePath);

            var blazorWebAssemblyFileExists = this.Exists_File(blazorWebAssemblyFilePath);
            return blazorWebAssemblyFileExists;
        }
    }
}
