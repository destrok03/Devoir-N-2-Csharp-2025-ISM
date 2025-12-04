using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestionapp.ViewModels
{
    public class ApprovisionnementCreateVm
    {
        [Required]
        public string Reference { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public string? Observations { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateApprovisionnement { get; set; }

        [Required]
        public int FournisseurId { get; set; }

        public List<ApprovisionnementLigneCreateVm> Lignes { get; set; } =
            new List<ApprovisionnementLigneCreateVm> { new ApprovisionnementLigneCreateVm() };

        public IEnumerable<SelectListItem>? Fournisseurs { get; set; }
        public IEnumerable<SelectListItem>? Articles { get; set; }
    }
}
