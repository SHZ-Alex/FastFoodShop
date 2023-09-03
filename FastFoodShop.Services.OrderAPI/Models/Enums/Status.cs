using Microsoft.OpenApi.Attributes;

namespace FastFoodShop.Services.OrderAPI.Models.Enums;

public enum Status
{
  [Display("Создание")]
  Pending,
  [Display("Принят")]
  Approved,
  [Display("Готов для отправки")]
  ReadyForPickup,
  [Display("Завершен")]
  Completed,
  [Display("Возврат")]
  Refunded,
  [Display("Отменен")]
  Cancelled
}