//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterPol.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int idProduct { get; set; }
        public int idType { get; set; }
        public string NameProduct { get; set; }
        public string Article { get; set; }
        public Nullable<decimal> MinCostPartner { get; set; }
    
        public virtual ProductType ProductType { get; set; }
    }
}
