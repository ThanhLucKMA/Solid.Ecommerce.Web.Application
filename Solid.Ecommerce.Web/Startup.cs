﻿using AutoMapper;
using Solid.Ecommerce.Application.Mappings;
using Solid.Ecommerce.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solid.Ecommerce.Services.Services;

namespace Solid.Ecommerce.Web;

public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly IHostingEnvironment _currentEnvironment;


    public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
    {
        Configuration = configuration;
        _currentEnvironment = hostingEnvironment;


    }
    public void ConfigureServices(IServiceCollection services)
    {
        
        if (_currentEnvironment.IsDevelopment())
        {
            services.AddWebOptimizer(options =>
            {
                
                options.MinifyCssFiles("/Solid.Ecommerce.Web.styles.css");
                options.MinifyCssFiles("/css/site.css");
                options.MinifyCssFiles("/css/style.css");
                options.MinifyJsFiles("/js/site.js");
                
                //options.AddCssBundle("/css/bundle.css", "css/**/*.css");
            });
        }
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddHttpContextAccessor();
        services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IRazorRenderService, RazorRenderService>();

        services.AddResponseCompression(options => {
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "text/javascript" }
            );
        });


        services.AddRazorPages();

        //Set Session cookie flag
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);//Thoi gian gioi han session 
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        //Set HSTS 
        services.AddHsts(options =>
        {
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        //Set Anti CSRF
        services.AddAntiforgery(options =>
        {
            options.FormFieldName = "_AntiCSRFToken";//Chống giả mạo (1)
            options.HeaderName = "X-Anti-Xsrf-Token";// (2)
        });

        /*call service manual*/
        services.EcommerceInfrastructureDatabase(Configuration);//dependency injection
        services.AddDataServices();
        services.AddAutoMapperService();
        
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseHsts();
            
            
        }
        app.UseRouting(); // start endpoint routing
        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseResponseCompression();

        app.UseWebOptimizer();
        app.UseStaticFiles();

        app.UseHttpsRedirection();//Tất cả các Http request sẽ được gửi bằng HTTPS

        app.UseSession();
        app.UseAuthentication();
<<<<<<< HEAD
        app.UseAuthorization();
=======
>>>>>>> 6d568daec5f91849aaf5361644df63871ad916f8
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapGet("/hello", () => "Hello World!");
        });
    }
}
