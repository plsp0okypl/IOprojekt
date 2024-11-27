using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koszyk
{
    internal class Cart
    {
        List<Products> ProductsInCart = new List<Products>;

        void AddToCart()
        {
            Console.WriteLine()

            DisplayProducts();

            ProductsInCart.Add(SelectProduct());
        }
        void RemoveFromCart()
        {
            ProductsInCart.Remove(SelectProduct());
        }
        void DisplayCart()
        {
            foreach (var product in ProductsInCart)
            {
                Console.WriteLine(product.Name + "\t" + product.Price);
            }
        }
        void DisplayTaskList()
        {
            Console.WriteLine("1. Dodaj produkt do koszyka");
            Console.WriteLine("2. Usun produkt z koszyka");
        }
    }
}
