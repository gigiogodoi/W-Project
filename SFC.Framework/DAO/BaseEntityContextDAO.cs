﻿using SFC.Framework.Context;
using System;
using System.Data.Entity;
using System.Web;

namespace SFC.Framework.DAO
{
    public abstract class BaseEntityContextDAO<TContext>
        where TContext : DbContext
    {
        public TContext Context
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    lock (this)
                    {
                        if (HttpContext.Current.Items[typeof(TContext).ToString()] == null)
                            HttpContext.Current.Items[typeof(TContext).ToString()] = Activator.CreateInstance<TContext>();
                    }

                    return (TContext)HttpContext.Current.Items[typeof(TContext).ToString()];
                }
                else if (SimpleContext.Current != null)
                {
                    lock (SimpleContext.Current)
                    {
                        if (!SimpleContext.Current.ContainsKey(typeof(TContext).ToString()))
                            SimpleContext.Current.Add(typeof(TContext).ToString(), Activator.CreateInstance<TContext>());
                    }

                    return (TContext)SimpleContext.Current[typeof(TContext).ToString()];
                }
                else
                    throw new NotImplementedException();
            }
        }

        public void PublishChanges()
        {
            Context.SaveChanges();
        }
    }
}
