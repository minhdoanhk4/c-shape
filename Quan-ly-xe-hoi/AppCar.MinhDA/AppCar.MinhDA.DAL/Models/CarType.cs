using System;
using System.Collections.Generic;

namespace AppCar.MinhDA.DAL.Models;

public partial class CarType
{
    public int TypeId { get; set; }

    public string? TypeModel { get; set; }

    public string? TypeName { get; set; }

    public string? TypeDescription { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
