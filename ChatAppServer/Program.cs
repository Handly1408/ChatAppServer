using ChatAppServer.Hubs;
using ChatAppServer.Model;
using DAL.Context;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static Google.Protobuf.Reflection.GeneratedCodeInfo.Types;
using static System.Net.Mime.MediaTypeNames;

public class Program

{
    public static WebApplication App { get; private set; }
     static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR(options =>
        {
            options.KeepAliveInterval = TimeSpan.FromSeconds(10); // Клиент отправляет пинг раз в 10 сек
            options.HandshakeTimeout = TimeSpan.FromSeconds(5);   // Если клиент не отвечает 5 сек — разрыв
            options.EnableDetailedErrors = true;
  
        });

        ConfigureServices(builder.Services);

         builder.Services.AddDbContext<ChatDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        App = builder.Build();
        // Configure the HTTP request pipeline.
        if (!App.Environment.IsDevelopment())
        {
            App.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
           // App.UseHttpsRedirection();

            App.UseHsts();
        }

        //sugest using top level round registration instead of useendpoints
        App.UseStaticFiles();

        App.UseRouting();



        App.UseAuthentication();
        App.UseAuthorization();
        App.MapRazorPages();

        App.MapHub<MessangerHub>("/messenger");
        App.MapHub<ContactOnlineStatusHub>("/contactsonlinestatus");
        App.MapHub<WebRTCSignalHub>("/webrtc");
        App.MapHub<DbHub>("/dbhub", options =>{
            
            options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
              

        });


        App.Run();
    }
    public static void ConfigureServices(IServiceCollection services)
    {


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions,FirebaseTokenValidator>(JwtBearerDefaults.AuthenticationScheme
        ,null);
        

    }

}
 
 


