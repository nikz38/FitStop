//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FitStop.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Meal
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public System.DateTime DateTimeFor { get; set; }
        public double Calories { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual User User { get; set; }
    }
}
