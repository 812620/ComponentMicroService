﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComponentProcessingMicroservice.Models;

namespace ComponentProcessingMicroservice.Processing
{
    public interface IProcessing
    {
         int ProcessingCharge(ProcessRequest ob1,ProcessResponse ob2);

    }
}
