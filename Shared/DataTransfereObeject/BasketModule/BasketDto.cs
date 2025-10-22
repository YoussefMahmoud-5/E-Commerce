﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransfereObeject.BasketModule
{
    public class BasketDto
    {
        public string Id { get; set; } = null!;
        public ICollection<BasketItemDto> Items { get; set; } = [];
    }
}
