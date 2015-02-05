using System;

namespace SharpSenses.RealSense {
    public class LanguageNotSupportedException : Exception {
        public SupportedLanguage Language { get; set; }

        public LanguageNotSupportedException(SupportedLanguage language) : base("This language is not supported. Please, install the desired language using the RealSense SDK installer") {
            Language = language;
        }
    }
}