
using DiagramCursVue.Data;
using Microsoft.EntityFrameworkCore;

namespace DiagramCursVue.Server {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("pdf_editor")));

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                    builder => {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();  // ��������������� �� HTTPS

            app.UseStaticFiles();       // ������������ ����������� ������
            app.UseRouting();           // �������� �������������
            app.UseCors("AllowMyOrigin"); // ��������� CORS (�� �������������� � �����������)
            app.UseAuthorization();      // �������������� � �����������

            app.MapControllers();        // ������������ ����������� API
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
