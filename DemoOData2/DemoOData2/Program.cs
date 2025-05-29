using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using DemoOData2.Models;
using DemoOData2; // ??i theo namespace c?a b?n

var builder = WebApplication.CreateBuilder(args);

// ??ng ký DbContext (dùng in-memory nh? slide ho?c dùng SQL)
builder.Services.AddDbContext<BookStoreContext>(opt =>
    opt.UseInMemoryDatabase("BookLists"));

// ??ng ký OData services + c?u hình EdmModel
builder.Services.AddControllers()
    .AddOData(opt =>
        opt.Select()
            .Filter()
            .Count()
            .OrderBy()
            .Expand()
            .SetMaxTop(100)
            .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// T?o EDM Model cho OData
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Book>("Books");
    builder.EntitySet<Press>("Presses");
    return builder.GetEdmModel();
}
