using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LoginRegistrationAPI.Data
{
    public class AUTHDBContext : IdentityDbContext
    {
        public AUTHDBContext(DbContextOptions<AUTHDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "7f415c08-9c69-4bba-abb5-36c943d4310e";
            var writerRoleId = "7da0b544-04eb-4afd-9117-a99ba7b8b2dc";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER"
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
