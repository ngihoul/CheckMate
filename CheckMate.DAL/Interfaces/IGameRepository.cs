﻿using CheckMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Interfaces
{
    public interface IGameRepository
    {
        public Task<Game> Create(Game game);
        public Task<Game> GetById(int id);
        public Task<bool> PatchScore(Game game);
    }
}
