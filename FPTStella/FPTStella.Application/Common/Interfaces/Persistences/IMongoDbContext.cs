﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Persistences
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
    }
}
