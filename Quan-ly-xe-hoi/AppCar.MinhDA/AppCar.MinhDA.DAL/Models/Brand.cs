using System;
using System.Collections.Generic;

namespace AppCar.MinhDA.DAL.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? BrandName { get; set; }

    public int? FoundingYear { get; set; }

    public string? BrandNational { get; set; }

    public string? BrandDescription { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
