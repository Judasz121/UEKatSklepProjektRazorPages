using BasiaProjektRazorPages.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace BasiaProjektRazorPages.ViewModels
{
    public class DeliveryViewModel
    {
        //Produkt_magazyn, Dostawca, Magazyn
        public int ID_Produkt_magazyn { get; set; }
        
        public int ID_Produktu { get; set; }
        
        //Magazyn
        public int ID_Magazynu { get; set; }
        public string Nazwa { get; set; }
        public List<Produkt_magazyn> WarehouseProductsList { get; set; } = new List<Produkt_magazyn>();
        public List<Dostawca> SuppliersList { get; set; } = new List<Dostawca>();

        public List<Magazyn> WarehousesList { get; set; } = new List<Magazyn>();
        public IActionResult Index()
        {
            return Page();
        }

        private IActionResult Page()
        {
            throw new NotImplementedException();
        }
    }
}
