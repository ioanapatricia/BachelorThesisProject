using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class OrderForGetDto
    {

        public string Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public StatusForGetDto Status { get; set; }
        public PaymentTypeForGetDto PaymentType { get; set; }
        public AddressForReturnDto Address { get; set; }
        public UserForGetDto User { get; set; }
        public IEnumerable<ProductForGetDto> Products { get; set; }
    }
}
