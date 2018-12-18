namespace RecordUnion.Automation.Web.Framework.Constants
{
    public class ValidationsMessage
    {
        public const string SoftWarningCompoundArtists = "Featuring artists shouldn't be mentioned here";
        public const string HardWarningsTooManyCharacters = "The more the merrier is usually the case, but this is just too much unfortunately";
        public const string HardWarningsMentionedInMusicService = "We love our stores, but they can't be mentioned here unfortunately";

        public const string HardValidationVersionOriginal =
            "Originality is great, but it can't be mentioned here unfortunately";

        public const string MaxStringSize251 = "qwertyuiop qwertyuiop qwertyuiop "
                                               + "qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop "
                                               + "qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop "
                                               + "qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuiop qwertyuio";

        public const string Soft = "Soft";
        public const string Hard = "Hard";
        public const string SameMainAndAlternateGenre = "Sorry, main and alternate can't be the same";
        public const string SameAlternateAndMainGenre = "Sorry, alternate and main can't be the same";
    }
}