using System;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class DeleteActivity
{
    public class Command : IRequest
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            // finding the 
            var activity = await context.Activities
                .FindAsync([request.Id], cancellationToken)
                    ?? throw new Exception("Cannot find activity");

            // reminder this just changes the object in memory not in the database
            context.Remove(activity);

            // saving changes makes any changes applied in memory also get done in the database
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}