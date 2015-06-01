using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class Setting
    {
        [Required]
        [RegularExpression(@"[0-9]+")]
        [Display(Name = "Следующий номер в очереди")]
        public int NextNumberQueue { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*")]
        [Display(Name = "Сколько раз приглашать клиента ")]
        public int NumberCall { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*")]
        [Display(Name = "Промежуток времени (в секундах), через который делать приглашение")]
        public int TimeCall { get; set; }
    }
}