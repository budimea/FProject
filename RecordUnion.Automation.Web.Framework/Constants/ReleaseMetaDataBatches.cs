using System.Security.Cryptography;

//ToDo replace this with different enums

namespace RecordUnion.Automation.Web.Framework.Constants
{
    public static class ReleaseMetaDataBatches
    {
        public const int ReleaseTitleVersionBatch = 0;
        public const int Title = 0;
        public const int Version = 1;

        public const int MainGenreBatch = 1;
        public const int MainGenre = 0;
        public const int MainSubGenre = 1;

        public const int AlternateGenreBatch = 2;
        public const int AlternateMainGenre = 0;
        public const int AlternateSubGenre = 1;

        public const int PrimaryArtistRole = 0;
        public const int FeaturingArtistRole = 1;
        public const int ProducersRole = 2;
        public const int AdditionalCollaboratorsRole = 3;

        public const int ReleaseCopyrightOwner = 0;
        public const int Label = 1;
        

        public const int CatalogueNumber = 3;
        public const int Ean = 4;
        public const int Language = 5;

        public const string ReleaseCopyRightOwner = "Copyright Owner";
    }
}
