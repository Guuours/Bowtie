using Serilog;
using System;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public abstract class BaseEntity
    {
        public abstract bool IsNew(Connection conn = null);

        public virtual bool Save(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (IsNew(conn))
                {
                    OnInserting(conn);
                    conn.Insert(this);
                    OnInserted(conn);
                }
                else
                {
                    OnUpdating(conn);
                    conn.Update(this);
                    OnUpdated(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (IsNew(conn))
                {
                    OnInserting(conn);
                    await conn.InsertAsync(this);
                    OnInserted(conn);
                }
                else
                {
                    OnUpdating(conn);
                    await conn.UpdateAsync(this);
                    OnUpdated(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        public virtual bool Delete(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (!IsNew(conn))
                {
                    OnDeleting(conn);
                    conn.Delete(this);
                    OnDeleted(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (!IsNew(conn))
                {
                    OnDeleting(conn);
                    await conn.DeleteAsync(this);
                    OnDeleted(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        public virtual void OnInserting(Connection conn = null) { }

        public virtual void OnInserted(Connection conn = null) { }

        public virtual void OnUpdating(Connection conn = null) { }

        public virtual void OnUpdated(Connection conn = null) { }

        public virtual void OnDeleting(Connection conn = null) { }

        public virtual void OnDeleted(Connection conn = null) { }
    }

    public abstract class Entity : BaseEntity
    {
        [Column(PK = true, Ignore = When.Insert | When.Update)]
        public virtual long Id { get; set; }

        public override bool IsNew(Connection conn = null)
        {
            return Id == 0;
        }

        public override bool Save(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (IsNew(conn))
                {
                    OnInserting(conn);
                    Id = conn.Insert(this);
                    OnInserted(conn);
                }
                else
                {
                    OnUpdating(conn);
                    conn.Update(this);
                    OnUpdated(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        public async override Task<bool> SaveAsync(Connection conn = null)
        {
            try
            {
                if (conn == null)
                {
                    conn = DB.Default;
                }

                if (IsNew(conn))
                {
                    OnInserting(conn);
                    Id = await conn.InsertAsync(this);
                    OnInserted(conn);
                }
                else
                {
                    OnUpdating(conn);
                    await conn.UpdateAsync(this);
                    OnUpdated(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }
    }
}