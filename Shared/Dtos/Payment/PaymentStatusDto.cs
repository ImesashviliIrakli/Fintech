using Shared.Enums;

namespace Shared.Dtos.Payment;

public class PaymentStatusDto
{
    public int OrderId { get; set; }
    public int CompanyId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}
