﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XuongMay.ModelViews.CategoryModelViews
{
    public class CreateCategoryModelView
    {
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
    }
}
