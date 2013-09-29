namespace manathan.find.Configuration
{
    #region

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class MissingRequiredConfigurationOptionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MissingRequiredConfigurationOptionException()
        {
        }

        public MissingRequiredConfigurationOptionException(string key)
            : base(string.Format("Required Option {0} not found in configuration for page.", key))
        {
        }

        public MissingRequiredConfigurationOptionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingRequiredConfigurationOptionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}