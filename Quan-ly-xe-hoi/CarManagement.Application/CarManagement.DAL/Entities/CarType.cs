using System;
using System.Collections.Generic;

namespace CarManagement.DAL.Entities;

public partial class CarType
{
    public int TypeId { get; set; }

    public string? typeModel { get; set; }

    public string? TypeName { get; set; }

    public string? TypeDescription { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
