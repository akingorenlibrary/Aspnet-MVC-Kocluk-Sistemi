using Kocluk.Dto;
using Kocluk.Models;
using Microsoft.EntityFrameworkCore;

namespace Kocluk.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<KullanicilarinAntrenorleri> KullanicilarinAntrenorleris { get; set; }
        public DbSet<DanisanEgzersizProgrami> DanisanEgzersizProgramlari { get; set; }
        public DbSet<DanisanBeslenmeProgrami> DanisanBeslenmeProgramlari { get; set; }
        public DbSet<Mesajlar> Mesajlar { get; set; }
        public DbSet<IlerlemeDurumu> IlerlemeDurumlari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KullanicilarinAntrenorleri>()
                .HasOne(ka => ka.Kullanici)
                .WithMany(k => k.KullanicilarinAntrenorleris)
                .HasForeignKey(ka => ka.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KullanicilarinAntrenorleri>()
                .HasOne(ka => ka.Antrenor)
                .WithMany()
                .HasForeignKey(ka => ka.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            //

            modelBuilder.Entity<DanisanEgzersizProgrami>()
                .HasOne(dep => dep.Kullanici)
                .WithMany(k => k.DanisanEgzersizProgramlari)
                .HasForeignKey(dep => dep.DanisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanisanEgzersizProgrami>()
                .HasOne(dep => dep.Antrenor)
                .WithMany()
                .HasForeignKey(dep => dep.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            //

            modelBuilder.Entity<DanisanBeslenmeProgrami>()
                .HasOne(dep => dep.Kullanici)
                .WithMany(k => k.DanisanBeslenmeProgramlari)
                .HasForeignKey(dep => dep.DanisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanisanBeslenmeProgrami>()
                .HasOne(dep => dep.Antrenor)
                .WithMany()
                .HasForeignKey(dep => dep.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            //

            modelBuilder.Entity<Mesajlar>()
                .HasOne(dep => dep.Alici)
                .WithMany(k => k.Mesajlar)
                .HasForeignKey(dep => dep.AliciId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesajlar>()
                .HasOne(dep => dep.Gonderen)
                .WithMany()
                .HasForeignKey(dep => dep.GonderenId)
                .OnDelete(DeleteBehavior.Restrict);

            //

            modelBuilder.Entity<IlerlemeDurumu>()
                .HasOne(dep => dep.Kullanici)
                .WithMany(k => k.IlerlemeDurumlari)
                .HasForeignKey(dep => dep.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
