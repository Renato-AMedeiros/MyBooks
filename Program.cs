
using Microsoft.Azure.Cosmos;
using my_library_cosmos_db.Middlewares;
using my_library_cosmos_db.Services;
using Newtonsoft.Json;

namespace my_library_cosmos_db
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar o CosmosClient com a ConnectionString
            builder.Services.AddSingleton((s) =>
            {
                var connectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                var cosmosClientOptions = new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Gateway // ou Direct
                };
                return new CosmosClient(connectionString, cosmosClientOptions);
            });

            // Add services to the container.
            builder.Services.AddControllers()
                        .AddNewtonsoftJson(options =>
                        {
                            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<LibrariesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware(typeof(ErrorMiddleware));
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
