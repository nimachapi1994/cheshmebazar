//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CheshmebazarIrMyProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProviderHasManyProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> Ofprice { get; set; }
        public byte[] Logo { get; set; }
        public string Text { get; set; }
        public Nullable<int> ParentCategory { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
