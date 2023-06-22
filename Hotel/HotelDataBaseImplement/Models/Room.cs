using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDataBaseImplement.Models
{
    public class Room : IRoomModel
    {
        [Required]
        public string RoomName { get; set; } = string.Empty;
        [Required]
        public string RoomFrame { get; set; } = string.Empty;
        [Required]
        public double RoomPrice { get; set; } 
        public int HeadwaiterId { get; private set; }
        public int? MealPlanId { get; private set; }
        public int Id { get; private set; }

        public virtual Headwaiter Headwaiter { get; set; }
        public virtual MealPlan? MealPlan { get; set; }

        [ForeignKey("RoomId")]
        public virtual List<RoomDinner> Dinners { get; set; }

        private Dictionary<int, IDinnerModel> _roomDinners = null;
        [NotMapped]
        public Dictionary<int, IDinnerModel> RoomDinners
        {
            get
            {
                if (_roomDinners == null)
                {
                    using var context = new HotelDataBase();
                    _roomDinners = Dinners
                        .ToDictionary(x => x.DinnerId, x => (context.Dinners
                        .FirstOrDefault(y => y.Id == x.DinnerId)! as IDinnerModel));
                }
                return _roomDinners;
            }
        }

        public static Room Create(HotelDataBase context, RoomBindingModel model)
        {
            return new Room()
            {
                Id = model.Id,
                RoomName = model.RoomName,
                RoomFrame = model.RoomFrame,
                RoomPrice = model.RoomPrice,
                HeadwaiterId = model.HeadwaiterId,
                MealPlanId = model.MealPlanId,
                Dinners = model.RoomDinners.Select(x => new RoomDinner
                {
                    Dinner = context.Dinners.First(y => y.Id == x.Key),
                }).ToList()
            };
        }

        public void Update(RoomBindingModel model)
        {
            RoomName = model.RoomName;
            RoomFrame = model.RoomFrame;
            RoomPrice = model.RoomPrice;
            HeadwaiterId = model.HeadwaiterId;
            MealPlanId = model.MealPlanId;
        }

        public RoomViewModel GetViewModel => new()
        {
            Id = Id,
            RoomName = RoomName,
            RoomFrame = RoomFrame,
            HeadwaiterId = HeadwaiterId,
            MealPlanId = MealPlanId,
            RoomPrice = RoomPrice,
            RoomDinners = RoomDinners
        };

        public void UpdateDinners(HotelDataBase context, RoomBindingModel model)
        {
            var roomDinners = context.RoomDinners.Where(rec => rec.RoomId == model.Id).ToList();

            if (roomDinners != null && roomDinners.Count > 0)
            {
                context.RoomDinners.RemoveRange(roomDinners.Where(rec => !model.RoomDinners.ContainsKey(rec.DinnerId)));
                context.SaveChanges();

                foreach (var updateDinner in roomDinners)
                {
                    model.RoomDinners.Remove(updateDinner.DinnerId);
                }
                context.SaveChanges();
            }

            var room = context.Rooms.First(x => x.Id == Id);

            foreach (var cm in model.RoomDinners)
            {
                context.RoomDinners.Add(new RoomDinner
                {
                    Room = room,
                    Dinner = context.Dinners.First(x => x.Id == cm.Key),
                });
                context.SaveChanges();
            }
            _roomDinners = null;
        }
    }
}
