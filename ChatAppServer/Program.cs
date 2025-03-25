using ChatAppServer.Hubs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class Program

{
    public static WebApplication App { get; private set; }
     static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR();
        
        ConfigureServices(builder.Services);
        //builder.WebHost.UseUrls("https://35.208.79.49:443");
        App = builder.Build();
       // App.Urls.Add("http://35.209.201.152:80");


        // Configure the HTTP request pipeline.
        if (!App.Environment.IsDevelopment())
        {
            App.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //App.UseHttpsRedirection();

            App.UseHsts();
        }
        //App.UseCertificateForwarding();

        //sugest using top level round registration instead of useendpoints
        App.UseStaticFiles();

        App.UseRouting();
      



        App.UseAuthentication();
        App.UseAuthorization();
        App.MapRazorPages();

        App.MapHub<MessangerHub>("/messenger");
        App.MapHub<ContactOnlineStatusHub>("/contactsonlinestatus");
        App.MapHub<WebRTCSignalHub>("/webrtc");

     
        App.Run();
        
    }
    public static void ConfigureServices(IServiceCollection services)
    {


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions,FirebaseTokenValidator>(JwtBearerDefaults.AuthenticationScheme
            ,null);
       

    }

}

