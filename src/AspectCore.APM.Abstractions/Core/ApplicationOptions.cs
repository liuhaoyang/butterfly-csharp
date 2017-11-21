namespace AspectCore.APM.Core
{
    public class ApplicationOptions : IOptionAccessor<ApplicationOptions>
    {
        public string ApplicationName { get; set; }

        public string Environment { get; set; }

        public string Host { get; set; }

        public string Urls { get; set; }

        public ApplicationOptions Value => this;
    }
}