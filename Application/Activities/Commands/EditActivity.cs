using System;
using System.Diagnostics;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class EditActivity
{
    public class Command : IRequest
    {
        public required Domain.Activity Activity { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .FindAsync([request.Activity.Id], cancellationToken)
                    ?? throw new Exception("Cannot find activity");

            // Utilize automapper to redefine the properties for the activity in the request
            mapper.Map(request.Activity, activity);
            

            // Save changes to the database 
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}
