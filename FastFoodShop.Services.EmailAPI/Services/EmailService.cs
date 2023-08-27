using System.Text;
using FastFoodShop.Services.EmailAPI.Data;
using FastFoodShop.Services.EmailAPI.Models;
using FastFoodShop.Services.EmailAPI.Models.Dto;
using FastFoodShop.Services.EmailAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.EmailAPI.Services;

public class EmailService : IEmailService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public EmailService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task EmailCartAndLog(CartDto cartDto)
    {
        StringBuilder message = new StringBuilder();

        message.AppendLine("<br/>Корзина ");
        message.AppendLine("<br/>Сумма " + cartDto.CartHeader.CartTotal);
        message.Append("<br/>");
        message.Append("<ul>");
        foreach (var item in cartDto.CartDetails)
        {
            message.Append("<li>");
            message.Append(item.Product.Name + " x " + item.Count);
            message.Append("</li>");
        }
        message.Append("</ul>");

        await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
    }

    public async Task RegisterUserEmailAndLog(string email)
    {
        string message = "Новый пользователь зарегестрирован. <br/> Email : " + email;
        await LogAndEmail(message, "Новый пользователь");
    }

    private async Task<bool> LogAndEmail(string message, string email)
    {
        try
        {
            EmailLogger emailLog = new()
            {
                Email = email,
                EmailSent = DateTime.Now,
                Message = message
            };
            await using var _db = new AppDbContext(_dbOptions);
            await _db.EmailLoggers.AddAsync(emailLog);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}