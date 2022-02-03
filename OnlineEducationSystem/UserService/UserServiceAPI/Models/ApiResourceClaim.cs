﻿using System;
using System.Collections.Generic;

namespace UserServiceAPI.Models
{
    public partial class ApiResourceClaim
    {
        public int Id { get; set; }
        public int ApiResourceId { get; set; }
        public string Type { get; set; }

        public virtual ApiResource ApiResource { get; set; }
    }
}
