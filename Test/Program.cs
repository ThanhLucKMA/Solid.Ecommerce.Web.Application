
using Solid.Ecommerce.Application.Interfaces.Common;
using Solid.Ecommerce.Application.Interfaces.Repositories;
using Solid.Ecommerce.Infrastructure.Context;

using Solid.Ecommerce.Infrastructure.Repositories;

using Solid.Ecommerce.Shared;

namespace Solid.Ecommerce.Test;

public class Program
{
    public static void Main(string[] args)
    {

        //1. Lấy ra 1 mảng các Product 

        DbFactoryContext dbContext = new DbFactoryContext(() => new SolidEcommerceDbContext());

        //2. init ApplicationDbContext
        IApplicationDBContext db = new ApplicationDbContext(dbContext);

        //3. Khoi tạo Repository
        IRepository<Product> productRepository = new Repository<Product>(db);

        //4. 
        var data = productRepository.Find(1);
        Console.WriteLine(data);

        Console.ReadLine();

    }
}