﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public interface IApp
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
    }

    public interface IFeature
    {
    
    }

    public interface IPage
    { 
    
    }
}