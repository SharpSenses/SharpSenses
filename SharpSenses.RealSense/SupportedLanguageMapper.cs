namespace SharpSenses.RealSense {
    public static class SupportedLanguageMapper {
        public static SupportedLanguage FromString(string language) {
            if (language == "LANGUAGE_US_ENGLISH") return SupportedLanguage.EnUS;
            if (language == "LANGUAGE_GB_ENGLISH") return SupportedLanguage.EnGB;
            if (language == "LANGUAGE_DE_GERMAN") return SupportedLanguage.GeDE;
            if (language == "LANGUAGE_US_SPANISH") return SupportedLanguage.EsUS;
            if (language == "LANGUAGE_LA_SPANISH") return SupportedLanguage.EsLA;
            if (language == "LANGUAGE_FR_FRENCH") return SupportedLanguage.FrFR;
            if (language == "LANGUAGE_IT_ITALIAN") return SupportedLanguage.ItIT;
            if (language == "LANGUAGE_JP_JAPANESE") return SupportedLanguage.JaJP;
            if (language == "LANGUAGE_CN_CHINESE") return SupportedLanguage.ZhCN;
            if (language == "LANGUAGE_BR_PORTUGUESE") return SupportedLanguage.PtBR;
            return SupportedLanguage.NotSpecified;
        }

        public static string ToLabel(SupportedLanguage supportedLanguage) {
            switch (supportedLanguage) {
                case SupportedLanguage.EnUS:
                    return "LANGUAGE_US_ENGLISH";
                case SupportedLanguage.PtBR:
                    return "LANGUAGE_BR_PORTUGUESE";
                case SupportedLanguage.EnGB:
                    return "LANGUAGE_GB_ENGLISH";
                case SupportedLanguage.GeDE:
                    return "LANGUAGE_DE_GERMAN";
                case SupportedLanguage.EsUS:
                    return "LANGUAGE_US_SPANISH";
                case SupportedLanguage.EsLA:
                    return "LANGUAGE_LA_SPANISH";
                case SupportedLanguage.FrFR:
                    return "LANGUAGE_FR_FRENCH";
                case SupportedLanguage.ItIT:
                    return "LANGUAGE_IT_ITALIAN";
                case SupportedLanguage.JaJP:
                    return "LANGUAGE_JP_JAPANESE";
                case SupportedLanguage.ZhCN:
                    return "LANGUAGE_CN_CHINESE";
            }
            return "NotSpecified";
        }
    }
}