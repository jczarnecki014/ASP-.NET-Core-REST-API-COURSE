using Bogus;
using RestaurantAPI.Entities;
using System.Collections.Generic;

namespace RestaurantAPI
{
    public class RestaurantGenerator
    {
        public static void Seed(RestaurantDbContext context)
        {
            var locale = "pl";    


            var userGenerator = new Faker<User>(locale)
                .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.FirstName, f => f.Person.FirstName)
                .RuleFor(a => a.LastName, f => f.Person.LastName)
                .RuleFor(a => a.Nationality, f => f.Address.Country())
                .RuleFor(a => a.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(a => a.RoleId, 2);


            var restaurantGenerator = new Faker<Restaurant>(locale)
                .RuleFor(a => a.name, f=> f.Company.CompanyName())
                .RuleFor(a => a.description, f => f.Lorem.Paragraph())
                .RuleFor(a => a.ContactEmail, f=> f.Person.Email)
                .RuleFor(a => a.ContactNumber, f=> f.Person.Phone)
                .RuleFor(a => a.hasDelivery, f => f.Random.Bool())
                .RuleFor(a => a.CreatedBy, f => userGenerator.Generate());


            var addresGenerator  = new Faker<Address>(locale)
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Street, f => f.Address.StreetName())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Restaurant, f => restaurantGenerator.Generate());

           var addreses = addresGenerator.Generate(100);
           context.AddRange(addreses);
           context.SaveChanges();
        }
    }
}
