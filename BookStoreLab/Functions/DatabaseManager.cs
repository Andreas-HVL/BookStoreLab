﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreLab.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreLab.Functions
{
    public static class DatabaseManager
    {
        public static void Remove<T>(T entity, MyBookStoreContext context) where T : class
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
    }
}
