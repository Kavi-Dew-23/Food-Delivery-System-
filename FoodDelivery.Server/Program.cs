using FirebaseAdmin;
using FoodDelivery.Server.Services;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//register userservice 
builder.Services.AddSingleton<UserService>();

// Initialize firebase admin
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(
        Path.Combine(AppContext.BaseDirectory, "firebase-adminsdk.json")
    )
});

// Allow balzor client to connect to the backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    policy.WithOrigins(
        "https://localhost:7188", 
        "http://localhost:5208")
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

app.MapGet("/", () => "Backend running and firebase connected");

app.UseCors("AllowBlazor");
app.MapControllers();

app.Run();