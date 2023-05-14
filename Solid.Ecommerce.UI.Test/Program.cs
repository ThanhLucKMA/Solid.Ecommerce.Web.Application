
using Solid.Ecommerce.Application;
using Solid.Ecommerce.Application.Interfaces.Common;
using Solid.Ecommerce.Application.Interfaces.Repositories;
using Solid.Ecommerce.Infrastructure.Context;
using Solid.Ecommerce.Infrastructure.Repositories;
using Solid.Ecommerce.Shared;


namespace Solid.Ecommerce.UI.Test;
public class Program
{
    public  static void Main(string[] args)
    {

        DbFactoryContext dbContext = new DbFactoryContext(() => new SolidEcommerceDbContext());

        IApplicationDBContext db = new ApplicationDbContext(dbContext);

        IRepository<Product> r = new Repository<Product>(db);
        var data =  r.Find(1);

        Console.WriteLine($"Product ID: {data.ProductId}, Product Name: {data.Name}");
        


        Console.ReadLine();
    }
}