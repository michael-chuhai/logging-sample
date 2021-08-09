using System;

namespace MySecrets
{
    public class MySecretsOptions
    {
        public const string SectionName = "MySecrets";


        public TimeSpan AcceptableExecutionTime { get; set; }
    }
}