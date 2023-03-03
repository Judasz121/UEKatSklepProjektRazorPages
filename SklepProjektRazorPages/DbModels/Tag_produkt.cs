namespace SklepProjektRazorPages.DbModels
{
    public class Tag_produkt : BaseDbModel
    {
        public int ID_Produktu { get; set; }

        public int ID_Tagu { get; set; }
    }
}
