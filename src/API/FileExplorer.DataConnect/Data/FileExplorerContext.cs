using FileExplorer.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.DataConnect
{
    public class FileExplorerContext : DbContext
    {
        public FileExplorerContext(DbContextOptions<FileExplorerContext> options) : base(options)
        {
        }

        public FileExplorerContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<UserModel> Users { get; set; }
    }
}
