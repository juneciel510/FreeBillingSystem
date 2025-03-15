using FluentValidation;
using FreeBilling.Data.Entities;
using FreeBilling.Web.Data;
using FreeBilling.Web.Models;
using FreeBilling.Web.Validators;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace FreeBilling.Web.Apis
{
    public static class TimeBillsApi
    {
        public static void Register(WebApplication app)
        {
            var group=app.MapGroup("/api/timebills")
                .AllowAnonymous();

            group.MapGet("{id:int}", GetTimeBill).WithName("GetTimeBill");

            group.MapPost("", PostTimeBill)
                .AddEndpointFilter<ValidateEndpointFilter<TimeBillModel>>();
        }

        public static async Task<IResult> GetTimeBill(IBillingRepository repository, int id)
        {

            var bill = await repository.GetTimeBill(id);
            if (bill is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(bill);
        }

        public static async Task<IResult> PostTimeBill(IBillingRepository repository,
            TimeBillModel model)
        {
            var newEntity = model.Adapt<TimeBill>();
            //var newEntity =new TimeBill
            //{
            //    EmployeeId = model.EmployeeId,
            //    CustomerId = model.CustomerId,
            //    Hours = model.HoursWorked,
            //    BillingRate = model.Rate,
            //    Date = model.Date,
            //    WorkPerformed = model.Work
            //};
            repository.AddEntity<TimeBill>(newEntity);
            if (await repository.SaveChanges())
            {
                //to get the complete entity with other nested entities: e.g. Customer, Employee
                var newBill = await repository.GetTimeBill(newEntity.Id);
                return Results.CreatedAtRoute("GetTimeBill", new
                {
                    id = model.Id
                }, newBill);
            }
            else
            {
                return Results.BadRequest();
            }
        }
    }
}
