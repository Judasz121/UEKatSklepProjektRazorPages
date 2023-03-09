using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;
using System.Text.Json;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;

namespace SklepProjektRazorPages.Pages.Cart
{
    [IgnoreAntiforgeryToken]
    public class apiModel : PageModel
    {

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public string productId { get; set; }
        [BindProperty]
        public string productsAmount { get; set; }
        [BindProperty]
        public string cartId { get; set; }
        [BindProperty]
        public string orderId {get; set; }  

        public IActionResult OnPostAddProductToCart()
        {
            dynamic resp = new ExpandoObject();
            resp.success = false;
            resp.errros = new List<Error>();
            if (AccountHelper.loggedInVerified == false)
            {
                resp.errros.Add(new Error
                {
                    title = "You are not logged in",
                    message = "in order to add to cart you need to be logged in"
                });
                return new JsonResult(resp);
            }
            Zamowienie order = this.EnsureEmptyOrderExists();
            productsAmount = string.IsNullOrWhiteSpace(productsAmount) ? "1" : productsAmount;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                // check if already in cart
                List<Koszyk> dbCart = conn.Query<Koszyk>("SELECT * FROM Koszyk WHERE ID_Produktu = @ID_Produktu AND ID_Zamowienia = @ID_Zamowienia "
                    , new {ID_Produktu = productId, ID_Zamowienia = order.ID_Zamowienia}).ToList();
                if (dbCart.Count > 0)
                {
                    conn.Execute("UPDATE Koszyk SET Ilosc_produktow = Ilosc_produktow + @ilosc WHERE ID_Zamowienia = @ID_Zamowienia AND ID_Produktu = @ID_Produktu",
                        new { ilosc = productsAmount, ID_Zamowienia = order.ID_Zamowienia, ID_Produktu = this.productId });
                }
                else
                {
                    conn.Execute("INSERT INTO Koszyk (ID_Produktu, ID_Zamowienia, Ilosc_Produktow) VALUES(@ID_Produktu, @ID_Zamowienia, @ilosc)",
                        new { ID_Produktu = productId, ID_Zamowienia = order.ID_Zamowienia, @ilosc = productsAmount });
                }
            }
            resp.success = true;
            return new JsonResult(resp);
        }


        public IActionResult OnPostGetCart()
        {
            dynamic resp = new ExpandoObject();
            resp.success = false;
            resp.errors = new List<Error>();
            resp.products = new List<CartRecord>();
            if (AccountHelper.loggedInVerified == false)
            {
                resp.errors.Add(new Error
                {
                    title = "You are not logged in",
                    message = "in order to add to cart you need to be logged in"
                });
                return new JsonResult(resp);
            }
            Zamowienie order = this.EnsureEmptyOrderExists();
            using(IDbConnection conn = DbHelper.GetDbConnection())
            {
                var cart = conn.Query<Koszyk>("SELECT * FROM Koszyk WHERE ID_Zamowienia = @ID_Zamowienia", order);
                foreach(Koszyk c in cart)
                {
                    Produkt p = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", c);
                    resp.products.Add(new CartRecord() { amount = (int)c.Ilosc_produktow, product = p });
                }
            }
            resp.success = true;
            return new JsonResult(resp);
        }
        public IActionResult OnPostUpdateCart()
        {
            
            dynamic resp = new ExpandoObject();
            resp.success = false;
            resp.errors = new List<Error>();
            resp.products = new List<CartRecord>();
            if (AccountHelper.loggedInVerified == false)
            {
                resp.errors.Add(new Error
                {
                    title = "You are not logged in",
                    message = "in order to add to cart you need to be logged in"
                });
                return new JsonResult(resp);
            }
            Zamowienie order = this.EnsureEmptyOrderExists();
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                int result = 0;
                var oldcart = conn.Query<Koszyk>("SELECT * FROM Koszyk WHERE ID_Zamowienia = @ID_Zamowienia", order);
                foreach (Koszyk c in oldcart)
                { 
                    Produkt p = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", c);
                    if(p.ID_Produktu == int.Parse(productId))
                    {
                        conn.Execute($"UPDATE Koszyk  SET Ilosc_produktow = {productsAmount} WHERE ID_Koszyka = {cartId} AND ID_Produktu = {productId}");
                        
                    }
                    resp.products.Add(new CartRecord() { amount = (int)c.Ilosc_produktow, product = p});
                }
                var newcart = conn.Query<Koszyk>("SELECT * FROM Koszyk WHERE ID_Zamowienia = @ID_Zamowienia", order);
                foreach (Koszyk c in newcart)
                {
                    Produkt p = conn.QueryFirst<Produkt>("SELECT * FROM Produkt WHERE ID_Produktu = @ID_Produktu", c);
                    result += (int)p.Cena_jednostkowa * (int)c.Ilosc_produktow;
                    resp.totalprice = result;
                }
            }
            resp.success = true;
            return new JsonResult(resp);
        }

        public void OnPostDeleteProduct()
        {
            Koszyk koszyk = null;
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                koszyk = conn.ExecuteScalar<Koszyk>($"DELETE FROM Koszyk WHERE ID_Zamowienia = {orderId} AND ID_Produktu = {productId}");
            }
        }

        public IActionResult OnPost()
        {            

            return Page();
        }


        public Zamowienie EnsureEmptyOrderExists(string clientId = null)
        {
            if (clientId == null) {
                clientId = AccountHelper.loggedInAccount.ID_Klienta.ToString();
            }
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                List<Zamowienie> dbZam = conn.Query<Zamowienie>("SELECT * FROM Zamowienie WHERE ID_Klienta = @ID_Klienta AND Zlozone = 0 ", new { ID_klienta = clientId }).ToList();
                if (dbZam.Count == 0)
                {
                    int orderId = conn.ExecuteScalar<int>("INSERT INTO Zamowienie (ID_Klienta) VALUES (@clientId); SELECT SCOPE_IDENTITY();", new { clientId = clientId });
                    return conn.QueryFirst<Zamowienie>("SELECT * FROM Zamowienie WHERE ID_Zamowienia = @id", new { id = orderId });
                }
                else
                    return dbZam.FirstOrDefault();
            }

        }

        public struct CartRecord
        {
            public Produkt product { get; set; }
            public int amount { get; set; }
        }
        public struct Error
        {
            public string title { get; set; }
            public string message { get; set; }
        }

    }
}
