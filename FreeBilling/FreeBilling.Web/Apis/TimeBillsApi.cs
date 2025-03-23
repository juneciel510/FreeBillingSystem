using FluentValidation;
using FreeBilling.Data.Entities;
using FreeBilling.Web.Data;
using FreeBilling.Web.Models;
using FreeBilling.Web.Validators;
using Mapster;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FreeBilling.Web.Apis
{
    //minimal api for timebills
    public static class TimeBillsApi
    {
        public static void Register(WebApplication app)
        {
            var group = app.MapGroup("/api/timebills");

            group.MapGet("{id:int}", GetTimeBill)
                .WithName("GetTimeBill")
                .RequireAuthorization("ApiPolicy");

            group.MapPost("", PostTimeBill)
                .AddEndpointFilter<ValidateEndpointFilter<TimeBillModel>>()
                .RequireAuthorization("ApiPolicy");
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
            TimeBillModel model,
            ClaimsPrincipal user)
        {
            var newEntity = model.Adapt<TimeBill>();
            
            var employee = await repository.GetEmployee(user.Identity?.Name!);

            if(employee is null)
            {
                return Results.BadRequest("No employee with user's email.");
            }

            newEntity.EmployeeId = employee?.Id;

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
