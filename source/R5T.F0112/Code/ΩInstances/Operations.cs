using System;


namespace R5T.F0112
{
    public class Operations : IOperations
    {
        #region Infrastructure

        public static IOperations Instance { get; } = new Operations();


        private Operations()
        {
        }

        #endregion
    }
}


namespace R5T.F0112.Internal
{
    public class Operations : IOperations
    {
        #region Infrastructure

        public static IOperations Instance { get; } = new Operations();


        private Operations()
        {
        }

        #endregion
    }
}