using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TodoApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ToDoDB"),
        ServerVersion.Parse("8.0.46-mysql")));
        builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
var app = builder.Build();

app.UseCors("AllowAll");
app.MapGet("/items", async (ToDoDbContext db) => await db.Todoitems.ToListAsync());
app.MapPost("/items",async(ToDoDbContext db,Todoitem item)=>{db.Todoitems.Add(item); await db.SaveChangesAsync();return Results.Created($"/items/{item.Id}",item);});
app.MapPut("/items/{id}",async(ToDoDbContext db,int id,Todoitem update) =>
{
    var item=await db.Todoitems.FindAsync(id);
    if(item is null) return Results.NotFound();
    item.Name=update.Name;
    item.IsComplete=update.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(item);
} );
app.MapDelete("/items/{id}",async(ToDoDbContext db,int id) =>
{
    var item=await db.Todoitems.FindAsync(id);
    if(item is null) return Results.NotFound();
    db.Todoitems.Remove(item);
     await db.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();
