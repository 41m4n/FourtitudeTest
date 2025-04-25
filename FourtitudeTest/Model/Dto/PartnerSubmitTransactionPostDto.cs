﻿using System.ComponentModel.DataAnnotations;

namespace FourtitudeTest.Model.Dto
{
    public class PartnerSubmitTransactionPostDto
    {
        [Required]
        [MaxLength(50)]
        public string partnerkey { get; set; }

        [Required]
        [MaxLength(50)]
        public string partnerRefno { get; set; }

        [Required]
        [MaxLength(50)]
        public string partnerPassword { get; set; }

        [Required]
        public long totalAmount { get; set; }

        public List<PartnerItem> items { get; set; }

        [Required]
        public string timeStamp { get; set; }

        [Required]
        public string sig { get; set; }
    }

    public class PartnerItem
    {
        [Required]
        [MaxLength(50)]
        public string partneritemref { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        public int qty { get; set; }

        [Required]
        public long unitprice { get; set; }
    }
}
