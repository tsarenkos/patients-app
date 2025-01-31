namespace PatientsProject.Application.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ApplicationValidationException(string propertyName, string message)
            : base("One or more validation errors occured.")
        {
            this.Errors = new Dictionary<string, string[]>()
            {
                { propertyName, new string[] { message } }
            };
        }
    }
}
