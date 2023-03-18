using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace SklepProjektRazorPages.ViewModels
{
    public class ProductViewModel : Produkt
    {
        public ProductViewModel(int productId, bool fetchAssociations = false)
        {
            ID_Produktu = productId;
            using(IDbConnection conn = DbHelper.GetDbConnection())
            {
                Produkt product = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", this);
                Nazwa = product.Nazwa;
                sciezkaZdjecia = product.sciezkaZdjecia;
                Cena_jednostkowa = product.Cena_jednostkowa;
                usuniety = product.usuniety;
            }

        }
        public ProductViewModel(Produkt product, bool fetchAssociations = false)
        {
            ID_Produktu = product.ID_Produktu;
            Nazwa = product.Nazwa;
            sciezkaZdjecia = product.sciezkaZdjecia;
            Cena_jednostkowa = product.Cena_jednostkowa;

            if (fetchAssociations)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    // tag
                    try
                    {
                        var tag_products = conn.Query<Tag_produkt>($"SELECT * FROM Tag_produkt WHERE ID_Produktu = @ID_Produktu", product);
                        foreach (Tag_produkt tag_product in tag_products)
                        {
                            Tagi.Add(conn.QueryFirst<Tag>("SELECT * FROM Tag WHERE ID_Tagu = @ID_Tagu", tag_product));
                        }
                    }
                    catch (InvalidOperationException exc) { }

                    try
                    {
                        // category
                        Kategoria = conn.QueryFirst<Kategoria>("SELECT * FROM Kategoria WHERE ID_Kategorii = @ID_Kategorii", product);
                    }
                    catch (InvalidOperationException exc) { }
                }
            }
        }
        public ProductViewModel(Produkt p, Kategoria category) : this(p)
        {
            Kategoria = category;
        }
        public ProductViewModel(Produkt p, Kategoria c, IEnumerable<Tag> tags) : this(p, c)
        {
            Tagi = tags.ToList();
        }
        //public int? ID_Produktu { get; set; }
        //public string Nazwa { get; set; }
        [Display(Name = "Cena jednostkowa")]
        public new int? Cena_jednostkowa { get; set; }
        //public int? ID_Kategorii { get; set; }
        //public string sciezkaZdjecia { get; set; }
        public Kategoria Kategoria { get; set; }
        public List<Tag> Tagi { get; set; } = new List<Tag>();
    }
}
