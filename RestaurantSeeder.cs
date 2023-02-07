using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext db)
        {
            _dbContext = db;
        }
        public void seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Restaurants.Any())
                {
                     List<Restaurant> restaurants = GetRestaurantsList();
                     _dbContext.AddRange(restaurants);
                     _dbContext.SaveChanges();
                }
            }
        }
        private List<Restaurant> GetRestaurantsList()
        {
            List<Restaurant>restaurants= new List<Restaurant>()
            {
                new Restaurant()
                {
                    name = "KFC",
                    description = "Restauracja oferująca najlepsze gorące kurczaki w przystępnej cenie, co prawda nie są one najzdrowsze ale czym jest zdrowie przy dodatkowej darmowej dolewce pepsji ?",
                    Category = "Fast Food",
                    hasDelivery= true,
                    ContactEmail = "zamowienia@kfc.pl",
                    ContactNumber = "611 - 104 - 756",
                    address= new Address()
                    {
                        City = "Jelenia Góra",
                        Street = "Jana Pawla II",
                        PostalCode = "58-500",
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish()
                        {
                            Name="Strips",
                            Description = "Pyszny kurczaczek w gorącej panierce",
                            Price = 40,
                        },
                        new Dish()
                        {
                            Name="Longer",
                            Description = "Pyszna kanapeczka z pysznym tanim kurczakiem",
                            Price = 20,
                        },
                        new Dish()
                        {
                            Name="Quasaidlla",
                            Description = "Prawdziwa meksykańska zapiekanka",
                            Price = 60,
                        },
                    },
                    
                },
                new Restaurant()
                {
                    name = "McDonald",
                    description = "Tani fastfood z średnim rzarciem ale co jakiś czas wrzucimy drwala to ludzie walą drzwiami i oknami XD",
                    Category = "Fast Food",
                    hasDelivery= true,
                    ContactEmail = "takeOff@mcDonald.pl",
                    ContactNumber = "406 - 214 - 111",
                    address= new Address()
                    {
                        City = "Jelenia Góra",
                        Street = "Jana Pawla II",
                        PostalCode = "58-500",
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish()
                        {
                            Name="Nugets",
                            Description = "Małe dobre kurczaki, ponoć dodajemy do nich środki przeciw wymiotne",
                            Price = 30,
                        },
                        new Dish()
                        {
                            Name="mcFrytki",
                            Description = "Zajebiście tłuste frytki",
                            Price = 11,
                        },
                        new Dish()
                        {
                            Name="McDrwal",
                            Description = "Ta kanapka sprawi, że każda dupa będzie twoja",
                            Price = 40,
                        },
                    },
                    
                }
            };
            return restaurants;
        }
    }
}
