using System;
using System.Collections.Generic;

namespace CarManagement.DAL.Entities;

public partial class Car
{
    public int CarId { get; set; }

    public string? CarModel { get; set; }

    public string? CarEngine { get; set; }

    public int? TypeId { get; set; }

    public int? CarSeat { get; set; }

    public string? CarDescription { get; set; }

    public int? YearPublish { get; set; }

    public double? DollarPrice { get; set; }

    public int? CarWarranty { get; set; }

    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual CarType? CarType { get; set; }
}
