using HotelDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelDataBaseImplement
{
    public class HotelDataBase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=MAKSIM\SQLEXPRESS;Initial Catalog=HotelDataBaseFu;Integrated Security=True;MultipleActiveResultSets=True;;TrustServerCertificate=True");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Conference> Conferences { set; get; }
        public virtual DbSet<Room> Rooms { set; get; }
        public virtual DbSet<MealPlan> MealPlans { set; get; }
        public virtual DbSet<ConferenceBooking> ConferenceBookings { set; get; }
        public virtual DbSet<Member> Members { set; get; }
        public virtual DbSet<Dinner> Dinners { set; get; }
        public virtual DbSet<Organiser> Organisers { set; get; }
        public virtual DbSet<Headwaiter> Headwaiters { set; get; }
        public virtual DbSet<ConferenceMember> ConferenceMembers { set; get; }
        public virtual DbSet<RoomDinner> RoomDinners { set; get; }
        public virtual DbSet<MealPlanMember> MealPlanMembers { set; get; }
        public virtual DbSet<ConferenceBookingDinner> ConferenceBookingDinners { set; get; }

    }
}