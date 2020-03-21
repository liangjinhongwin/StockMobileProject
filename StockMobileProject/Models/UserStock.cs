//<<<<<<< HEAD
//﻿using StockMobileProject.Data;
//=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//>>>>>>> branch-kimo

namespace StockMobileProject.Models
{
    public class UserStock
    {
//<<<<<<< HEAD
//        public string Email { get; set; }
//        public string Symbol { get; set; }
//        public bool IsWatched { get; set; }
//        public int PurchasedCount { get; set; }
//        public ApplicationUser ApplicationUser { get; set; }
//    }
//}
//=======
        public string Id { get; set; }
        public string Symbol { get; set; }
        public bool IsWatched { get; set; }
        public int PurchasedCount { get; set; }

        // Parent
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
//>>>>>>> branch-kimo
